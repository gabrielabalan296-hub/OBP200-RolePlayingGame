namespace OBP200_RolePlayingGame;

public interface IDamage
{
    void ApplyDamageToPlayer(int dmg);
    int CalculatePlayerDamage(int enemyDef); 
}