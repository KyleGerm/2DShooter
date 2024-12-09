using UnityEngine;


public static class WeaponUpgrades<T> where T : IUpgradable
{
    /// <summary>
    /// Takes in an Upgrade enum, and a Type of Weapon and returns the current Level, Max level, and Cost of that Upgrade Type
    /// </summary>
    /// <param name="type">Type of Upgrade to search for</param>
    /// <param name="gun">The Weapon having the upgrade applied</param>
    /// <param name="Level">Current level of the upgrade type</param>
    /// <param name="MaxLevel">Max Level of the upgrade type</param>
    /// <param name="Cost">Current cost of the upgrade type</param>
    private static void ValuesToCheck(UpgradeType type, T gun,out int Level, out int MaxLevel, out int Cost) 
    {
        switch(type)
        {
            case UpgradeType.DAMAGE:
                Level = gun.DamageLevel;
                MaxLevel = gun.MaxDamageLevel;
                Cost = gun.DamageCost;
                break;
            case UpgradeType.RELOAD:
                Level = gun.ReloadLevel;
                MaxLevel = gun.MaxReloadLevel;
                Cost = gun.ReloadSpeedCost;
                break;
            case UpgradeType.MAGSIZE:
                Level = gun.MagLevel;
                MaxLevel = gun.MaxMagLevel;
                Cost = gun.MagSizeCost;
                break;
            case UpgradeType.ACCURACY:
                Level = gun.AccuracyLevel;
                MaxLevel = gun.MaxAccuracyLevel;
                Cost = gun.AccuracyCost;
                break;
            case UpgradeType.FIRERATE:
                Level = gun.FireRateLevel;
                MaxLevel = gun.MaxFireRateLevel;
                Cost = gun.FireRateCost;
                break;
            default:
               Level = 0; MaxLevel = 0;  Cost = 0;
                break;
        }
    }

    /// <summary>
    /// Takes the weapon, and upgrade type, and a wallet, and applies the relevant upgrade to it.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="type"></param>
    /// <param name="wallet"></param>
    public static void UpgradeWeapon(T gun, UpgradeType type, ref int wallet)
    {
        ValuesToCheck(type, gun, out int Level, out int MaxLevel, out int Cost);
        
        if(Cost > wallet)
        {
            //Cannot afford Upgrade
            wallet = 0;
            return;
        }
        else if(Level < MaxLevel)
        {
            wallet = Cost;
           ApplyNewValues(gun, type);
            StatsPanel.Instance.UpdateValue(type);
        }
        else
        {
            //Upgrade Level is at Max
            wallet = 0;
        }
    }
    /// <summary>
    /// Takes in a weapon type and upgrade type, and applies the relevant upgrades
    /// </summary>
    /// <param name="gun">Weapon to be upgraded</param>
    /// <param name="type">Type of upgrade to be applied</param>
    private static void ApplyNewValues(T gun, UpgradeType type )
    {
        int newCost;
        float value;
        switch(type)
        {
            case UpgradeType.DAMAGE:
                newCost = gun.DamageCost;
                CalculateNewValues(gun, type, out value, ref newCost);
                gun.DamageCost = newCost;
                gun.Damage = (int)value;
                gun.DamageLevel++; 
                break;
            case UpgradeType.RELOAD:
                newCost = gun.ReloadSpeedCost;
                CalculateNewValues(gun, type, out value, ref newCost);
                gun.ReloadSpeedCost = newCost;
                gun.ReloadTime = value;
                gun.ReloadLevel++;
                break;
            case UpgradeType.MAGSIZE:
                newCost = gun.MagSizeCost;
                CalculateNewValues(gun, type, out value, ref newCost);
                gun.MagSizeCost = newCost;
                gun.MagSize = (int)value;
                gun.MagLevel++;
                break;
            case UpgradeType.ACCURACY:
                newCost = gun.AccuracyCost;
                CalculateNewValues(gun, type,out value, ref newCost);
                gun.AccuracyCost = newCost;
                gun.Accuracy = value;
                gun.AccuracyLevel++;
                break;
            case UpgradeType.FIRERATE:
                newCost = gun.FireRateCost;
                CalculateNewValues(gun, type, out value, ref newCost);
                gun.FireRateCost = newCost;
                gun.FireRate = value;
                gun.FireRateLevel++;
                break;

            default: return;
        }
    }

