using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public bool line_on = true;
    LineRenderer lr
    {
        get
        {
            return GetComponent<LineRenderer>();
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            GM.enemies.StartGeneratingEnemies();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (GM.player.player_dummy.hp > 0) { 
                GM.player.covering = true;
            }
        } 
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            GM.player.covering = false;

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GM.player.player_dummy.hp <= 0)
            {
                GM.enemies.RestartFromCheckpoint();
            }
            else
            {
                GM.player.player_dummy.ShootRay();
            }
        }

        Character pchar = GM.player.player_dummy;
        GameObject arm = pchar.arm;

        Vector2 ppos = arm.transform.position;
        Vector2 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mpos - ppos;
        direction.Normalize();
        if (line_on)
        {
            lr.enabled = true;
            lr.SetPosition(0, ppos);
            lr.SetPosition(1, ppos + direction * 5f);
        }
        else
        {
            lr.enabled = false;
        }

        if(Vector2.Distance(arm.transform.position, mpos) > 1f)
        {
            pchar.arm_z_rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.Euler(0f, 0f, pchar.arm_z_rotation + 90);
        }
        

        
        

    }
    [SerializeField]
    public float cover_precision_decrease = .5f;
    
}
