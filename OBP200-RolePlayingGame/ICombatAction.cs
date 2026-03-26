public interface ICombatAction
{
    int UseClassSpecial(int enemyDef, bool IsBoss);
    bool TryRunAway();
}