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
    public List<Enemy> alive_enemies
    {
        get { return all_enemies.FindAll(delegate (Enemy en) { return en.alive; }); }
    }

    public List<Enemy> all_enemies
    {
        get
        {
            return _all_enemies;
        }
    }

    public bool any_alive
    {
        get
        {
            return alive_enemies.Count > 0;
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
    Queue<EnemyInfo> info_from_checkpoint = new Queue<EnemyInfo>();
    Coroutine generating_routine;
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

        generating_routine = StartCoroutine(GenerateEnemies());
    }
    public bool activated = true;
    public int level_modifier_dev = 1;
    public void KillAll()
    {
        all_enemies.ForEach(delegate (Enemy obj)
        {
            obj.StopAllCoroutines();
            Destroy(obj.gameObject);
        });
        _all_enemies.Clear();
    }
    public void RestartFromCheckpoint()
    {
        was_dead = true;
        GM.player.player_dummy.ResetAnim();
        KillAll();
        StopCoroutine(generating_routine);
        enemy_info = new Queue<EnemyInfo>(info_from_checkpoint);
        generating_routine = StartCoroutine(GenerateEnemies());
        Debug.Log("should reload");
    }
    public bool was_dead = false;
    [TextArea(10, 15)]
    public string was_dead_string = "";
    IEnumerator GenerateEnemies()
    {
        if (!activated)
        {
            yield break;
        }
        info_from_checkpoint = new Queue<EnemyInfo>(enemy_info);
        while (enemy_info.Count > 0)
        {
            EnemyInfo next_enemy = enemy_info.Peek();
            if (!was_dead)
            {
                yield return new WaitForSeconds(next_enemy.delay);
            }
            if (next_enemy.type == "waitfordead") {
                while (any_alive)
                {
                    yield return null;
                }
            }
            else if (next_enemy.type == "dialogue")
            {
                info_from_checkpoint = new Queue<EnemyInfo>(enemy_info); 
                GM.player.player_dummy.hp = GM.player.hpmax;
                GM.DevoutUpdate();
                GM.cinema.PlayLevelString(int.Parse(next_enemy.target) + level_modifier_dev, was_dead ? was_dead_string : "");
                was_dead = false;
                while (GM.cinema.active)
                {
                    yield return null;
                }
            }
            else if (next_enemy.type == "end")
            {
                GM.game_ended = true;
                yield return null;
            }
            else
            {
                PlaceEnemy(next_enemy.type, next_enemy.entry, next_enemy.target);
                        
            }
            enemy_info.Dequeue();
        }

        generating_routine = null;
    }

    void PlaceEnemy(string type, string entry, string target)
    {
        Enemy e = Instantiate(named_enemies.GetByName(type)).GetComponent<Enemy>();
        e.transform.position = GM.level_manager.current_level.entrances.GetByName(entry).transform.position;
        e.movement_target = e.type == enemy_type.melee ? GM.player.player_dummy.gameObject : GM.level_manager.current_level.targets.GetByName(target);
        e.Initialize();
        e.gameObject.name = type + " " + Random.Range(0, 399);
    }
    [SerializeField]
    float stress_distance_modifier = 3f;

    public void DecreaseFireStress()
    {
        alive_enemies.RemoveAll(delegate (Enemy en)
        {
            return en == null;
        });
        alive_enemies.ForEach(delegate (Enemy en) {

            en.DecreaseFireStress(-1f);
        });
    }

    public void FireStress(Vector3 impact_point)
    {
        alive_enemies.RemoveAll(delegate (Enemy en)
        {
            return en == null;
        });
        alive_enemies.ForEach(delegate (Enemy en) {
            
            en.FireStress(Mathf.Clamp(stress_distance_modifier / Vector2.Distance(en.transform.position, impact_point), 0f, 1f));
        });
    }

}
