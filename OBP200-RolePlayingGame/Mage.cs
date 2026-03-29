namespace OBP200_RolePlayingGame;
using System;
public class Mage : Player
{
    public Mage(string name)
        :base (name, "Mage", 28, 28, 10, 2, 2, 15, 0, 1)
    { }

    public override bool TryRunAway()
    {
        return Rng.NextDouble() < 0.35;
    }
    public override void ApplyLevelUpStats()
    {
        HPSystem.MaxHealth += 4; 
        Attack += 4; 
        Defense += 1;
    }

    public override int CalculatePlayerDamage(int enemyDef)
    {
        int damage = base.CalculatePlayerDamage(enemyDef);
        damage += 2; // mage buff
        return damage;
    }

    public override int UseClassSpecial(int enemyDef,  bool vsBoss)
    {
        // Fireball: stor skada, kostar guld
        int specialDmg; 
        int gold = Gold;
        if (gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            Gold = (gold - 3);
            int atk = Attack;
            specialDmg = Math.Max(3, atk + 5 - (enemyDef / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            specialDmg = 0;
        }
        
        return BossDamageReduction(specialDmg, vsBoss);
    }
}