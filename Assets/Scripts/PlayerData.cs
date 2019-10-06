using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public int ammo = 5;
    public int hpmax = 10;
    
    public Player player_dummy;

    public bool covering
    {
        get
        {
            return player_dummy.covering;
        }
        set
        {
            player_dummy.covering = value;
            
        }
    }

    public damage_type Shoot()
    {
        if(ammo <= 0)
        {
            return damage_type.none;
        }
        ammo = Mathf.Clamp(ammo - 1, 0, ammo);
        GM.DevoutUpdate();
        return covering ? damage_type.stress : damage_type.physical;
    }

    

    
    private void Start()
    {
    
    }

}

public enum damage_type
{
    physical,
    stress,
    none
}