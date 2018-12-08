using GTA;
using GTA.Math;

namespace ZombiesMod
{
  public interface IProp
  {
    string PropName { get; set; }

    BlipSprite BlipSprite { get; set; }

    BlipColor BlipColor { get; set; }

    Vector3 GroundOffset { get; set; }

    bool Interactable { get; set; }

    bool IsDoor { get; set; }

    bool CanBePickedUp { get; set; }
  }
}
