namespace ZombiesMod
{
  public interface ICraftable
  {
    CraftableItemComponent[] RequiredComponents { get; set; }
  }
}
