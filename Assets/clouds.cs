using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clouds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float cloud_speed = .1f;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * cloud_speed;
    }
}
