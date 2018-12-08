using NativeUI;
using System;

namespace ZombiesMod
{
  [Serializable]
  public class InventoryItemBase : IIdentifier
  {
    [NonSerialized]
    public UIMenuItem MenuItem;

    public InventoryItemBase(int amount, int maxAmount, string id, string description)
    {
      this.Amount = amount;
      this.MaxAmount = maxAmount;
      this.Id = id;
      this.Description = description;
    }

    public void CreateMenuItem()
    {
      this.MenuItem = new UIMenuItem(this.Id, this.Description);
    }

    public int Amount { get; set; }

    public int MaxAmount { get; set; }

    public string Id { get; set; }

    public string Description { get; set; }
  }
}
