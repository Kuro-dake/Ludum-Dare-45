using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{

    public scene_object_type type;
    public string identifier;

    public static Dictionary<string, SceneObject> scene_objects = new Dictionary<string, SceneObject>();

    public static void InitializeLevel()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum scene_object_type
{
    entry,
    cover,
    player_cover,
    destructible
}