using System.Collections.Generic;
using UnityEngine;


public class GunManager 
{
   
    //Used to store all current versions of the players weapons.
   [SerializeField] private static Dictionary<string, GunData> weaponList = new Dictionary<string, GunData>();

    //Template Used to store all base versions of weapons. Can be used to create a new version of a weapon in weaponList, if one does not already exist
    [SerializeField] private List<GunData> GunStats = new();

    private int[] PurchasePrice = {0,100,225,350,400,375};

    /// <summary>
    /// Public access for cost of weapons
    /// </summary>
    public int[] WeaponPrice { get => PurchasePrice;  }

    /// <summary>
    /// Returns a weapon based on the name given to it.
    /// </summary>
    /// <param name="WeaponName"></param>
    /// <returns></returns>
    public GunData GetWeapon(string WeaponName)
    {
        if (weaponList.ContainsKey(WeaponName))
        { 
              return weaponList[WeaponName];
        }
        else if(WeaponToBuy(WeaponName) != (int)WeaponSelect.NONE)
        {   
            if (PlayerController.Money >= PurchasePrice[WeaponToBuy(WeaponName)])
            {
                foreach (GunData weapon in GunStats)
                {
                    if (weapon.Description == WeaponName) 
                    { 
                        GameManager.Instance.EventManager.RemoveFromWallet(PurchasePrice[WeaponToBuy(WeaponName)]);
                        CostSheet.Instance.UpdateAllWeaponsCost();
                        return weapon;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Stores the current version of the weapon back in the weapons list. If the same type exits, it is replaced.
    /// </summary>
    /// <param name="stored"></param>
    public void StoreWeapon(GunData stored)
    {
        bool success = false;
        if (weaponList.ContainsKey(stored.Description))
        {
            weaponList.Remove(stored.Description);
            weaponList.Add(stored.Description, stored);
            success = true;
        }
        if (!success)
        {
            weaponList.Add(stored.Description, stored);
        }
    }

    /// <summary>
    /// Conversion from name to enum
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public int WeaponToBuy(string weapon)
    {
        switch(weapon.ToLower())
        {
            case "pistol":
            case "pistolcost":
                return (int)WeaponSelect.PISTOL;

            case "sub-machine gun":
            case "smgcost":
                return (int)WeaponSelect.SMG;

            case "shotgun":
            case "shotguncost":
                return (int)WeaponSelect.SHOTGUN;

            case "sniper rifle":
            case "snipercost":
                return (int)WeaponSelect.SNIPER;

            case "assault rifle":
            case "riflecost":
                return (int)WeaponSelect.RIFLE;

            case "unarmed":
                return (int)WeaponSelect.UNARMED;

            default: return (int)WeaponSelect.NONE;

        }
    }

    /// <summary>
    /// Takes an upgrade enum and applies the correct upgrade
    /// </summary>
    /// <param name="upgrade"></param>
    public void Upgrades(UpgradeType upgrade)
    {
        int wallet = PlayerController.Money;
        string cost = CostToFind(upgrade);
        WeaponUpgrades<GunClass>.UpgradeWeapon(GameManager.Instance.PlayerGun, upgrade, ref wallet);
        GameManager.Instance.EventManager.RemoveFromWallet(wallet);
        CostSheet.Instance.UpdateCostOf(cost);
    }

    /// <summary>
    /// Returns a weapon name based on an enum
    /// </summary>
    /// <param name="upgrade"></param>
    /// <returns></returns>
    private string CostToFind(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.DAMAGE: 
                return "DamageCost";
            case UpgradeType.RELOAD:
                return "ReloadSpeedCost";
            case UpgradeType.FIRERATE:
                return "FireRateCost";
            case UpgradeType.MAGSIZE:
                return "MagSizeCost";
            case UpgradeType.ACCURACY:
                return "AccuracyCost";

            default: return "";
        }
    }

    /// <summary>
    /// Changes the players weapon to a new one
    /// </summary>
    /// <param name="Weapon"></param>
    public void ChangeToGun(string Weapon)
    {
        GameManager.Instance.PlayerGun.ChangeWeapon(Weapon);
    }

    /// <summary>
    /// Takes a list of GunData and populates the weapons list with it.
    /// </summary>
    /// <param name="data"></param>
    public void SetUpGunStats(List<GunData> data)
    {
        foreach (GunData item in data)
        { 
            GunStats.Add(item);
        }
    }
}  

