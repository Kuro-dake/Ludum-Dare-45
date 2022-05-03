using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    NamedObjects actors = new NamedObjects();
    [SerializeField]
    Text dialogue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            GM.audio.PlaySound("hit_flesh");
            Progress();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse1))
        {

            Progress(true);
        }
    }
    string speech
    {
        get
        {
            return dialogue.text;
        }
        set
        {
            dialogue.text = value;
        }
    }

    [TextArea(10, 15)]
    public List<string> level_intros = new List<string>();

    public void PlayLevelString(int string_num, string prefix = "")
    {
        active = true;
        LoadLevelIntros();
        PlayString(prefix + level_intros[string_num]);
    }

    public void LoadLevelIntros()
    {
        if (level_intros.Count != 0)
        {
            return;
        }

        string text = Resources.Load<TextAsset>("dialogues/dialogues").text;
        level_intros = new List<string>(text.Split(new string[] { "\n--\n" }, System.StringSplitOptions.RemoveEmptyEntries));
    }
    string[] lines;
    int current_line = 0;
    public bool active
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            GM.devout_visible = !value;
            gameObject.SetActive(value);
           // GM.canvas.SetActive(!value);
        }
    }

    public void Initialize()
    {
        GetComponentInChildren<Canvas>().enabled = true;
        actors.objects.ForEach(delegate (NamedOjbectType not)
        {
            not.second.SetActive(false);
        });
        left_actor = right_actor = null;
    }

    public void PlayString(string s)
    {

        /*
                Initialize();

                active = true;
                transform.parent.localScale = Vector3.one * GM.inst.cam_size_to_cinema_scale_and_genie_positions[GM.cam.ortosize][0]; // I'm going to hell for this as well
                MoveTo(lord, g_gone + Vector3.right * 25f, true);
                if (cinema_phase == 1)
                {
                    MoveTo(peasant, p_gone, true);
                    MoveTo(genie, g_gone, true);

                    MoveTo(peasant, p_talk);
                    MoveTo(genie, g_talk);
                }
                */
        Initialize();
        lines = s.Split(new char[] { '|' });
        current_line = 0;
        Progress();
    }
    Coroutine progress_routine;
    int cinema_phase = 0;
    actor left_actor, right_actor;
    [SerializeField]
    Transform left_anchor, right_anchor;
    Vector3 la_pos { get { return left_anchor.transform.position; } }
    Vector3 ra_pos { get { return right_anchor.transform.position; } }
    Vector3 la_gone_pos { get { return left_anchor.transform.position + (Vector3.left * 10f); } }
    Vector3 ra_gone_pos { get { return right_anchor.transform.position + (Vector3.right * 10f); } }
    public bool Progress(bool force_end = false)
    {

        /*if (progress_routine != null)
        {
            return true;
        }*/
        
        if (force_end)
        {
            current_line = lines.Length;
        }
        if (current_line >= lines.Length)
        {
            if (!GM.game_ended)
            {
                active = false;
            }
            return false;
         }

        progress_routine = StartCoroutine(ProgressStep(lines[current_line]));

        current_line++;
        return true;
    }
    AudioSource audio
    {
        get
        {
            return GetComponent<AudioSource>(); 
        }
    }
    string TextToFileName(string actor, string text)
    {
        new List<string>() { "<color=green>", "<b>", "</color>", "</b>", "=", "<", ">", "\n", " ", ".", ",", "!", "*", "?", "'" }.ForEach(delegate (string find)
        {
            text = text.Replace(find, "");
        });
        int namelen = 25;
        return actor + "_" + text.Substring(0, text.Length < namelen ? text.Length : namelen).ToLower();
    }
    void StopActorsTalking()
    {
        if (left_actor != null)
        {
            left_actor.talking = false;
        }
        if (right_actor != null)
        {
            right_actor.talking = false;
        }
        audio.Stop();
    }
    Dictionary<string, string> names = new Dictionary<string, string>() {
        { "butch", "Butcher" },
        { "player", "Timmy" },
        { "playergun", "armed Timmy" },
        { "karol", "Karl" },
        { "cutter", "A guy with a tiny knife" },
        { "carrot", "Narrot" },
        { "hans", "Hans Hansonson" },
        { "gunner", "A guy with a gun" },

    };
    Coroutine talkwatch_routine;
    IEnumerator ProgressStep(string line)
    {
        if (talkwatch_routine != null)
        {
            StopCoroutine(talkwatch_routine);
        }
        string[] line_params = line.Split(new char[] { ':' });
        string[] side_id = line_params[0].ToLower().Split(new char[] { ';' });

        string text = line_params[1];
        string char_id = side_id[0];


        actor actor = char_id == "none" ? null : actors.GetByName(char_id).GetComponent<actor>();
        StopActorsTalking();
        if (actor != null)
        {
            actor.gameObject.SetActive(true);
            actor.talking = true;
        }
        if (side_id[1] == "l")
        {
            if (left_actor != null && left_actor != actor || actor == null)
            {
                left_actor.Goto(la_pos, la_gone_pos, true);
            }
            if (actor != null)
            {
                Vector3 ls = actor.transform.localScale;
                actor.transform.localScale = new Vector3(Mathf.Abs(ls.x), ls.y, ls.z);

                if (left_actor != actor)
                {
                    actor.Goto(la_gone_pos, la_pos);
                }
                
            }
            left_actor = actor;


        }
        else
        {
            if (right_actor != null && right_actor != actor || actor == null)
            {
                right_actor.Goto(ra_pos, ra_gone_pos, true);
            }
            if (actor != null)
            {
                Vector3 ls = actor.transform.localScale;
                actor.transform.localScale = new Vector3(Mathf.Abs(ls.x) * -1f, ls.y, ls.z);

                if (right_actor != actor)
                {
                    actor.Goto(ra_gone_pos, ra_pos);
                }
                
            }
            right_actor = actor;
        }


        string textname = names.ContainsKey(char_id) ? names[char_id] : char_id;
        speech = "<b>" + textname + "</b>\n\n" + text;// + ";fn:"+ TextToFileName(char_id, text);

        AudioClip clip = (AudioClip)Resources.Load("Audio/Speech/"+ TextToFileName(char_id, text));
        if (clip != null)
        {
            audio.Stop();
            audio.PlayOneShot(clip);
            
        }
        talkwatch_routine = StartCoroutine(WatchTalking(clip == null ? text.Length : -1));

        yield return null;

        progress_routine = null;

        if (text.Length == 0)
        {
            Progress();
        }
    }

    IEnumerator WatchTalking(int length)
    {
        if (length == -1)
        {
            while (audio.isPlaying)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(length * .1f);
        }
        StopActorsTalking();
        talkwatch_routine = null;
    }

    // Start is called before the first frame update
    

}
