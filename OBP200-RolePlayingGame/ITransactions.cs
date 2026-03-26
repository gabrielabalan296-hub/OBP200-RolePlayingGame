public interface ITransactions
{
    void SellMinorGems();
    void TryBuy(int cost, Action apply, string successMsg);
}