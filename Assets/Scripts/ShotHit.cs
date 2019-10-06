using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotHit : MonoBehaviour
{
    public float decrease_speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    SpriteRenderer sr
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (sr.color.a <= 0f)
        {
            return;
        }
            Vector3 decrease = Vector3.one * Time.deltaTime * decrease_speed;
        decrease.z = 0f;
        transform.localScale += decrease;
        Vector3 lr = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.EulerAngles(0, 0, lr.z + 11f);// Time.deltaTime * 100f);
        sr.color -= Color.black * Time.deltaTime * decrease_speed;
        if(sr.color.a <= 0f)
        {
            Destroy(transform.parent.gameObject, 2f);
        }
    }
}
