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
    public void ResetAnim()
    {
        anim.SetBool("dead", false);
    }
    public override void Attacked(int damage)
    {
        
        if (!covering)
        {
            hp = Mathf.Clamp(hp-damage, 0, GM.player.hpmax);
        }
        GM.DevoutUpdate();
        if (hp <= 0)
        {
            anim.SetBool("dead", true);
            //GM.ReloadScene();
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
