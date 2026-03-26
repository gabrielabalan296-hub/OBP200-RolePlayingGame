public interface IUpgradePlayerStats
{
    void AddPlayerXp(int amount);
    void AddPlayerGold(int amount);
    void ApplyLevelUpStats();
    void MaybeLevelUp();
}