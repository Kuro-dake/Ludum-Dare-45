using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public int ammo = 5;
    public int damage = 1;
    public int hp = 3;

    public int Shoot()
    {
        if(ammo <= 0)
        {
            return 0;
        }
        ammo = Mathf.Clamp(ammo - 1, 0, ammo);
        GM.DevoutUpdate();
        return damage;
    }

    public void Attacked(int damage)
    {
        hp -= damage;
        GM.DevoutUpdate();
        if(hp <= 0)
        {
            GM.ReloadScene();
        }
    }
}
