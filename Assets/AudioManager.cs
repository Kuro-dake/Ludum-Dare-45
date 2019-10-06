using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public List<NamedAudioClip> clips = new List<NamedAudioClip>();
 
    public GameObject as_template;

    AudioSource ads {
        get{
        return GetComponent<AudioSource>();
        }
}

    public void PlaySound(string sound_name)
    {
        ads.pitch = Random.Range(.9f, 1.1f);
        ads.PlayOneShot(clips.Find(delegate (NamedAudioClip ac)
        {
            return ac.first == sound_name;
        }).second);
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
