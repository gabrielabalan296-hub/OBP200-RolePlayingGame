public class HealthSystem
{
    public int Health {get; private set;}
    public int MaxHealth {get; set;}
    public bool IsPlayerDead => Health <= 0;

    public HealthSystem(int health, int maxHealth)
    {
        MaxHealth = maxHealth;
        Health = health;
    }
    
    public void ApplyDamageToPlayer(int dmg) 
    {
        Health -= Math.Max(0, dmg);
    }

    public int Heal(int amount)
    {
        int oldHealth = Health;
        Health = Math.Min(Health + amount, MaxHealth);
        return Health - oldHealth;
    }
    public void RestoreToFullHealth()
    {
        Health = MaxHealth;
    }
}