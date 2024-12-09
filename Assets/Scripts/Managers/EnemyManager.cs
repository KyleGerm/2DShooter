using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : IManagerSubscriptions
{
    private EnemyController[] enemies;

    //Management Methods for organising enemies, verifying status, reinitialising enemies, and changing the enemy list.
   public EnemyManager()
    {
        GameManager.Instance.EventManager.SubscribeToEnemyDeath(this);
    }
    /// <summary>
    /// Populates the enemy list with the array given
    /// </summary>
    /// <param name="newEnemies"></param>
    /// <param name="difficulty"></param>
    public void StoreNewEnemies(EnemyController[] newEnemies, float difficulty)
    {
        enemies = newEnemies;
        ResetEnemies(difficulty);
    }
    /// <summary>
    /// Goes through the list and calls the reinitialize method on each
    /// </summary>
    /// <param name="difficulty"></param>
    private void ResetEnemies(float difficulty)
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ReInitialize(difficulty);
        }
    }
    /// <summary>
    /// Activates the given number of enemies
    /// </summary>
    /// <param name="num"></param>
    public void WakeEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (TryGetNewEnemy(out GameObject enemy))
            {
                enemy.transform.position = EnemySpawnPoints.Instance.GetSpawnPoint().position;
                enemy.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Returns all enemies to the pool
    /// </summary>
    public void SleepEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ReturnToPool();
        }
    }
    /// <summary>
    /// returns a new gameobject from the list which qualifies to be used. Return type is a bool to clarify success
    /// </summary>
    /// <param name="newEnemy"></param>
    /// <returns></returns>
    public bool TryGetNewEnemy(out GameObject newEnemy)
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null && !enemy.gameObject.activeInHierarchy && enemy.Health > 0)
            {
                newEnemy = enemy.gameObject;
                return true;
            }
        }
        newEnemy = null;
        return false;
    }
    /// <summary>
    /// Checks the list for active enemies and returns true if any are.
    /// </summary>
    /// <returns></returns>
    public bool CheckListForActiveEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Adds currency and score, and sees if there is a new enemy in the list to set up. If not, starts a new wave.
    /// </summary>
       public void OnEnemyDeath()
    {
        int wave = GameManager.Instance.Wave;
        GameManager.Instance.ScoreUp(5 * wave);
        GameManager.Instance.EventManager.AddToWallet(Mathf.CeilToInt(10 * wave * EnemyDifficulty(wave)));

            if (TryGetNewEnemy(out GameObject newEnemy))
            {
                newEnemy.transform.position = EnemySpawnPoints.Instance.GetSpawnPoint().position;
                newEnemy.SetActive(true);
            }
             else
             {
                 StartNewWave();
             }
        
       
    }
    /// <summary>
    /// verifies a new wave should start, and sets it up.
    /// </summary>
    public void StartNewWave()
    {
        if(GameManager.Instance.WaveUp(out int newWave)) 
        { 
            StoreNewEnemies(GameManager.Instance.Pool.RequestMultiple<EnemyController>(EnemiesToSpawn(newWave), PrefabList.ENEMY), EnemyDifficulty(newWave));
            WakeEnemies(5);
        }
    }
    /// <summary>
    /// The number of enemies that will exist in each wave.
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    private int EnemiesToSpawn(int wave)
    {
        int value = 15 + (wave * 5);
        return value < 40 ? value : 40;
    }

    /// <summary>
    /// The difficulty rating of enemies based on wave.
    /// </summary>
    /// <param name="wave"></param>
    /// <returns></returns>
    private float EnemyDifficulty(int wave)
    {
        switch(wave)
        {
            case <= 6:
                return 1f;
            case >= 7:
                return 1f + ((wave % 6) * .25f);
        }
    }


}
