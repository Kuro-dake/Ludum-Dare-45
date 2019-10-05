using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int hp;
    public int damage = 1;
    public GameObject movement_target;

    public float attack_delay = 1f;

    GameObject _crosshair;
    GameObject crosshair
    {
        get
        {
            if(_crosshair == null)
            {
                _crosshair = transform.Find("crosshair").gameObject;
            }
            return _crosshair;
        }
    }

    public bool crosshair_visible
    {
        get
        {
            return crosshair.activeSelf;
        }

        set
        {
            crosshair.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        all_enemies.Add(this);
        StartCoroutine(Act());
    }
    static List<Enemy> all_enemies = new List<Enemy>();
    public static void KillAll()
    {
        all_enemies.ForEach(delegate (Enemy obj)
        {
            obj.alive = false;
        });
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attack_delay);
        GM.player.Attacked(damage);
        StartCoroutine(DevAttackAnimation());
    }

    IEnumerator DevAttackAnimation()
    {
        Vector3 prevscale = transform.localScale;
        transform.localScale *= 2f;
        while(transform.localScale.x > prevscale.x)
        {
            transform.localScale -= Time.deltaTime * prevscale;
            yield return null;
        }
        transform.localScale = prevscale;
    }
    public bool alive = true;
    IEnumerator Act()
    {
        while (Vector2.Distance(transform.position, movement_target.transform.position) > .5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, movement_target.transform.position, Time.deltaTime);
            yield return null;
        }

        while (alive)
        {
            yield return StartCoroutine(Attack());
        }
        all_enemies.Remove(this);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        crosshair_visible = true;
    }

    private void OnMouseExit()
    {
        crosshair_visible = false;
    }

    private void OnMouseDown()
    {
        Attacked();
    }

    void Attacked()
    {
        
        if ((hp -= GM.player.Shoot()) < 1)
        {
            alive = false;
        }
    }
}
