using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    GameObject follow;
    [SerializeField]
    float width = 2f;
    [SerializeField]
    protected GameObject dot;
    protected virtual Vector3 position_modifier
    {
        get
        {
            return Vector3.down * position_distance;
        }
    }

    public float position_distance = .4f;

    public void Init(GameObject f)
    {
        follow = f;
        transform.SetParent(f.transform);
    }

    public void Display(int num)
    {
        transform.DestroyChildren();
        float step = width / num;
        float start_point = width / -2f + step / 2f;
        for(int i = 0; i<num; i++)
        {
            GameObject ndot = Instantiate(dot);
            ndot.transform.SetParent(transform, true);
            ndot.transform.localPosition = Vector3.left * (start_point + i * step);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position + position_modifier;
    }
}
