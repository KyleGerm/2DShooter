/// <summary>
/// Base stats of any weapon
/// </summary>
    public interface IGunStatistics
    {
        string Description { get; }
        int CurrentClip { get; }
        int Damage { get; set; }
        float FireRate { get; set; }
        float ReloadTime { get; set; }
        int MagSize { get; set; }
        float Range { get; set; }
        float Accuracy { get; set; }
        int Projectiles { get; }
        float ProjectileSpeed { get; }
    }
/// <summary>
/// Stats of any weapon which can be upgraded
/// </summary>
    public interface IUpgradable : IGunStatistics
    {
        int DamageCost { get; set; }
        int ReloadSpeedCost { get; set; }
        int FireRateCost { get; set; }
        int AccuracyCost { get; set; }
        int MagSizeCost { get; set; }
        int MaxFireRateLevel { get; }
        int FireRateLevel { get; set; }
        int Presteige { get; set; }
        float DamageIncreaseRate { get; }
        float FireRateIncreaseRate { get; }
        int MagIncreaseRate { get; }
        int ReloadLevel { get; set; }
        int MaxReloadLevel { get; }
        float ReloadIncreaseRate { get; }
        float RangeIncreaseRate { get; }
        float AccuracyIncreaseRate { get; }
        int MaxAccuracyLevel { get; }
        int AccuracyLevel { get; set; }
        int MaxMagLevel { get; }
        int MagLevel { get; set; }
        int MaxDamageLevel { get; }
        int DamageLevel { get; set; }
        int MaxRangeLevel { get; }
        int RangeLevel { get; set; }
    }
/// <summary>
/// Anything which needs to be notified of any death or restart in the game
/// </summary>
    public interface ISubscribable
    {
        void OnDeath();
        void OnRestart();

    }

  /*  public interface IEnemyListener
    { 
      void OnFire(float range, int damage);
    }*/
  

/// <summary>
/// Qualifies the class to be part of a pool 
/// </summary>
    public interface IPoolable
    {
    /// <summary>
    /// Returns the item to the pool it belongs to.
    /// </summary>
        void ReturnToPool();
    }
/// <summary>
/// List of all game prefabs
/// </summary>
    public enum PrefabList
    {
        BULLET,
        ENEMY
    }
/// <summary>
/// List of all weapon types in the game
/// </summary>
    public enum WeaponSelect
    {
        UNARMED,
        PISTOL,
        SMG,
        RIFLE,
        SNIPER,
        SHOTGUN,
        NONE
    }
/// <summary>
/// List of all types of weapon upgrade
/// </summary>
    public enum UpgradeType
    {
        DAMAGE,
        FIRERATE,
        RELOAD,
        ACCURACY,
        MAGSIZE,
    }
/// <summary>
/// Identifier for anything interested in enemy deaths
/// </summary>
    public interface IManagerSubscriptions
    {
        void OnEnemyDeath();
    }
/// <summary>
/// Should be implemented by anything which holds a Health component
/// </summary>
    public interface IHaveHealth
    {
        void Dead();
    }
/// <summary>
/// Wallet components implement this
/// </summary>
    public interface IWallet
    {
        void AddCurrency(int amount);
        void RemoveCurrency(int amount);
    }
/// <summary>
/// HUDController identifier for decoupling
/// </summary>
    public interface IHUDController
    {
        void FormatHealthBar(float current, float max);
        void UpdateAmmoCount(int current, int max);
    void UpdateWave(int wave);
    }
