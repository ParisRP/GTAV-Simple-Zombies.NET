using System;

namespace ZombiesMod
{
  [Serializable]
  public class CraftableInventoryItem : InventoryItemBase, ICraftable, IValidatable
  {
    public CraftableInventoryItem(int amount, int maxAmount, string id, string description, Func<bool> validation)
      : base(amount, maxAmount, id, description)
    {
      this.Validation = validation;
    }

    public CraftableItemComponent[] RequiredComponents { get; set; }

    public Func<bool> Validation { get; set; }
  }
}
