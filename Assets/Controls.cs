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
            GM.player.ShootRay();
        }

        Vector2 ppos = GM.player.player_dummy.transform.position;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction -= ppos;
        direction.Normalize();

        lr.SetPosition(0, ppos);

        lr.SetPosition(1, ppos + direction * 5f);
    }
}
