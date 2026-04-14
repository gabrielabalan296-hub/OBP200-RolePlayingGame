using System; 
using System.Linq; 
using System.Collections.Generic;

public abstract class Player : IDamage, ITransactions, IHeal, IUpgradePlayerStats, IPlayerStatus, ICombatAction
{
    // Spelarens "databas": alla värden 
    public string Name { get; set; }
    public string Class { get; set; }
    public int Health => HPSystem.Health; 
    public int MaxHealth => HPSystem.MaxHealth;
    public int Attack  { get; set; }
    public int Defense { get; set; }
    public int Gold  { get; set; }
    public int Potions { get; set; }
    public int XP => LvlSystem.XP; 
    public int Level => LvlSystem.Level;
    public HealthSystem HPSystem { get; set; }
    public LevelUpSystem LvlSystem { get; set; }

    protected static readonly Random Rng = new Random();
    public List<string> Inventory { get; set; } = new List<string>();

    public Player(string Name, string Class, int Health, int MaxHealth, int attack, int defense, int gold, int potions,
        int xp, int level)
    {
        HPSystem = new HealthSystem(Health, MaxHealth);
        LvlSystem = new LevelUpSystem(level, xp);
        this.Name = Name;
        this.Class = Class; 
        Attack = attack; 
        Defense = defense;
        Gold = gold;
        Potions = potions;
        Inventory.Add( "Wooden Sword");
        Inventory.Add( "Cloth Armor" );
    }

    public bool IsPlayerDead() => HPSystem.IsPlayerDead; 
    public abstract int UseClassSpecial(int enemyDef, bool vsBoss);
    
    public void ApplyDamageToPlayer(int dmg) 
    {
        HPSystem.ApplyDamageToPlayer(dmg);
    }

    public virtual int CalculatePlayerDamage(int enemyDef)
    {
        // Beräkna grundskada
        int baseDmg = Math.Max(1, Attack - (enemyDef / 2));
        int roll = Rng.Next(0, 3); // liten variation
        
        return (baseDmg + roll);
    }

    protected int BossDamageReduction(int damage, bool vsBoss)
    {
        // Dämpa skada mot bossen
        if (vsBoss)
        {
            return (int)Math.Round(damage * 0.8);
        }
        return damage;
    }
    
    public void AddPlayerXp(int amount)
    {
        LvlSystem.AddXP(amount);
        MaybeLevelUp();
    }
    
    public void AddPlayerGold(int amount)
    {
        Gold +=Math.Max (0, amount);
    }
    public void ShowStatus()
    {
    Console.WriteLine($"Namn: {Name}, Klass: {Class} " + $"HP: {Health} / {MaxHealth}  " + 
                          $"ATK: {Attack}  DEF: {Defense} Level:  {Level} " + 
                          $"XP:  {XP}  Guld: {Gold}  Drycker {Potions}");
        
        if (Inventory != null && Inventory.Any())
        {
            Console.WriteLine($"Väska: {string.Join(", ", Inventory)}");
        }
    }

    public virtual void ApplyLevelUpStats()
    {
        HPSystem.MaxHealth += 4;
        Attack += 3;
        Defense += 1;
    }
    public void MaybeLevelUp()
    {
        while (LvlSystem.VerifyLevellingUp())
        {
            ApplyLevelUpStats();
            HPSystem.RestoreToFullHealth();

            Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
        }
    }
    
   public virtual bool TryRunAway()
    {
        return Rng.NextDouble() < 0.25;
    }
    
    public void UsePotion()
    { 
        if (Potions <= 0) 
        { 
            Console.WriteLine ( "Du har inga drycker kvar.");
        } 
        // Helning av spelaren
        
        Potions--; 
        int healAmount = HPSystem.Heal(12);
        
        Console.WriteLine ( $"Du dricker en dryck och återfår {healAmount} HP."); 
    }
    
    public bool DoRest()
    {
       Console.WriteLine ("Du slår läger och vilar.");
        HPSystem.RestoreToFullHealth();
        Console.WriteLine("HP återställt till max.");
        return true;
    }
    public void TryBuy(int cost, Action apply, string successMsg)
    {
        if (Gold >= cost)
        {
            apply();
            Console.WriteLine(successMsg);
        }
        else
        {
            Console.WriteLine("Du har inte råd.");
        }
    }
    
    public void SellMinorGems()
    {
        if (Inventory.Count == 0)
        {
            Console.WriteLine("Du har inga föremål att sälja.");
            return;
        }

        int count = Inventory.Count(item  => item == "Minor Gem");
        if (count == 0)
        {
            Console.WriteLine("Inga 'Minor Gem' i väskan.");
            return;
        }

        Inventory.RemoveAll(x => x == "Minor Gem");

        AddPlayerGold(count * 5);
        Console.WriteLine($"Du säljer {count} st Minor Gem för {count * 5} guld.");
    }
}