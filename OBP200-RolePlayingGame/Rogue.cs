using System;
public class Rogue : Player
{
    public  Rogue(string name)
        :base (name, "Rogue" ,32, 32,8 , 3, 20, 3, 0, 1)
    {
    }

    public override void ApplyLevelUpStats()
    {
        MaxHealth+= 5; 
        Attack += 3; 
        Defense += 1;
    }

    private static readonly Random Rng = new Random();
    public override int CalculatePlayerDamage(int enemyDef)
    {
        int damage = base.CalculatePlayerDamage(enemyDef);
        damage += (Rng.NextDouble() < 0.2) ? 4 : 0; // rogue crit-chans
        return damage;
    }

    public override bool TryRunAway()
    {
        return Rng.NextDouble() < 0.5;
    }
    
    public override int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        int specialDmg;
        // Backstab: chans att ignorera försvar, hög risk/hög belöning
        if (Rng.NextDouble() < 0.5)
        {
            Console.WriteLine("Rogue utför en lyckad Backstab!");
            int atk = Attack;
            specialDmg = Math.Max(4, atk + 6);
        }
        else
        {
            Console.WriteLine("Backstab misslyckades!");
            specialDmg = 1;
        }
        // Dämpa skada mot bossen
        if (vsBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return specialDmg;
    }
}