    /// <summary>
    /// Generates values for the next upgrade, before applying them
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="upgrade"></param>
    /// <param name="value"></param>
    /// <param name="cost"></param>
    private static void CalculateNewValues( T gun,UpgradeType upgrade, out float value, ref int cost)
    {
        switch(upgrade)
        {
            case UpgradeType.DAMAGE:
                DamageValues(gun, out float damage, out float difference);
                damage += Mathf.Ceil(difference);
                value = (int)damage;
                break;
            case UpgradeType.FIRERATE:
                value = gun.FireRate;
                value += FireRateValue(gun);
                break;
            case UpgradeType.RELOAD:
                value = gun.ReloadTime;
                value -= ReloadSpeedValue(gun);
                break;
            case UpgradeType.MAGSIZE:
                value = gun.MagSize;
                value += gun.MagIncreaseRate;
                break;
            case UpgradeType.ACCURACY:
                value = gun.Accuracy;
                value -= AccuracyIncreaseValue(gun);
                break;
            default: value = 0; cost = 0; break;    
        }
        cost = CostIncrease(cost);
    }
    //---------------------------------------------------------------------------------------------------------
    //Below are Tools to help classes return current values of weapon stats
    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Returns the current accuracy and the difference of the next upgrade in a string.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    public static void AccuracyPercentageValuesToString(T gun,out string current, out string difference)
    {
        float currentGunAccuracy = AccuracyToPercent(gun.Accuracy);
        float upgradedAccuracy = gun.Accuracy - AccuracyIncreaseValue(gun);
        float upgradedAccuracyPercentage = AccuracyToPercent(upgradedAccuracy);
        float upgradeDifference = upgradedAccuracyPercentage - currentGunAccuracy;

        string upgradeDifferenceString = upgradeDifference.ToString("n2");
        string currentGunAccuracyString = currentGunAccuracy.ToString("n2");

        current = currentGunAccuracyString;
        difference = upgradeDifferenceString;
    }

    /// <summary>
    /// returns the current damage value and the upgrade value of the next to string.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    public static void DamageValuesToString(T gun, out string current, out string difference)
    {
        float currentDamage;
        float damageDifference;
        DamageValues(gun, out currentDamage, out damageDifference);
        current = ((int)currentDamage).ToString();
        difference = ((int)Mathf.Ceil(damageDifference)).ToString();
    }

    /// <summary>
    /// Returns the current fire rate and the difference of the next upgrade in a string.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    public static void FireRateValuesToString(T gun, out string current, out string difference)
    {
        current = gun.FireRate.ToString("n3");
        difference = FireRateValue(gun).ToString("n3");
    }

    /// <summary>
    /// Returns the current reload speed and the difference of the next upgrade in a string.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    public static void ReloadSpeedValuesToString(T gun, out string current, out string difference)
    {
        current = gun.ReloadTime.ToString("n2");
        difference = ReloadSpeedValue(gun).ToString("n2");
    }

    /// <summary>
    /// Returns the current Magazing size and the difference of the next upgrade in a string.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    public static void MagSizeValuesToString(T gun, out string current, out string difference)
    {
        current = gun.MagSize.ToString();
        difference = gun.MagIncreaseRate.ToString();
    }

    /// <summary>
    /// Returns the current and maximum level of an upgradable category.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="upgradeType"></param>
    /// <param name="currentLevel"></param>
    /// <param name="MaxLevel"></param>
    public static void GetLevelValues(T gun, UpgradeType upgradeType, out int currentLevel, out int MaxLevel)
    {
        switch (upgradeType)
        {
            case UpgradeType.RELOAD:
                currentLevel = gun.ReloadLevel;
                MaxLevel = gun.MaxReloadLevel;
                break;
            case UpgradeType.DAMAGE:
                currentLevel = gun.DamageLevel;
                MaxLevel = gun.MaxDamageLevel;
                break;
            case UpgradeType.MAGSIZE: 
                currentLevel = gun.MagLevel;
                MaxLevel = gun.MaxMagLevel;
                break;
            case UpgradeType.ACCURACY:
                currentLevel = gun.AccuracyLevel;
                MaxLevel = gun.MaxAccuracyLevel;
                break;
            case UpgradeType.FIRERATE:
                currentLevel = gun.FireRateLevel;
                MaxLevel = gun.MaxFireRateLevel;
                break;

            default: 
                currentLevel = 0;
                MaxLevel = 0;
                break;
        }
    }
    //---------------------------------------------------------------------------------------------------------
    //These are calculation methods to eliminate repetition throughout the script
    //---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// The current damage and the difference of the next.
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="current"></param>
    /// <param name="difference"></param>
    private static void DamageValues(T gun, out float current, out float difference)
    {
        // These must initially return as float for rounding 
        current = gun.Damage;
        difference = (float)gun.Damage / 10 * gun.DamageIncreaseRate ;
    }
    /// <summary>
    /// The caluclation for the next accuracy upgrade
    /// </summary>
    /// <param name="gun"></param>
    /// <returns></returns>
    private static float AccuracyIncreaseValue(T gun)
    {
        return gun.Accuracy / 20 * gun.AccuracyIncreaseRate;
    }

    /// <summary>
    /// The value of accuracy as a percentage.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static float AccuracyToPercent(float value)
    {
        return (360 - (value * 2)) / 360 * 100;
    }

    /// <summary>
    /// The Calculation for the next FireRate upgrade.
    /// </summary>
    /// <param name="gun"></param>
    /// <returns></returns>
    private static float FireRateValue(T gun)
    {
        return gun.FireRate / 50 * gun.FireRateIncreaseRate;
    }

    /// <summary>
    /// The Calculation for the next ReloadSpeed upgrade.
    /// </summary>
    /// <param name="gun"></param>
    /// <returns></returns>
    private static float ReloadSpeedValue(T gun)
    {
        return gun.ReloadTime / 25 * gun.ReloadIncreaseRate;
    }

    /// <summary>
    /// The cost increase rate for all items with a currency value.
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private static int CostIncrease(int current)
    {
         return Mathf.CeilToInt(current * Mathf.Log(current , current/2f));
    }
}
