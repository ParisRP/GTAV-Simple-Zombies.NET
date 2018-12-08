using GTA.Native;
using System;

namespace ZombiesMod
{
  [Serializable]
  public class WeaponInventoryItem : InventoryItemBase, IWeapon, ICraftable
  {
    public WeaponInventoryItem(int amount, int maxAmount, string id, string description, int ammo, WeaponHash weaponHash, WeaponComponent[] weaponComponents)
      : base(amount, maxAmount, id, description)
    {
      this.Ammo = ammo;
      this.Hash = weaponHash;
      this.Components = weaponComponents;
    }

    public int Ammo { get; set; }

    public WeaponHash Hash { get; set; }

    public CraftableItemComponent[] RequiredComponents { get; set; }

    public WeaponComponent[] Components { get; set; }
  }
}
