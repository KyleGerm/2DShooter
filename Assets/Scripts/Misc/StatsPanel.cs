using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanel : Singleton<StatsPanel>
{
 
    // Stores gameobjects assigned to it in the editor
    [SerializeField] private List<GameObject> Text;

    //used later for easier organisation of components
    private Dictionary<string, TextMeshProUGUI> elements = new Dictionary<string, TextMeshProUGUI>();

    //Gets a new component assigned to it in each method, depending on the one we need to manipulate
    TextMeshProUGUI Textline;

    //used to track length of string so we know at what point a color change should happen
    int length;
    private void Awake()
    {
        //Adds each TMPUGUI component to a dictionary with a key defined by the name of the gameObject in the editor
        foreach(var item in Text)
        {
            elements.Add(item.name,item.GetComponent<TextMeshProUGUI>());
        }
    }

    /// <summary>
    /// Used externally to update all parameters of the class
    /// </summary>
    public void UpdateStats()
    {
        UpdateName();   

      for (int i = 0;i < 5; i++)
        {
            UpdateValue((UpgradeType)i);
        }
    }

    //Simple method to return the appropriate TMPUGUI that we want to use from the dictionary
    //means we can dynamically update the value of one field instead of having multiple for one instance 
    private TextMeshProUGUI GetLine(string name)
    {
        if (elements.ContainsKey(name))
        {
            return elements[name];
        }

       return null;
    }

    /// <summary>
    /// Takes in Parameters and outputs them to the correct TMPGUI.
    /// </summary>
    /// <param name="line">The TMPGUI to use, and the name of the Upgrade to show in the text</param>
    /// <param name="current">Current Value</param>
    /// <param name="difference">The difference between the current value, and the upgraded value</param>
    /// <param name="currentLevel">Current Level of the Upgrade Type</param>
    /// <param name="MaxLevel">Max Level of the Upgrade Type</param>
    private void UpdateLine(string line, string current, string difference, int currentLevel, int MaxLevel)
    {
        Textline = GetLine(line);
        if(Textline != null && currentLevel < MaxLevel)
        {
            Textline.text = new string($"{line}: {current}");
            length = Textline.text.Length;
            if(line == "Reload Speed")
            {
                Textline.text += $" -{difference}";
            }
            else
            {
                Textline.text += $" +{difference}";
            }

            Textline.text = Textline.text.Substring(0, length) + "<color=green>" + Textline.text.Substring(length);
        }
        else
        {
            Textline.text = new string($"{line}: {current}");
        }
        

    }

    private void UpdateName()
    {
        Textline = GetLine("CurrentWeapon");

        Textline.text = new string($"Current Weapon: {GameManager.Instance.PlayerGun.Description}");
    }

    /// <summary>
    /// Finds the correct values and applies them to the UpdateLine method
    /// </summary>
    /// <param name="type"></param>
    public void UpdateValue(UpgradeType type)
    {
        string line = "";
        string current = "";
        string difference = "";
        WeaponUpgrades<GunClass>.GetLevelValues(GameManager.Instance.PlayerGun, type, out int currentLevel, out int maxLevel);
        switch (type)
        {
            case UpgradeType.ACCURACY:
                line = "Accuracy";
                WeaponUpgrades<GunClass>.AccuracyPercentageValuesToString(GameManager.Instance.PlayerGun, out current, out difference);
                break;
            case UpgradeType.RELOAD:
                line = "Reload Speed";
                WeaponUpgrades<GunClass>.ReloadSpeedValuesToString(GameManager.Instance.PlayerGun, out current, out difference);
                break;
            case UpgradeType.DAMAGE:
                line = "Damage";
                WeaponUpgrades<GunClass>.DamageValuesToString(GameManager.Instance.PlayerGun, out current, out difference);
                break;
            case UpgradeType.FIRERATE:
                line = "FireRate";
                WeaponUpgrades<GunClass>.FireRateValuesToString(GameManager.Instance.PlayerGun, out current, out difference);
                break;
            case UpgradeType.MAGSIZE:
                line = "MagSize";
                WeaponUpgrades<GunClass>.MagSizeValuesToString(GameManager.Instance.PlayerGun, out current, out difference);
                break;
        }
        UpdateLine(line, current, difference, currentLevel, maxLevel);
    }
}
