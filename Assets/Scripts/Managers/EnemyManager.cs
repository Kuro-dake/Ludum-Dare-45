using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YamlDotNet.RepresentationModel;
public class EnemyManager : MonoBehaviour
{

    public NamedObjects named_enemies = new NamedObjects();
    public HPBar hpbar;
    public StressBar stressbar;


    List<Enemy> _all_enemies = new List<Enemy>();
    public List<Enemy> all_enemies
    {
        get { return _all_enemies; }
    }
    public bool any_alive
    {
        get
        {
            return all_enemies.Count > 0;
        }
    }
    public void AddEnemy(Enemy en)
    {
        _all_enemies.Add(en);
    }
    public void RemoveEnemy(Enemy en)
    {
        _all_enemies.Remove(en);
    }

    private void Update()
    {
        
    }

    Queue<EnemyInfo> enemy_info = new Queue<EnemyInfo>();

    public void StartGeneratingEnemies() {
        foreach (YamlMappingNode mn in Setup.GetFile("level1").GetNode<YamlSequenceNode>("enemies"))
        {
            enemy_info.Enqueue(new EnemyInfo {
                entry = mn.Get("entry"), 
                type = mn.Get("type"), 
                delay = mn.GetFloat("delay"), 
                target = mn.Get("target")
                }

            );
        }

        StartCoroutine(GenerateEnemies());
    }

    IEnumerator GenerateEnemies()
    {
        

        while (enemy_info.Count > 0)
        {
            EnemyInfo next_enemy = enemy_info.Dequeue();

            yield return new WaitForSeconds(next_enemy.delay);
            if (next_enemy.type != "waitfordead") { 
                PlaceEnemy(next_enemy.type, next_enemy.entry, next_enemy.target);
            }
            else
            {
                while (any_alive)
                {
                    yield return null;
                }
            }
            
        }

        Debug.Log("routine done");
    }

    void PlaceEnemy(string type, string entry, string target)
    {
        Enemy e = Instantiate(named_enemies.GetByName(type)).GetComponent<Enemy>();
        e.transform.position = GM.level_manager.current_level.entrances.GetByName(entry).transform.position;
        e.movement_target = GM.level_manager.current_level.targets.GetByName(target);
        e.Initialize();
        e.gameObject.name = type + " " + Random.Range(0, 399);
    }
    [SerializeField]
    float stress_distance_modifier = 3f;

    public void FireStress(Vector3 impact_point)
    {
        all_enemies.ForEach(delegate (Enemy en) {
            Debug.Log(en.name + " " + Vector2.Distance(transform.position, impact_point));
            en.FireStress(Mathf.Clamp(stress_distance_modifier / Vector2.Distance(en.transform.position, impact_point), 0f, 1f));
        });
    }

}
