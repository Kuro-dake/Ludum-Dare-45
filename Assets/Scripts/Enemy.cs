using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    
    public GameObject movement_target;

    public enemy_type type;

    public float movement_speed = 2f;

    public float stress = 0f;
    public float stress_resistance = 1f;
    public float max_stress = 3f;

    Vector3 orig_size;
    protected override void CorrectFacing()
    {
        if (type == enemy_type.ranged)
        {
            base.CorrectFacing();
        }
        else
        {
            
            int direction = 1;
            if (movement_target.transform.position.x < transform.position.x)
            {
                direction = -1;
            }

            if (direction != 0)
            {
                transform.localScale = new Vector3(starting_scale.x * direction, starting_scale.y, starting_scale.z);
            }
        }
    }
    Character character
    {
        get
        {
            return GetComponent<Character>();
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

    
    Coroutine act_routine;
    HPBar hpbar;
    StressBar stressbar;
    public void Initialize()
    {
        orig_size = transform.localScale;
        GM.enemies.AddEnemy(this);
        character.character_target = GM.player.player_dummy.transform.Find("torso").gameObject;
        hpbar = Instantiate(GM.enemies.hpbar).GetComponent<HPBar>();
        hpbar.Init(gameObject);
        hpbar.Display(hp);

        stressbar= Instantiate(GM.enemies.stressbar).GetComponent<StressBar>();
        stressbar.Init(gameObject);
        stressbar.DisplayFloat(stress);


        act_routine = StartCoroutine(Act());

        anim.SetBool("melee", type == enemy_type.melee);

    }
    
    /// <summary>
    /// Should set all enemies to dead : doesn't have reason to exist right now, since logic behind alive/dead was changed
    /// @TODO: Remove if obsolete
    /// </summary>
    
    float last_attack = 0f;
    public float attack_delay = 1f;

    public float salve_between_shots_delay = .2f;
    public int salve_number = 3;

    IEnumerator Attack()
    {
        
        for (int i = 0; i < salve_number; i++)
        {
            if (!alive)
            {
                yield break;
            }
            if (type == enemy_type.ranged)
            {
                character.ShootRay();
            }
            else
            {
                anim.SetTrigger("attack");
                GM.player.player_dummy.Attacked(damage);
            }

            yield return new WaitForSeconds(salve_between_shots_delay);
        }
        last_attack = attack_delay;
        while (last_attack > 0f)
        {
            if (!covering)
            {
                last_attack -= Time.deltaTime;
            }
            yield return null;
        }
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
                StopAllCoroutines();
                FadeOut();
                anim.SetBool("dead", true);
                
            }
            _alive = value;
        }
    }
    IEnumerator Act()
    {
        float desired_distance = type == enemy_type.melee ? 1.5f : .5f;
        while (Vector2.Distance(transform.position, movement_target.transform.position) > desired_distance)
        {
            if (!covering)
            {
                transform.position = Vector2.MoveTowards(transform.position, movement_target.transform.position, Time.deltaTime * movement_speed);
            }
            anim.SetBool("walking", true);
            yield return null;
        }
        anim.SetBool("walking", false);

        while (true)
        {
            yield return StartCoroutine(Attack());
        }
       
    }

    protected override void Update()
    {
        
        stress -= Time.deltaTime / 2f;
        covering = stress > 1f;
        if(stress > 0f) {
            stressbar.DisplayFloat(stress);
        }
        if(stress < 0f)
        {
            stress = 0f;
        }
        base.Update();

    }

    private void OnMouseEnter()
    {
        //crosshair_visible = true;
    }

    private void OnMouseExit()
    {
        //crosshair_visible = false;
    }

    private void OnMouseDown()
    {
        //Attacked();
    }

    public override void Attacked(int damage)
    {
        if (covering)
        {
            return;
        }
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
    protected override bool aim {
        get {
            return type != enemy_type.melee;
        }
    }
    public void DecreaseFireStress(float how_much)
    {
        
    }

    public override void FireStress(float distance_modifier)
    {
        if(type == enemy_type.melee)
        {
            return;
        }
        stress = Mathf.Clamp(stress + distance_modifier / stress_resistance, 0f, max_stress);
    }
}

public enum enemy_type
{
    melee,
    ranged
}