using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actor : MonoBehaviour
{

    public Animator mouth;

    Animator anim
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    private void Start()
    {
        anim.SetBool("acting", true);
        anim.speed = Random.Range(.9f, 1.1f);
    }

    public bool talking
    {
        set
        {
            mouth.SetBool("talking", value);
        }
    }
    Coroutine movement_routine;
    public void Goto(Vector3 from, Vector3 to, bool disable = false)
    {
        anim.SetBool("acting", true);
        if (movement_routine != null)
        {
            StopCoroutine(movement_routine);
        }
        movement_routine = StartCoroutine(GotoStep(from, to,disable));
    }

    IEnumerator GotoStep(Vector3 from, Vector3 to, bool disable = false)
    {
        transform.position = from;
        while(Vector2.Distance(transform.position, to) > .1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, to, Time.deltaTime * 20f);
            yield return null;
        }
        movement_routine = null;
        if (disable)
        {
            gameObject.SetActive(false);
        }
    }
}
