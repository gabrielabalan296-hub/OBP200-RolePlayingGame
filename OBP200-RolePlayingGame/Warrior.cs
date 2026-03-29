namespace OBP200_RolePlayingGame;
using System; 
public class Warrior : Player 
{
    public Warrior(string name)
        :base (name, "Warrior", 40, 40, 7, 5, 2, 15, 0, 1)
    {}

    public override void ApplyLevelUpStats()
    {
        HPSystem.MaxHealth += 6; 
        Attack += 2; 
        Defense += 2;
    }

    public override bool TryRunAway()
    {
        return Rng.NextDouble() < 0.25;
    }

    public override int CalculatePlayerDamage(int enemyDef)
    {
        int damage = base.CalculatePlayerDamage(enemyDef);
        damage += 1; // warrior buff
        return damage;
    }

    public override int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg;
        
        // Heavy Strike: hög skada men självskada
        Console.WriteLine("Warrior använder Heavy Strike!");
        int atk = Attack;
        specialDmg = Math.Max(2, atk + 3 - enemyDef);
        ApplyDamageToPlayer(2); // självskada
       
        return BossDamageReduction(specialDmg, vsBoss);
    }
}