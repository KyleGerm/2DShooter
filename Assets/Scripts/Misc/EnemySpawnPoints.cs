using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoints : Singleton<EnemySpawnPoints>
{
   [SerializeField] private List<Transform> spawnPoints;
   [SerializeField] private Transform playerPos;
    /// <summary>
    /// Returns a new spawn point from a lits of transforms
    /// </summary>
    /// <returns></returns>
    public Transform GetSpawnPoint()
    {
        List<Transform> list = spawnPoints;
        Transform spawn;
        while(true)
        {
            int num = Random.Range(0, list.Count);
            spawn = list[num];
            if(spawn != null && Vector2.Distance(spawn.position, playerPos.position) > 10f)
            {
                break;
            }

            list.RemoveAt(num);
        }
        return spawn;
    }
}
