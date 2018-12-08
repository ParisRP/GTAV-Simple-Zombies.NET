using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class MapProp : IMapObject, IIdentifier, IProp, ISpatial, IHandleable, IDeletable
  {
    public MapProp(string id, string propName, BlipSprite blipSprite, BlipColor blipColor, Vector3 groundOffset, bool interactable, bool isDoor, bool canBePickedUp, Vector3 rotation, Vector3 position, int handle, List<Weapon> weapons)
    {
      this.Id = id;
      this.PropName = propName;
      this.BlipSprite = blipSprite;
      this.BlipColor = blipColor;
      this.GroundOffset = groundOffset;
      this.Interactable = interactable;
      this.IsDoor = isDoor;
      this.CanBePickedUp = canBePickedUp;
      this.Rotation = rotation;
      this.Position = position;
      this.Handle = handle;
      this.Weapons = weapons;
    }

    public string Id { get; set; }

    public string PropName { get; set; }

    public BlipSprite BlipSprite { get; set; }

    public BlipColor BlipColor { get; set; }

    public Vector3 GroundOffset { get; set; }

    public bool Interactable { get; set; }

    public bool IsDoor { get; set; }

    public bool CanBePickedUp { get; set; }

    public Vector3 Rotation { get; set; }

    public Vector3 Position { get; set; }

    public int Handle { get; set; }

    public List<Weapon> Weapons { get; set; }

    public bool Exists()
    {
      return (bool) Function.Call<bool>((Hash) 8230805619690780346L, new InputArgument[1]
      {
        InputArgument.op_Implicit(this.Handle)
      });
    }

    public unsafe void Delete()
    {
      int handle = this.Handle;
      Function.Call((Hash) -5891624910369535543L, new InputArgument[1]
      {
        InputArgument.op_Implicit(&handle)
      });
      this.Handle = handle;
    }
  }
}
