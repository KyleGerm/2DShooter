using System;
using UnityEngine;

public class GunClass : Weapon, IUpgradable, ISubscribable
{
    public float AccuracyIncreaseRate { get; protected set; }
    public float FireRateIncreaseRate { get; protected set; }
    public int DamageLevel { get; set; }
    public int MaxDamageLevel { get; protected set; }
    public float DamageIncreaseRate { get; protected set; }
    public int MaxRangeLevel { get; protected set; }
    public int RangeLevel { get; set; }
    public int MaxAccuracyLevel { get; protected set; }
    public int AccuracyLevel { get; set; }
    public int MaxMagLevel { get; protected set; }
    public int MagLevel { get; set; }
    public int MaxFireRateLevel { get; protected set; }
    public int FireRateLevel { get; set; }
    public int Presteige { get; set; }
    public int MagIncreaseRate { get; protected set; }
    public int ReloadLevel { get; set; }
    public int MaxReloadLevel { get; protected set; }
    public float ReloadIncreaseRate { get; protected set; }
    public float RangeIncreaseRate {  get; protected set; }
    public int DamageCost { get; set; }
    public int ReloadSpeedCost { get; set; }
    public int FireRateCost { get; set; }
    public int AccuracyCost { get; set; }
    public int MagSizeCost { get; set; }

    private void Start()
    {
        TryGetComponent(out player);
        GameManager.Instance.EventManager.SubscribeToRestart(this);
       CheckForPlayer();
    }
    /// <summary>
    /// Changes the weapon to a new one from a list
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(string weapon)
    {
        if(Description == weapon)       //If the weapon currently equipped matches the one to change to, return.
        {
            return; 
        }

        GunData newWeapon = GameManager.Instance.GunManager.GetWeapon(weapon);

        if (newWeapon != null)
        {
            GunData oldWeapon = ScriptableObject.CreateInstance<GunData>();

            oldWeapon.CopyCurrentStatsToTemplate(this);

            CopyNewStatsToCurrent(newWeapon);
            GameManager.Instance.GunManager.StoreWeapon(oldWeapon);
            cooldown = 0f;
        }
       
    }
    private void OnEnable()
    {
        CheckForPlayer();
    }
   
    /// <summary>
    /// Applies the new stats from the retrieved GunData to the GunClass stats.
    /// </summary>
    /// <param name="newStats"></param>
    public void CopyNewStatsToCurrent(IUpgradable newStats)
    {
        Description = newStats.Description;
        Damage = newStats.Damage;
        DamageLevel = newStats.DamageLevel;
        MaxDamageLevel = newStats.MaxDamageLevel;
        DamageIncreaseRate = newStats.DamageIncreaseRate;
        FireRate = newStats.FireRate;
        FireRateLevel = newStats.FireRateLevel;
        MaxFireRateLevel = newStats.MaxFireRateLevel;
        FireRateIncreaseRate = newStats.FireRateIncreaseRate;
        Accuracy = newStats.Accuracy;
        AccuracyLevel = newStats.AccuracyLevel;
        MaxAccuracyLevel = newStats.MaxAccuracyLevel;
        AccuracyIncreaseRate = newStats.AccuracyIncreaseRate;
        Presteige = newStats.Presteige;
        CurrentClip =  newStats.MagSize;
        ReloadTime = newStats.ReloadTime;
        ReloadLevel = newStats.ReloadLevel;
        MaxReloadLevel = newStats.MaxReloadLevel; 
        ReloadIncreaseRate = newStats.ReloadIncreaseRate;
        Range = newStats.Range;
        RangeLevel = newStats.RangeLevel;
        MaxRangeLevel = newStats.MaxRangeLevel;
        RangeIncreaseRate = newStats.RangeIncreaseRate;
        MagSize = newStats.MagSize;
        MagLevel = newStats.MagLevel;
        MaxMagLevel = newStats.MaxMagLevel;
        MagIncreaseRate = newStats.MagIncreaseRate;
        Presteige =newStats.Presteige;
        Projectiles = newStats.Projectiles;
        ProjectileSpeed = newStats.ProjectileSpeed;
        DamageCost = newStats.DamageCost;
        ReloadSpeedCost = newStats.ReloadSpeedCost;
        FireRateCost = newStats.FireRateCost;
        AccuracyCost = newStats.AccuracyCost;
        MagSizeCost = newStats.MagSizeCost;

    }


    public void OnDeath()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets the gun to an unused state.
    /// </summary>
    public void OnRestart()
    {
       cooldown = 0;
        clip = magSize;
        isReloaded = true;
    }

    /// <summary>
    /// Returns the cost of a specified type
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetCosting(string name)
    {
        return name switch
        {
            "DamageCost" => DamageCost,
            "FireRateCost" => FireRateCost,
            "AccuracyCost" => AccuracyCost,
            "ReloadSpeedCost" => ReloadSpeedCost,
            "MagSizeCost" => MagSizeCost,
            _ => 0,
        };
    }
}
