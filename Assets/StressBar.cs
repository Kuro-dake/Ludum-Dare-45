using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressBar : HPBar
{
    protected override Vector3 position_modifier {
        get
        {
            return Vector3.up * position_distance;
        }
    }

    public void DisplayFloat(float num)
    {
        Display(Mathf.CeilToInt(num));
        if (num > 0f)
        {
            transform.GetChild(transform.childCount - 1).localScale = Vector3.one * (num - Mathf.FloorToInt(num));
        }
    }
}
