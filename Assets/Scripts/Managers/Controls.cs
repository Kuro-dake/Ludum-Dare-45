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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GM.player.covering = true;
        } 
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            GM.player.covering = false;

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootRay();
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

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        
        

    }
    [SerializeField]
    float cover_precision_decrease = .5f;
    public void ShootRay()
    {
        int grnd = 1 << LayerMask.NameToLayer("environment");
        int fly = 1 << LayerMask.NameToLayer("enemies");
        int mask = grnd | fly;
        Vector2 ppos = GM.player.player_dummy.arm.transform.position;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction -= ppos;
        direction.Normalize();
        direction *= 5f;
        if (GM.player.covering)
        {
            direction += Random.insideUnitCircle.normalized * cover_precision_decrease;
        }

        RaycastHit2D hit = Physics2D.Raycast(ppos, direction, Mathf.Infinity, mask);
        Vector2 hit_position = hit.point;

        
        if (hit.collider != null)
        {
            GM.game.Hit(hit_position);
            Enemy en = hit.collider.transform.GetComponentInParent<Enemy>();
            if (en != null)
            {
                en.Attacked(GM.player.damage);
            }
            GM.enemies.FireStress(hit_position);
        }
        GM.player.player_dummy.Shoot();
    }
}
