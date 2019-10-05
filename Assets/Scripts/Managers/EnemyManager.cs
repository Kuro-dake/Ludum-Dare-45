using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YamlDotNet.RepresentationModel;
public class EnemyManager : MonoBehaviour
{

    public NamedObjects named_enemies = new NamedObjects();

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
        //yield return new WaitForSeconds(Random.RandomRange(2f, 4f));

        while (enemy_info.Count > 0)
        {
            EnemyInfo next_enemy = enemy_info.Dequeue();

            yield return new WaitForSeconds(next_enemy.delay);

            PlaceEnemy(next_enemy.type, next_enemy.entry, next_enemy.target);
            
        }

        Debug.Log("routine done");
    }

    void PlaceEnemy(string type, string entry, string target)
    {
        Enemy e = Instantiate(named_enemies.GetByName(type)).GetComponent<Enemy>();
        e.transform.position = GM.level_manager.current_level.entrances.GetByName(entry).transform.position;
        e.movement_target = GM.level_manager.current_level.targets.GetByName(target);
        e.Initialize();
    }

}
