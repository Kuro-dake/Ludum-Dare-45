using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int hp;
    public int damage = 1;
    public GameObject movement_target;

    public float attack_delay = 1f;

    public enemy_type type;

    public float movement_speed = 2f;

    public float stress = 0f;
    public float stress_resistance = 1f;
    public float max_stress = 3f;

    bool _covering = false;
    Vector3 orig_size;
    bool covering
    {
        get
        {
            return _covering;
        }
        set
        {
            _covering = value;
            transform.localScale = value ? new Vector3(orig_size.x, orig_size.y / 2f) : orig_size;
        }
    }

    GameObject _crosshair;
    GameObject crosshair
    {
        get
        {
            if(_crosshair == null)
            {
                _crosshair = transform.Find("crosshair").gameObject;
            }
            return _crosshair;
        }
    }

    public bool crosshair_visible
    {
        get
        {
            return crosshair.activeSelf;
        }

        set
        {
            crosshair.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    Coroutine act_routine;
    HPBar hpbar;
    StressBar stressbar;
    public void Initialize()
    {
        orig_size = transform.localScale;
        GM.enemies.AddEnemy(this);

        hpbar = Instantiate(GM.enemies.hpbar).GetComponent<HPBar>();
        hpbar.Init(gameObject);
        hpbar.Display(hp);

        stressbar= Instantiate(GM.enemies.stressbar).GetComponent<StressBar>();
        stressbar.Init(gameObject);
        stressbar.DisplayFloat(stress);


        act_routine = StartCoroutine(Act());
    }
    
    /// <summary>
    /// Should set all enemies to dead : doesn't have reason to exist right now, since logic behind alive/dead was changed
    /// @TODO: Remove if obsolete
    /// </summary>
    public static void KillAll()
    {
        GM.enemies.all_enemies.ForEach(delegate (Enemy obj)
        {
            obj.alive = false;
        });
    }
    float last_attack = 0f;
    IEnumerator Attack()
    {
        last_attack = attack_delay;
        while(last_attack > 0f)
        {
            if (!covering)
            {
                last_attack -= Time.deltaTime;
            }
            yield return null;
        }
        
        if (!alive)
        {
            yield break;
        }
        GM.player.Attacked(damage, type);
        StartCoroutine(DevAttackAnimation());
    }

    IEnumerator DevAttackAnimation()
    {
        Vector3 prevscale = transform.localScale;
        transform.localScale *= 2f;
        while(transform.localScale.x > prevscale.x)
        {
            transform.localScale -= Time.deltaTime * prevscale;
            yield return null;
        }
        transform.localScale = prevscale;
    }
    bool _alive = true;
    public bool alive
    {
        get
        {
            return _alive;
        }
        set
        {
            if (!value && _alive)
            {
                GM.enemies.RemoveEnemy(this);
                if (act_routine != null)
                {
                    StopCoroutine(act_routine);
                }
                transform.Rotate(Vector3.back * 90f);
                gameObject.layer = 0;
                //Destroy(gameObject);
            }
            _alive = value;
        }
    }
    IEnumerator Act()
    {
        while (Vector2.Distance(transform.position, movement_target.transform.position) > .5f)
        {
            if (!covering)
            {
                transform.position = Vector2.MoveTowards(transform.position, movement_target.transform.position, Time.deltaTime * movement_speed);
            }
            yield return null;
        }

        while (true)
        {
            yield return StartCoroutine(Attack());
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        stress -= Time.deltaTime / 2f;
        covering = stress > 1f;
        if(stress > 0f) {
            stressbar.DisplayFloat(stress);
        }
        
    }

    private void OnMouseEnter()
    {
        crosshair_visible = true;
    }

    private void OnMouseExit()
    {
        crosshair_visible = false;
    }

    private void OnMouseDown()
    {
        //Attacked();
    }

    public void Attacked(int damage)
    {
        if ((hp -= damage) < 1)
        {
            alive = false;
        }
        hpbar.Display(hp);
        
        return;

        switch (GM.player.Shoot())
        {
            case damage_type.physical:
                
                break;
            case damage_type.stress:
                stress = Mathf.Clamp(stress + 1f / stress_resistance, 0f, max_stress);
                break;
        }
        
    }

    public void FireStress(float distance_modifier)
    {
        stress = Mathf.Clamp(stress + distance_modifier / stress_resistance, 0f, max_stress);
    }
}

public enum enemy_type
{
    melee,
    ranged
}