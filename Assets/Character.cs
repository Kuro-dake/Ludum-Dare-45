using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public GameObject arm;
    public GameObject gun_flash;
    public Animator mouth;

    public GameObject character_target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    Animator anim
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    public float jolt = 0f;
    public void Shoot()
    {
        Vector3 lrot = arm.transform.localRotation.eulerAngles;
        arm.transform.localRotation = Quaternion.EulerAngles(lrot.x, lrot.y, lrot.z - 1f);
        jolt = 10f;
        gun_flash.GetComponent<SpriteRenderer>().color = Color.white;
        gun_flash.transform.localRotation = Quaternion.EulerAngles(0f, 0f, Random.Range(-1f, 1f));
    }

    
    public bool covering
    {
        set
        {
            anim.SetBool("cover", value);    
        }
    }

    public bool talking
    {
        set
        {
            anim.SetBool("talking", value);
        }
    }

    void Update()
    {
        gun_flash.GetComponent<SpriteRenderer>().color -= Color.black * Time.deltaTime * 10f;

        if (character_target != null)
        {
            Vector2 ppos = arm.transform.position;
            Vector2 mpos = character_target.transform.position;
            Vector2 direction = mpos - ppos;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arm.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        }

            arm.transform.rotation = Quaternion.Euler(0f, 0f, arm.transform.rotation.eulerAngles.z + jolt);

            transform.rotation = Quaternion.Euler(0f, 0f, jolt / 10f);

            jolt = Mathf.Clamp(jolt - Time.deltaTime * 100f, 0f, Mathf.Infinity);
    }

        

        

}
