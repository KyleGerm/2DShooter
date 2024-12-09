

public class EventManager 
{
    private delegate void ColorManagementEvent(float current, float Max);
    private delegate void AmmoCountEvent(int current, int max);
    private delegate void WaveEvent(int wave);
    private delegate void MoneyEvent(int amount);
    private delegate void EventList();
    private static event EventList OnDeath;
    private static event EventList Restart;
    private static event EventList EnemyDeath;
   
    private static MoneyEvent MoneyUp;
    private static MoneyEvent MoneyDown;

    private static event ColorManagementEvent OnHealthChange;
    private static event AmmoCountEvent AmmoCountChange;
    private static event WaveEvent OnWaveChange;

    public void BroadcastDeathEvent()
    {
        GameManager.Instance.ConfirmDead();
        OnDeath ?.Invoke();
    }

   public void SubscribeToDeath(ISubscribable component)
    {
        OnDeath += component.OnDeath;
    }
    public void SubscribeToRestart(ISubscribable component)
    {
        Restart += component.OnRestart;
    }

    public void SubscribeToEnemyDeath(IManagerSubscriptions component)
    {
        EnemyDeath += component.OnEnemyDeath;
    }

    public void BroadcastRestart()
    {
        Restart?.Invoke();
    }

    public void BroadCastEnemyDeath()
    {
        EnemyDeath?.Invoke();
    }

    public void SubscribeMyWallet(IWallet component)
    {
        MoneyUp += component.AddCurrency;
        MoneyDown += component.RemoveCurrency;
    }
    public void AddToWallet(int amount)
    {
        MoneyUp?.Invoke(amount);
    }

    public void RemoveFromWallet(int amount)
    {
        MoneyDown?.Invoke(amount);
    }

    public void SubscribeToHUDEvents(IHUDController component)
    {
        OnHealthChange += component.FormatHealthBar;
        AmmoCountChange += component.UpdateAmmoCount;
        OnWaveChange += component.UpdateWave;

    }

    public void ChangeHealthBar(float current, float max)
    {
        OnHealthChange?.Invoke(current, max);
    }

    public void ChangeAmmoCount(int current, int max)
    {
        AmmoCountChange?.Invoke(current, max);
    }
    
    public void ChangeWave(int wave)
    {
        OnWaveChange?.Invoke(wave);
    }
     


    public void Clear()
    {
        OnHealthChange = null;
        OnDeath = null;
        Restart = null;
        MoneyDown = null;
        MoneyUp = null;
        EnemyDeath = null;
        AmmoCountChange = null;
    }

}


