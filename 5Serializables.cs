using System;

namespace ZombiesMod
{
  [Serializable]
  public class UsableInventoryItem : InventoryItemBase, ICraftable
  {
    public UsableInventoryItem(int amount, int maxAmount, string id, string description, UsableItemEvent[] itemEvents)
      : base(amount, maxAmount, id, description)
    {
      this.ItemEvents = itemEvents;
    }

    public UsableItemEvent[] ItemEvents { get; set; }

    public CraftableItemComponent[] RequiredComponents { get; set; }
  }
}
