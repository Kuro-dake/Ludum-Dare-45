using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject shot_hit;
    
    public void Hit(Vector2 point, Vector2 from, bool blood)
    {
        
        GameObject nsh = Instantiate(shot_hit, point, Quaternion.identity);
        Transform blood_particle_obj = nsh.transform.FindChild("particle");
        if (!blood) {
            Destroy(blood_particle_obj.gameObject);
        }

        Vector2 ppos = point;
        Vector2 mpos = from;
        Vector2 direction = mpos - ppos;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        blood_particle_obj.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
        blood_particle_obj.transform.position += blood_particle_obj.transform.right / 3f;
    }

}
