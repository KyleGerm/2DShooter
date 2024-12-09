using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "MyData/GunData")]
public class GunData : ScriptableObject, IUpgradable
{
    [SerializeField] private string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField] private int damage;
    [SerializeField] private int maxDamageLevel;
    [SerializeField] private float damageIncreaseRate;
    public int Damage { get => damage; set => damage = value; }
    public int DamageLevel { get; set; }
    public int MaxDamageLevel { get => maxDamageLevel; private set => maxDamageLevel = value; }
    public float DamageIncreaseRate { get => damageIncreaseRate; private set => damageIncreaseRate = value ; }

    [SerializeField] private float fireRate;
    [SerializeField] private int maxFireRateLevel;
    [SerializeField] private float fireRateIncreaseRate;
    public float FireRate { get => fireRate; set => fireRate = value; }
    public int FireRateLevel {  get; set; }
    public int MaxFireRateLevel { get => maxFireRateLevel; private set => maxFireRateLevel = value; }
    public float FireRateIncreaseRate { get => fireRateIncreaseRate; private set => fireRateIncreaseRate = value; }

    [SerializeField] private float reloadTime; 
    [SerializeField] private int maxReloadLevel;
    [SerializeField] private float reloadIncreaseRate;
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public int ReloadLevel { get; set; }
    public int MaxReloadLevel { get => maxReloadLevel; private set => maxReloadLevel = value; }
    public float ReloadIncreaseRate { get => reloadIncreaseRate; private set => reloadIncreaseRate = value; }

    [SerializeField] private int magSize;
    [SerializeField] private int maxMagLevel;
    [SerializeField] private int magIncreaseRate;
    public int MagSize { get => magSize; set => magSize = value; }
    public int MagLevel { get; set; }
    public int MaxMagLevel { get => maxMagLevel; private set => maxMagLevel = value; }
    public int MagIncreaseRate { get => magIncreaseRate; private set => magIncreaseRate = value; }

    [SerializeField] private float range;
    [SerializeField] private int maxRangeLevel;
    [SerializeField] private float rangeIncreaseRate;
    public float Range { get => range; set => range = value; }
    public int RangeLevel { get; set; }
    public int MaxRangeLevel { get => maxRangeLevel; private set => maxRangeLevel = value; }
    public float RangeIncreaseRate { get => rangeIncreaseRate; private set => rangeIncreaseRate = value; }

    [SerializeField] private float accuracy;
    [SerializeField] private int maxAccuracyLevel;
    [SerializeField] private float accuracyIncreaseRate;
     public float Accuracy { get => accuracy; set => accuracy = value; }
    public int AccuracyLevel { get; set; }
    public int MaxAccuracyLevel { get => maxAccuracyLevel; private set => maxAccuracyLevel = value; }
    public float AccuracyIncreaseRate { get => accuracyIncreaseRate; private set => accuracyIncreaseRate = value; }
   public int Presteige { get; set; }  
    public int CurrentClip {  get; set; }

    [SerializeField] private int projectiles;
    public int Projectiles{get => projectiles; private set => projectiles = value; }
    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed {  get => projectileSpeed; private set => projectileSpeed = value; }

    [SerializeField] private int damageCost;
    [SerializeField] private int reloadSpeedCost;
    [SerializeField] private int fireRateCost;
    [SerializeField] private int accuracyCost;
    [SerializeField] private int magSizeCost;
    public int DamageCost { get => damageCost; set => damageCost = value; }
    public int ReloadSpeedCost { get => reloadSpeedCost; set => reloadSpeedCost = value; }
    public int FireRateCost { get => fireRateCost; set => fireRateCost = value; }
    public int AccuracyCost { get => accuracyCost;set => accuracyCost = value; }
    public int MagSizeCost { get => magSizeCost; set => magSizeCost = value; }

    /// <summary>
    /// Copies the stats of the method caller to this version
    /// </summary>
    /// <param name="current"></param>
    public void CopyCurrentStatsToTemplate(IUpgradable current)
    {
      Description = current.Description;
        Damage = current.Damage;
        DamageLevel = current.DamageLevel;
        MaxDamageLevel = current.MaxDamageLevel;
        DamageIncreaseRate = current.DamageIncreaseRate;
        FireRate = current.FireRate;
        FireRateLevel = current.FireRateLevel;
        MaxFireRateLevel = current.MaxFireRateLevel;
        FireRateIncreaseRate = current.FireRateIncreaseRate;
        Accuracy = current.Accuracy;
        AccuracyLevel = current.AccuracyLevel;
        MaxAccuracyLevel = current.MaxAccuracyLevel;
        AccuracyIncreaseRate = current.AccuracyIncreaseRate;
        Presteige = current.Presteige;
        CurrentClip =  current.MagSize;
        ReloadTime = current.ReloadTime;
        ReloadLevel = current.ReloadLevel;
        MaxReloadLevel = current.MaxReloadLevel;
        ReloadIncreaseRate = current.ReloadIncreaseRate;
        Range = current.Range;
        RangeLevel = current.RangeLevel;
        MaxRangeLevel = current.MaxRangeLevel;
        RangeIncreaseRate = current.RangeIncreaseRate;
        MagSize = current.MagSize;
        MagLevel = current.MagLevel;
        MaxMagLevel = current.MaxMagLevel;
        MagIncreaseRate = current.MagIncreaseRate;
        Projectiles = current.Projectiles;
        ProjectileSpeed = current.ProjectileSpeed;
        DamageCost = current.DamageCost;
        ReloadSpeedCost = current.ReloadSpeedCost;
        FireRateCost = current.FireRateCost;
        AccuracyCost = current.AccuracyCost;
        MagSizeCost = current.MagSizeCost;
    }
}