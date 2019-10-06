using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public int hp;
    public int damage = 1;

    public GameObject arm;
    public float arm_z_rotation;
    public GameObject gun_flash;
    public Animator mouth;

    public GameObject character_target;

    // Start is called before the first frame update
    void Start()
    {
        starting_scale = transform.localScale;
    }

    
    protected Animator anim
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    public float jolt = 0f;
    public void Shoot()
    {
        Vector3 lrot = arm.transform.localRotation.eulerAngles;
        arm.transform.localRotation = Quaternion.EulerAngles(lrot.x, lrot.y, lrot.z - 1f);
        jolt = 10f;
        gun_flash.GetComponent<SpriteRenderer>().color = Color.white;
        gun_flash.transform.localRotation = Quaternion.EulerAngles(0f, 0f, Random.Range(-1f, 1f));
    }

    bool _covering = false;
    public bool covering
    {
        get
        {
            return _covering;
        }
            set
        {
            _covering = value;
            anim.SetBool("cover", value);
            if (is_player) { 
                gunfire_spread = value ? GM.controls.cover_precision_decrease : 0f;
            }
            foreach (BoxCollider2D pc2d in transform.GetComponentsInChildren<BoxCollider2D>())
            {
                pc2d.gameObject.layer = value ? 0 : LayerMask.NameToLayer(layer_name);
            }

        }
    }
    public virtual string layer_name
    {
        get
        {
            return "enemies";
        }
    }
    public bool talking
    {
        set
        {
            anim.SetBool("talking", value);
        }
    }

    Vector3 starting_scale;

    void CorrectFacing()
    {

        int direction = 0;
        float z = arm_z_rotation;
        /*if (gameObject.name == "player") {
            Debug.Log("correct facing " + z + " time " + Time.time);
        }*/

        if ( z > 122f || z < -58f)
        {
            direction = -1;
        }

        if (z < 58f && z > -58f)
        {
            direction = 1;
        }

        if (direction != 0)
        {
           transform.localScale = new Vector3(starting_scale.x * direction, starting_scale.y, starting_scale.z) ;
        }

        /*
        if (gameObject.name == "player")
        {
            Debug.Log(arm.transform.rotation.eulerAngles.z);
        }*/

        

    }
    protected virtual bool aim
    {
        get { return true; }
    }
    protected virtual void Update()
    {
        gun_flash.GetComponent<SpriteRenderer>().color -= Color.black * Time.deltaTime * 10f;

        if (character_target != null && aim)
        {

            Vector2 ppos = arm.transform.position;
            Vector2 mpos = character_target.transform.position;
            Vector2 direction = mpos - ppos;
            arm_z_rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            arm.transform.rotation = Quaternion.Euler(0f, 0f, arm_z_rotation + 90);

        }

            CorrectFacing();

            Vector3 rot = arm.transform.rotation.eulerAngles;

            arm.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z + jolt);

            rot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rot.x, rot.y, jolt / 10f);

            jolt = Mathf.Clamp(jolt - Time.deltaTime * 100f, 0f, Mathf.Infinity);
    }
    public bool is_player
    {
        get
        {
            return GetComponent<Player>() != null;
        }
    }

    public void FadeOut(bool destroy = true)
    {
        StartCoroutine(FadeOutStep(destroy));
    }

    IEnumerator FadeOutStep(bool destroy) {

        List<SpriteRenderer> rends = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        if(rends.Count == 0)
        {
            yield break;
        }

        while(rends[0].color.a > 0)
        {
            rends.RemoveAll(delegate (SpriteRenderer sr) { return sr == null; });
            rends.ForEach(delegate (SpriteRenderer sr)
            {
                sr.color -= Color.black * Time.deltaTime;
            });
            yield return null;
        }
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    public float gunfire_spread = 0f;
    public void ShootRay()
    {

        
        int grnd = 1 << LayerMask.NameToLayer("environment");
        int fly = 1 << LayerMask.NameToLayer(is_player ? "enemies" : "player");
        int mask = grnd | fly;
        Vector2 ppos = arm.transform.position;
        
        Vector2 direction =   is_player ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : GM.player.player_dummy.arm.transform.position;
        direction -= ppos;
        direction.Normalize();
        direction *= 5f;
        
        direction += Random.insideUnitCircle.normalized * gunfire_spread;
        

        RaycastHit2D hit = Physics2D.Raycast(ppos, direction, Mathf.Infinity, mask);
        Vector2 hit_position = hit.point;


        if (hit.collider != null)
        {
            bool blood = false; 
            Character ch = hit.collider.transform.GetComponentInParent<Character>();
            if (ch != null)
            {
                blood = true;
                ch.Attacked(damage);
                ch.FireStress(3f);
            }
            else if (is_player)
            {
                GM.enemies.FireStress(hit_position);
            }
            GM.game.Hit(hit_position, ppos, blood);
        }
        Shoot();
    }
    public virtual void FireStress(float distance_modifier)
    {
        
    }
    public virtual void Attacked(int damage)
    {

    }

}
