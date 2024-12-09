using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    /// <summary>
    /// Public Method for upgrading the enemys health only. This will not work if trying to apply it to the playercontroller.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="difficulty"></param>
    public void UpgradeHealth(float difficulty)
    {
            maxHealth = Mathf.CeilToInt(100 * difficulty);
    }
}
