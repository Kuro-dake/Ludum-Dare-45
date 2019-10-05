using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public int ammo = 5;
    public int damage = 1;
    public int hp = 3;

    public Character player_dummy;

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

    public void Attacked(int damage, enemy_type type)
    {
        if (type == enemy_type.melee || type == enemy_type.ranged && !covering)
        {
            hp -= damage;
        }
        GM.DevoutUpdate();
        if(hp <= 0)
        {
            GM.ReloadScene();
        }
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