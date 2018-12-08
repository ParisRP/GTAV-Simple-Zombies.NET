using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using ZombiesMod.PlayerManagement;

namespace ZombiesMod
{
  [Serializable]
  public class PedData : ISpatial, IHandleable, IDeletable
  {
    public PedData(int handle, int hash, Vector3 rotation, Vector3 position, PedTask task, List<Weapon> weapons)
    {
      this.Handle = handle;
      this.Hash = hash;
      this.Rotation = rotation;
      this.Position = position;
      this.Task = task;
      this.Weapons = weapons;
    }

    public int Handle { get; set; }

    public int Hash { get; set; }

    public Vector3 Rotation { get; set; }

    public Vector3 Position { get; set; }

    public PedTask Task { get; set; }

    public List<Weapon> Weapons { get; set; }

    public bool Exists()
    {
      return (bool) Function.Call<bool>((GTA.Native.Hash) 8230805619690780346L, new InputArgument[1]
      {
        InputArgument.op_Implicit(this.Handle)
      });
    }

    public unsafe void Delete()
    {
      int handle = this.Handle;
      Function.Call((GTA.Native.Hash) -5891624910369535543L, new InputArgument[1]
      {
        InputArgument.op_Implicit(&handle)
      });
      this.Handle = handle;
    }
  }
}
