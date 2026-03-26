using System; 
using System.Linq; 
using System.Collections.Generic;

public abstract class Player : IDamage , ITransactions, IHeal, IUpgradePlayerStats, IPlayerStatus, ICombatAction
{
    // Spelarens "databas": alla värden 
    public string Name { get; set; }
    public string Class { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Attack  { get; set; }
    public int Defense { get; set; }
    public int Gold  { get; set; }
    public int Potions { get; set; }
    public int XP { get; set; } = 0; 
    public int Level { get; set; } = 1;

    private static readonly Random Rng = new Random();
    public List<string> Inventory { get; set; } = new List<string>();

    public Player(string Name, string Class, int Health, int MaxHealth, int attack, int defense, int gold, int potions,
        int xp, int level)
    {
        this.Name = Name;
        this.Class = Class;
        this.Health = Health;
        this.MaxHealth = MaxHealth;
        Attack = attack; 
        Defense = defense;
        Gold = gold;
        Potions = potions;
        XP = xp;
        Level = level;
        Inventory.Add( "Wooden Sword");
        Inventory.Add( "Cloth Armor" );
    }

    public bool IsPlayerDead()
    {
        return Health <= 0;
    }
    public abstract int UseClassSpecial(int enemyDef, bool vsBoss);
    
    public void ApplyDamageToPlayer(int dmg) 
    {
        Health -= Math.Max(0, dmg);
    }

    public virtual int CalculatePlayerDamage(int enemyDef)
    {
        // Beräkna grundskada
        int baseDmg = Math.Max(1, Attack - (enemyDef / 2));
        int roll = Rng.Next(0, 3); // liten variation
        
        return (baseDmg + roll);
    }
    
    public void AddPlayerXp(int amount)
    {
        XP +=Math.Max (0, amount) ;
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
        MaxHealth += 4;
        Attack += 3;
        Defense += 1;
    }
    public void MaybeLevelUp()
    {
        // Nivåtrösklar
        int nextThreshold = Level == 1 ? 10 : (Level == 2 ? 25 : (Level == 3 ? 45 : Level * 20));


        if (XP >= nextThreshold)
        {
            Level++;
            ApplyLevelUpStats();
            Health = MaxHealth;

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
            Console.WriteLine("Du har inga drycker kvar."); 
            return; 
        } 
        int oldHealth = Health;
        
        // Helning av spelaren
        int heal = 12;
        Health = Math.Min(MaxHealth, Health + heal); 
        Potions = (Potions - 1); 
        int healAmount = Health - oldHealth;
        
        Console.WriteLine($"Du dricker en dryck och återfår {healAmount} HP."); 
    }
    
    public bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        Health = MaxHealth;
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