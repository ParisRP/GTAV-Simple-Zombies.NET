using GTA;
using GTA.Math;
using System;

namespace ZombiesMod
{
  [Serializable]
  public class BuildableInventoryItem : InventoryItemBase, IProp, ICraftable
  {
    public BuildableInventoryItem(int amount, int maxAmount, string id, string description, string propName, BlipSprite blipSprite, BlipColor blipColor, Vector3 groundOffset, bool interactable, bool isDoor, bool canBePickedUp)
      : base(amount, maxAmount, id, description)
    {
      this.PropName = propName;
      this.BlipSprite = blipSprite;
      this.BlipColor = blipColor;
      this.GroundOffset = groundOffset;
      this.Interactable = interactable;
      this.IsDoor = isDoor;
      this.CanBePickedUp = canBePickedUp;
    }

    public string PropName { get; set; }

    public BlipSprite BlipSprite { get; set; }

    public BlipColor BlipColor { get; set; }

    public Vector3 GroundOffset { get; set; }

    public bool Interactable { get; set; }

    public bool IsDoor { get; set; }

    public bool CanBePickedUp { get; set; }

    public CraftableItemComponent[] RequiredComponents { get; set; }
  }
}
