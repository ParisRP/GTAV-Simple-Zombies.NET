namespace ZombiesMod
{
  public interface IFood : IAnimatable
  {
    FoodType FoodType { get; set; }

    float RestorationAmount { get; set; }
  }
}
