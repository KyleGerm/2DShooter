using UnityEngine;

public class EnemyController : MonoBehaviour , IPoolable , IHaveHealth
{
   
    private Rigidbody2D rb;
    private Transform player;
    public EnemyHealth health;
    private EnemyWeapons weapon;
    private EnemyMovement movement;
    private float distance;
    public float Distance
    {
        get => distance;
    }

    public int Health
    {
        get
        {
            return health.CurrentHealth;
        }
    }
    public Weapon Weapon
    {
        get => weapon;
    }
    public Transform Player
    {
        get => player;
    }

    private void Awake()
    {
        health= GetComponent<EnemyHealth>(); 
        weapon = GetComponent<EnemyWeapons>(); 
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        movement = new(this);
    }

    void Update()
    {
        distance = Vector2.Distance(rb.position, player.position);
    }
    private void FixedUpdate()
    {
        movement.Move();

        if(weapon.IsReloaded && weapon.CurrentClip == 0)
        {
            weapon.BeginReload();
        }
      else if (player)
        {
            Aim();
            if (weapon.IsReloaded && distance < weapon.Range)
            {
                Fire();
            }
        }
    }

    /// <summary>
    /// Returns enemy to the pool it came from, and sends an event
    /// </summary>
    public void Dead()
    {
        ReturnToPool();
       GameManager.Instance.EventManager.BroadCastEnemyDeath();
    }

    /// <summary>
    /// Points the enemy to the players current position
    /// </summary>
    private void Aim()
    {
        Vector2 lookingAt = (Vector2)player.position - rb.position;
        float angle = Mathf.Atan2(lookingAt.y, lookingAt.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    /// <summary>
    /// Attempts to fire the enemys weapon
    /// </summary>
    void Fire()
    {
            try
            {
               weapon.Shoot();
            }
            catch (System.Exception)
            {
            return;
            }
        
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Recalibrates the enemys weapon and enemy stats to a new value and sets them to max
    /// </summary>
    /// <param name="difficulty"></param>
    public void ReInitialize(float difficulty)
    {
       weapon.InitializeWeaponType((WeaponSelect)Random.Range(1, 6), difficulty);
        health.UpgradeHealth(difficulty); 
        health.OnRestart();
    }

}
