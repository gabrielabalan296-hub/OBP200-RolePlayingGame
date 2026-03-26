namespace OBP200_RolePlayingGame;

public interface ITransactions
{
    void SellMinorGems();
    void TryBuy(int cost, Action apply, string successMsg);
}