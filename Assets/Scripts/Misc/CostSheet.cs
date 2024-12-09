using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostSheet : Singleton<CostSheet>
{
    [SerializeField] private List<TextMeshProUGUI> UpgradeCostings;
    [SerializeField] private List<TextMeshProUGUI> WeaponCostings;
    [SerializeField] private List<TextMeshProUGUI> Wallets;

    // private Dictionary<string, TextMeshProUGUI> UpgradeCost = new Dictionary<string, TextMeshProUGUI>() ;

    private TextMeshProUGUI CostText;
    
    /// <summary>
    /// Updates all TMPUGUI in the Upgrades list
    /// </summary>
   public void UpdateAllUpgradesCost()
    {
        foreach (var item in UpgradeCostings)
        {
            item.text = new string($"Cost: {GameManager.Instance.PlayerGun.GetCosting(item.name)}");
        }
        UpdateWallet("UpgradesWallet");
    }

    /// <summary>
    /// Updates all TMPUGUI in the weapons list
    /// </summary>
    public void UpdateAllWeaponsCost()
    {
         foreach (var item in WeaponCostings)
        {
            item.text = new string ($"Cost: {GameManager.Instance.GunManager.WeaponPrice[GameManager.Instance.GunManager.WeaponToBuy(item.name)]}");
        }
        UpdateWallet("WeaponsWallet");
    }

    /// <summary>
    /// Updates one TMPUGUI in the list
    /// </summary>
    /// <param name="subject"></param>
    public void UpdateCostOf(string subject)
    {
        CostText = GetCostIn(subject, UpgradeCostings);
        CostText.text = new string($"Cost: {GameManager.Instance.PlayerGun.GetCosting(subject)}");
        UpdateWallet("UpgradesWallet");
    }
    /// <summary>
    /// Updates the wallet text in a specified screen 
    /// </summary>
    /// <param name="wallet"></param>
    private void UpdateWallet(string wallet)
    {
        CostText = GetCostIn(wallet, Wallets);
        CostText.text = new string($"Gold: {PlayerController.Money}");
    }

    /// <summary>
    /// Returns the correct TMPUGUI from all elements
    /// </summary>
    /// <param name="name"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private TextMeshProUGUI GetCostIn(string name, List<TextMeshProUGUI> list)
    {
        if (list!= null)
        {
            foreach (var item in list)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
        }
        return null;
    }
}
