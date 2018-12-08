using GTA;
using System;

namespace ZombiesMod
{
  [Serializable]
  public class FoodInventoryItem : InventoryItemBase, IFood, IAnimatable, ICraftable
  {
    public FoodInventoryItem(int amount, int maxAmount, string id, string description, string animationDict, string animationName, AnimationFlags animationFlags, int animationDuration, FoodType foodType, float restorationAmount)
      : base(amount, maxAmount, id, description)
    {
      this.AnimationDict = animationDict;
      this.AnimationName = animationName;
      this.AnimationFlags = animationFlags;
      this.AnimationDuration = animationDuration;
      this.FoodType = foodType;
      this.RestorationAmount = restorationAmount;
    }

    public string AnimationDict { get; set; }

    public string AnimationName { get; set; }

    public AnimationFlags AnimationFlags { get; set; }

    public int AnimationDuration { get; set; }

    public FoodType FoodType { get; set; }

    public float RestorationAmount { get; set; }

    public CraftableItemComponent[] RequiredComponents { get; set; }

    public NearbyResource NearbyResource { get; set; }
  }
}
