using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public int ammo = 5;
    public int damage = 1;
    public int hp = 3;

    public GameObject player_dummy;

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
            Vector3 ls = player_dummy.transform.localScale;
            player_dummy.transform.localScale = value ? new Vector3(ls.x, ls.y / 2f, ls.z) : starting_scale;
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

    Vector3 starting_scale = Vector3.one;
    private void Start()
    {
        starting_scale = player_dummy.transform.localScale;
    }

    public void ShootRay()
    {
        int grnd = 1 << LayerMask.NameToLayer("environment");
        int fly = 1 << LayerMask.NameToLayer("enemies");
        int mask = grnd | fly;
        Vector2 ppos = player_dummy.transform.position;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction -= ppos;
        
        Debug.DrawRay(ppos, direction, Color.red, 3f);
        RaycastHit2D hit = Physics2D.Raycast(ppos, direction , Mathf.Infinity, mask);
        Instantiate(player_dummy).transform.position = hit.point;
        Debug.Log(hit.collider.gameObject.name);

    }
}

public enum damage_type
{
    physical,
    stress,
    none
}