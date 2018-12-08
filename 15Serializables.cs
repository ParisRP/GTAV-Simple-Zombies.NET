using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ZombiesMod
{
  [Serializable]
  public class VehicleData : ISpatial, IHandleable, IDeletable
  {
    public VehicleData(int handle, int hash, Vector3 rotation, Vector3 position, VehicleColor primaryColor, VehicleColor secondaryColor, int health, float engineHealth, float heading, VehicleNeonLight[] neonLights, List<Tuple<VehicleMod, int>> mods, VehicleToggleMod[] toggleMods, VehicleWindowTint windowTint, VehicleWheelType wheelType, Color neonColor, int livery, bool wheels1, bool wheels2)
    {
      this.Handle = handle;
      this.Hash = hash;
      this.Rotation = rotation;
      this.Position = position;
      this.PrimaryColor = primaryColor;
      this.SecondaryColor = secondaryColor;
      this.Health = health;
      this.EngineHealth = engineHealth;
      this.Heading = heading;
      this.NeonLights = neonLights;
      this.Mods = mods;
      this.ToggleMods = toggleMods;
      this.WindowTint = windowTint;
      this.WheelType = wheelType;
      this.NeonColor = neonColor;
      this.Livery = livery;
      this.Wheels1 = wheels1;
      this.Wheels2 = wheels2;
    }

    public int Handle { get; set; }

    public int Hash { get; set; }

    public Vector3 Rotation { get; set; }

    public Vector3 Position { get; set; }

    public VehicleColor PrimaryColor { get; set; }

    public VehicleColor SecondaryColor { get; set; }

    public int Health { get; set; }

    public float EngineHealth { get; set; }

    public float Heading { get; set; }

    public VehicleNeonLight[] NeonLights { get; set; }

    public List<Tuple<VehicleMod, int>> Mods { get; set; }

    public VehicleToggleMod[] ToggleMods { get; set; }

    public VehicleWindowTint WindowTint { get; set; }

    public VehicleWheelType WheelType { get; set; }

    public Color NeonColor { get; set; }

    public int Livery { get; set; }

    public bool Wheels1 { get; set; }

    public bool Wheels2 { get; set; }

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
