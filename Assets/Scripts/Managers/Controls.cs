using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
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

        Vector2 ppos = GM.player.player_dummy.transform.position;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction -= ppos;
        direction.Normalize();

        lr.SetPosition(0, ppos);

        lr.SetPosition(1, ppos + direction * 5f);
    }
    [SerializeField]
    float cover_precision_decrease = .5f;
    public void ShootRay()
    {
        int grnd = 1 << LayerMask.NameToLayer("environment");
        int fly = 1 << LayerMask.NameToLayer("enemies");
        int mask = grnd | fly;
        Vector2 ppos = GM.player.player_dummy.transform.position;
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

        GM.game.Hit(hit_position);

        Enemy en = hit.collider.GetComponent<Enemy>();
        if (en != null)
        {
            en.Attacked(GM.player.damage);
        }
        GM.enemies.FireStress(hit_position);
        

    }
}
