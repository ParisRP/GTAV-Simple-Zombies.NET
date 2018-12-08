using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class WeaponStorageInventoryItem : BuildableInventoryItem
  {
    public WeaponStorageInventoryItem(int amount, int maxAmount, string id, string description, string propName, BlipSprite blipSprite, BlipColor blipColor, Vector3 groundOffset, bool interactable, bool isDoor, bool canBePickedUp, List<Weapon> weaponsList)
      : base(amount, maxAmount, id, description, propName, blipSprite, blipColor, groundOffset, interactable, isDoor, canBePickedUp)
    {
      this.WeaponsList = weaponsList;
    }

    public List<Weapon> WeaponsList { get; set; }
  }
}
