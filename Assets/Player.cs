using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
    }

    public override void Attacked(int damage)
    {
        
        if (!covering)
        {
            hp -= damage;
        }
        GM.DevoutUpdate();
        if (hp <= 0)
        {
            GM.ReloadScene();
        }
        
    }

    public override string layer_name
    {
        get
        {
            return "player";
        }
    }

}
