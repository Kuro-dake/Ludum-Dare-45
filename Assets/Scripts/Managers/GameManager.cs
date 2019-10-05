using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ShotHit shot_hit;

    public void Hit(Vector2 point)
    {
        Instantiate(shot_hit, point, Quaternion.identity);
    }

}
