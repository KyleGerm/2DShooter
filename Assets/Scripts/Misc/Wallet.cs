
public class Wallet : IWallet
{
    private int money = 1000;
    public int Money
    {
        get { return money; }
    }
    /// <summary>
    /// Adds currency to the wallet
    /// </summary>
    /// <param name="amount"></param>
    public void AddCurrency(int amount)
    {
        money += amount;
        UIManager.Instance.UpdateCurrency();
    }

    /// <summary>
    /// removes currency from the wallet
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveCurrency(int amount)
    {
        money -= amount;
        if (money < 0)
        {
            money = 0;
        }
       UIManager.Instance.UpdateCurrency();
    }
}
