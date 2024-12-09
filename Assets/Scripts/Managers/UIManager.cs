using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager> ,ISubscribable
{
    [SerializeField] private GameObject PausedBanner;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject DeathMenu;
    [SerializeField] private GameObject UpgradesMenu;
    [SerializeField] private GameObject WeaponSelectMenu;
    [SerializeField] private GameObject ScoreBanner;
    [SerializeField] private GameObject CurrencyBanner;
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private TextMeshProUGUI WinText;
    private TextMeshProUGUI Score;
    private TextMeshProUGUI Currency;
    
    //Returns true if the cursor is over a button
    public bool OverButton
    {
        get { return EventSystem.current.currentSelectedGameObject != null ? true : false; }
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Score = ScoreBanner.GetComponent<TextMeshProUGUI>();
        Currency = CurrencyBanner.GetComponent<TextMeshProUGUI>();
        GameManager.Instance.EventManager.SubscribeToDeath(this); 
        GameManager.Instance.EventManager.SubscribeToRestart(this);
        SetPauseMenu();
        SetDeathMenu();
        SetUpgradesMenu();
        SetWeaponSelectMenu();
        UpdateScore();
        UpdateCurrency();
    }
    /// <summary>
    /// Sets the pause menu to active or inactive based on if the game manager says the game is paused.
    /// </summary>
    public void SetPauseMenu()
    {
       if (GameManager.Instance.IsDead) { return; } 

        switch (GameManager.Instance.IsPaused)
        {
            case true:
                PausedBanner.SetActive(true);
                PauseButton.SetActive(false);
                break;
            case false:
                PausedBanner.SetActive(false);
                PauseButton.SetActive(true);
                break;

        }
    }
    /// <summary>
    /// Sets the death menu based on if the game manager says the player is dead 
    /// </summary>
    public void SetDeathMenu()
    {
        switch (GameManager.Instance.IsDead)
        {
            case true:
            DeathMenu.SetActive(true);
                if(UpgradesMenu.activeInHierarchy)
                {
                    UpgradesMenu.SetActive(false);
                }
                else if(WeaponSelectMenu.activeInHierarchy)
                {
                    WeaponSelectMenu.SetActive(false);
                }
                break;

            default: DeathMenu.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Deactivates the upgrades menu
    /// </summary>
    public void SetUpgradesMenu()
    {
        UpgradesMenu.SetActive(false);
    }
    /// <summary>
    /// Sets the weapon select menu
    /// </summary>
    public void SetWeaponSelectMenu()
    {
        if (GameManager.Instance.IsDead)
        {
            WeaponSelectMenu.SetActive(true);
            CostSheet.Instance.UpdateAllWeaponsCost();
            DeathMenu.SetActive(false);
        }
        else WeaponSelectMenu.SetActive(false);
    }


    public void OnDeath()
    {
        GameScreen.SetActive(false);
        SetDeathMenu();
    }
    /// <summary>
    /// resets all screens to the default state
    /// </summary>
    public void OnRestart()
    {
        SetPauseMenu();
        SetDeathMenu();
        SetUpgradesMenu();
        SetWeaponSelectMenu();
        GameScreen.SetActive(true);
        UpdateScore();
        UpdateCurrency();
    }
    /// <summary>
    /// activates this menu and deactivates the death menu
    /// </summary>
    public void SelectUpgradesMenu()
    {
        DeathMenu.SetActive(false);
        UpgradesMenu.SetActive(true);
        StatsPanel.Instance.UpdateStats();
        CostSheet.Instance.UpdateAllUpgradesCost();
    }
    /// <summary>
    /// updates the score to show the current value
    /// </summary>
    public void UpdateScore()
    {
        Score.text = new string($"Score: {GameManager.Instance.Score}");
    } 
    /// <summary>
    /// Updates the currency to show the current value of the players wallet
    /// </summary>
    public void UpdateCurrency()
    {
        try
        {
             Currency.text = new string($"Gold: {PlayerController.Money}");
        }
        catch
        {
            return;
        }
       
    }
    /// <summary>
    /// Reconfigures the death screen to show a win message instead
    /// </summary>
    public void SetWinMenu()
    {
      
        WinText.text = new string("YOU WIN!!");
        GameScreen.SetActive(false);
        DeathMenu.SetActive(true);
    }
}
