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

    // Update is called once per frame
    void Update()
    {
        Vector3 decrease = Vector3.one * Time.deltaTime * decrease_speed;
        decrease.z = 0f;
        transform.localScale -= decrease;
        if(transform.localScale.x < 0f)
        {
            Destroy(gameObject);
        }
    }
}
