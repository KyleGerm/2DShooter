using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour ,ISubscribable
{
    private int health;
    private IHaveHealth healthHolder;
    private Coroutine healing;
    private float healingInterval = 1f;
    private float healingDelay = 2f;
    private int healthAmount = 5;
    private float delay;
    protected int maxHealth = 100;
    private PlayerController player;
   
    public int CurrentHealth
    {
        get => health;
    }

    private void Awake()
    {
        health = 100;
    }
    void Start()
    {
         GameManager.Instance.EventManager.SubscribeToRestart(this);
       healthHolder = gameObject.GetComponent<IHaveHealth>();
        gameObject.TryGetComponent(out player);
        ResetHealth();
    }

    /// <summary>
    /// Applies damage to the gameobject
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        CheckHealth();
        delay = healingDelay;

        if (healing != null || health <= 0 || !gameObject.activeInHierarchy) return;
        
        healing = StartCoroutine("PassiveHeal");
    }
    /// <summary>
    /// checks the current health and performs the relevant action
    /// </summary>
    private void CheckHealth()
    {

        CheckForPlayer();
        if (health <= 0)
        {
            health = 0;
            healthHolder.Dead();
            if (healing != null) 
            {
                StopCoroutine(healing);
            }

        }
        else if (health >= maxHealth)
        {
            health = 100;
            if(healing != null)
            {
                 StopCoroutine(healing);
                 healing = null;
            }
          
        }
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Resets health to default values
    /// </summary>
    public void OnRestart()
    {
        ResetHealth();
        CheckForPlayer();
    }
   /// <summary>
   /// health is set to max
   /// </summary>
    private void ResetHealth()
    {
        health = maxHealth;
    }
    /// <summary>
    /// Begins healing after a set period of time
    /// </summary>
    /// <returns></returns>
    private IEnumerator PassiveHeal()
    { 
        while (true)
        {
            while (delay > 0)
            {
             delay -= Time.deltaTime;
             yield return null;
            }
       
            health += healthAmount;
            yield return new WaitForSeconds(healingInterval);
            CheckHealth();
        }
    }
    /// <summary>
    /// checks for player and if it exists, updates the health bar
    /// </summary>
    private void CheckForPlayer()
    {
        if (player != null)
        {
            GameManager.Instance.EventManager.ChangeHealthBar(CurrentHealth, maxHealth);
        }
    }
 
}
