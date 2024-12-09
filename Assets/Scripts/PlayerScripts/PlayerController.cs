using UnityEngine;

public class PlayerController : MonoBehaviour, ISubscribable, IHaveHealth
{
   Transform Spawn;
   public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 mousePos;
    public Camera cam;
    private bool strafeStart;
    private Vector2 strafePos;
    private Vector2 staticWalk;
    private Vector2 movement;
    private GunClass gun;
    private static Wallet wallet = new();

   private bool isShooting;
    private bool fixedWalk;
    private bool isReloading;
   
    private bool IsDead
    {
        get { return !gameObject.activeInHierarchy; }
    }
    
    public static int Money
    {
        get { return wallet.Money; }
    }
    private void Awake()
    {
        Spawn = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponent<GunClass>();
        gun.ChangeWeapon("Unarmed");
        GameManager.Instance.WeaponReference(gun);
        GameManager.Instance.EventManager.SubscribeToRestart(this);
       LinkWallet();
        gameObject.transform.position = Spawn.position;
    }

    /// <summary>
    /// Receives input for the player
    /// </summary>
    void Update()
    {
       movement.x = Input.GetAxis("Horizontal");
       movement.y = Input.GetAxis("Vertical");
       mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
       isShooting = Input.GetMouseButton(0);
       isReloading = Input.GetMouseButtonDown(1);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ExitToMenu();
        }
    }
    /// <summary>
    /// PlayerController loop
    /// </summary>
    private void FixedUpdate()
    {
        if(IsDead) return;

        if (!UIManager.Instance.OverButton && !GameManager.Instance.IsPaused)
        {
            EvaluateInputs();
        }
        Aim();
        Movement();
    }
    /// <summary>
    /// Applies movement to the gameObject
    /// </summary>
    private void Movement()
    {
        Vector2 currentPos = new Vector2(rb.position.x, rb.position.y);
        Vector2 interpolatedMovement = Vector2.zero;
        //Makes player move toward mousePos when 'W' is pressed
        if (movement.y != 0.0f)
        {
            Vector2 ForwardMovement = new Vector2(rb.transform.up.x, rb.transform.up.y);
            if (isShooting)
            {
                if (!fixedWalk)
                {
                    staticWalk = ForwardMovement;
                    fixedWalk = true;
                }
              interpolatedMovement += movement.y * moveSpeed * Time.fixedDeltaTime * staticWalk;
            }
            else
            { 
                interpolatedMovement += movement.y * moveSpeed * Time.fixedDeltaTime * ForwardMovement;
                fixedWalk = false;
            }
        }
       
        if (movement.x != 0.0f)
        {
            Vector2 sideMove = new Vector2(rb.transform.right.x, rb.transform.right.y);
            if (Input.GetKey(KeyCode.LeftShift))
            {  
                if (!strafeStart)
                {
                    strafePos = sideMove;
                    strafeStart = true;
                }
                interpolatedMovement += movement.x * moveSpeed * Time.fixedDeltaTime * strafePos;
            }
            else
            {
                strafeStart = false;
                interpolatedMovement += movement.x * moveSpeed * Time.fixedDeltaTime * sideMove;
            }
        }
        rb.MovePosition( currentPos + interpolatedMovement);
    }
    /// <summary>
    /// Makes the player look at the mouse cursor 
    /// </summary>
    private void Aim()
    {
        Vector2 lookingAt = mousePos - rb.position;
        float angle = Mathf.Atan2(lookingAt.y, lookingAt.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    /// <summary>
    /// If the player dies, inform all subscribed objects
    /// </summary>
    public void Dead()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EventManager.BroadcastDeathEvent();
    }
    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Resets the player
    /// </summary>
    public void OnRestart()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = Spawn.position;
    }
    /// <summary>
    /// Checks what inputs are happenning and applies them
    /// </summary>
    private void EvaluateInputs()
    {
        if (isShooting)
        {
            gun.Shoot();
        }
        else if (isReloading)
        {
             gun.BeginReload();
        }
    }

    /// <summary>
    /// subscribes the wallet within this class to the event manager
    /// </summary>
    private void LinkWallet()
    {
        GameManager.Instance.EventManager.SubscribeMyWallet(wallet);
    }
}
