using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    static GM _inst;
    public static GM inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.Find("GM").GetComponent<GM>();
            }
            return _inst;
        }
    }

    public GameObject this[string s]
    {
        get
        {
            Transform[] ret = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform g in ret)
            {
                if (g.gameObject.name == s)
                {
                    return g.gameObject;
                }
            }
            throw new UnityException(s + " not found");

        }
    }
    
   
    static Dictionary<string, Component> scripts = new Dictionary<string, Component>();

    public static T GetScript<T>(string name) where T : Component
    {

        string script_key = name + typeof(T).ToString();
        if (!scripts.ContainsKey(script_key))
        {
            T ret = inst[name].GetComponent<T>();
            if (ret == null)
            {
                throw new UnityException("script " + script_key + " not found");
            }
            scripts.Add(script_key, ret);
        }
        return scripts[script_key] as T;
    }

    public static LevelManager level_manager { get { return GetScript<LevelManager>("LevelManager"); } }
    public static EnemyManager enemies { get { return GetScript<EnemyManager>("EnemyManager"); } }
    static Text devout { get { return GetScript<Text>("DevOut"); } }
    public static void DevoutUpdate()
    {
        devout.text = "ammo : " + GM.player.ammo.ToString() + " ; HP : " + GM.player.player_dummy.hp;
    }
    public static PlayerData player { get { return GetScript<PlayerData>("PlayerData"); } }

    

    public static GameManager game { get { return GetScript<GameManager>("GameManager"); } }
    public static Controls controls { get { return GetScript<Controls>("Controls"); } }

   /* public static Cinema cinema { get { return GetScript<Cinema>("Cinema"); } }
    public static TitleScreen title { get { return GetScript<TitleScreen>("TitleScreen"); } }

    public static GameObject canvas { get { return inst["Canvas"]; } }
    public static DialogueContainer dialogues { get { return GetScript<DialogueContainer>("Dialogues"); } }

    public static DynamicUI dynamic_ui { get { return GetScript<DynamicUI>("DynamicUI"); } }*/


    private void Start()
    {
        enemies.StartGeneratingEnemies();
    }

    public static void ReloadScene()
    {
        //Enemy.KillAll();
        scripts.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
