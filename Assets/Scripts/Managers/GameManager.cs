using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, ISubscribable
{
    private int score;
    private Pooler pool;
    private EnemyManager enemyManager;
    private EventManager eventManager;
    private GunManager gunManager;
    public int Wave
    {
        get;
        private set;
    }

    public Pooler Pool
    {
        get => pool;
    }

 public GunClass PlayerGun { get; private set; }

    public EventManager EventManager
    {
        get => eventManager;
    }

    public GunManager GunManager
    {
        get => gunManager;
    }

    private void Awake()
    { 
        CreateManagerInstances();
        SetUpGunManagerData();
        Time.timeScale = 0;
    }
    private void Start()
    {
        EventSubscriptions();
        CreatePool();
        PauseGame();
        enemyManager.StartNewWave();
    }


    public int Score
    {
        get { return score; }
    }


    public bool IsPaused
    {
        get { return Time.timeScale == 0f; }

    }
    public bool IsDead
    {
        get;
        private set;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // Game ManageMent
    //-----------------------------------------------------------------------------------------------------------------------
    public void ScoreUp(int score)
    {
        this.score += score;
        UIManager.Instance.UpdateScore();
    }
    public void OnDeath()
    {
        Time.timeScale = 0f;
        IsDead = true;
        pool.ReturnAll();
    }

    public void PauseGame()
    {
        if (!IsDead)
        {
            switch (IsPaused)
            {
                case false:
                    Time.timeScale = 0f;
                    UIManager.Instance.SetPauseMenu();
                    break;
                case true:
                    Time.timeScale = 1f;
                    UIManager.Instance.SetPauseMenu();
                    break;
            }
        }
    }

    public bool WaveUp( out int wave)
    {  
        if ( Wave == 0 || !enemyManager.CheckListForActiveEnemies())
        {
            if(Wave == 10)
            {
                OnDeath();
                UIManager.Instance.SetWinMenu();
               
                wave = Wave;
                return false;
            }
            Wave++;
            wave = Wave;
            eventManager.ChangeWave(Wave);
            return true;
        }
        wave = Wave;
        return false;
    }

    public void ConfirmDead()
    {
        IsDead = true;
    }

    public void OnRestart()
    {
        IsDead = false;
        Time.timeScale = 1f;
        score = 0;
        Wave = 0;
        enemyManager.StartNewWave();

    }

    public void Restart()
    {
        eventManager.BroadcastRestart();
    }
    //---------------------------------------------------------------------------------------------------------------------------------
    //GameManager Utility
    //---------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Handles all subscriptions for this class 
    /// </summary>
    private void EventSubscriptions()
    {
        eventManager.SubscribeToDeath(this);
        eventManager.SubscribeToRestart(this);
    }
    /// <summary>
    /// Creates initial GameObjects for the scene that will be needed at the start
    /// </summary>
    private void CreatePool()
    {
        pool = ScriptableObject.CreateInstance<Pooler>();
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Bullet");
        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        pool.GenerateList(PrefabList.BULLET, prefab, 10);
        pool.GenerateList(PrefabList.ENEMY, enemyPrefab, 40);
    }
    /// <summary>
    /// Creates a new instance of managers belonging to this class
    /// </summary>
    private void CreateManagerInstances()
    {
        eventManager = new EventManager();
        enemyManager = new EnemyManager();
        gunManager = new GunManager();

    }
    /// <summary>
    /// imports all GunData assets and applies them to the GunManager
    /// </summary>
    private void SetUpGunManagerData()
    {
        List<GunData> assets = new()
        {
             Resources.Load<GunData>("Data/Pistol"),
             Resources.Load<GunData>("Data/Rifle"),
             Resources.Load<GunData>("Data/Shotgun"),
             Resources.Load<GunData>("Data/SMG"),
             Resources.Load<GunData>("Data/Sniper"),
             Resources.Load<GunData>("Data/Unarmed"),
        };
        gunManager.SetUpGunStats(assets);
    }
    /// <summary>
    /// changes the player weapon. Makes the change weapon method in GunManager "visible" to be used in a button press
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(string weapon)
    {
        gunManager.ChangeToGun(weapon);
    }
    /// <summary>
    /// Upgrades the weapon. Makes the GunManager Upgrade method "Visible" to be used in a button press
    /// </summary>
    /// <param name="upgradeType"></param>
    public void Upgrade(int upgradeType)
    {
        if (upgradeType > (int)UpgradeType.MAGSIZE) return;

        gunManager.Upgrades((UpgradeType)upgradeType);
    }
    /// <summary>
    /// Exits to the main menu
    /// </summary>
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void OnDestroy()
    {
       eventManager.Clear();
    }
    /// <summary>
    /// Applies the players gun to the PlayerGun Reference
    /// </summary>
    /// <param name="gun"></param>
    public void WeaponReference(GunClass gun)
    {
        if (gun == null) return;

        PlayerGun = gun;
    }




}
