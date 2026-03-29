public class LevelUpSystem
{
  public int Level { get; set; }
  public int XP { get; set; }
  public LevelUpSystem(int level, int xp)
  {
    Level = level;
    XP = xp;
  }
  
  public void AddXP (int amount)
  {
    XP += Math.Max(0, amount);
  }

  private int NextThreshold()
  {
    // Nivåtrösklar
    int nextThreshold = Level == 1 ? 10 : (Level == 2 ? 25 : (Level == 3 ? 45 : Level * 20));
    if (Level == 1) return 10;
    if (Level == 2) return 25; 
    if (Level == 3) return 45;
    return Level * 20;
  }

  public bool VerifyLevellingUp()
  {
    if (XP >= NextThreshold())
    {
      Level++;
      return true;
    }
    return false;
  }
}