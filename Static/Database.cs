using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZombiesMod.Static
{
  public static class Database
  {
    public static Random Random;
    public static string[] VehicleModels;
    public static WeaponHash[] WeaponHashes;
    public static VehicleClass[] LandVehicleClasses;
    public static VehicleHash[] VehicleHashes;
    public static Model[] WrckedVehicleModels;
    public static Vector3[] AnimalSpawns;
    public static Vector3[] Shops247Locations;

    public static Ped PlayerPed
    {
      get
      {
        return Database.Player.get_Character();
      }
    }

    public static Player Player
    {
      get
      {
        return Game.get_Player();
      }
    }

    public static Vehicle PlayerCurrentVehicle
    {
      get
      {
        return Database.PlayerPed.get_CurrentVehicle();
      }
    }

    public static PedGroup PlayerGroup
    {
      get
      {
        return Database.PlayerPed.get_CurrentPedGroup();
      }
    }

    public static bool PlayerIsDead
    {
      get
      {
        return ((Entity) Database.PlayerPed).get_IsDead();
      }
    }

    public static bool PlayerInVehicle
    {
      get
      {
        return Database.PlayerPed.IsInVehicle();
      }
    }

    public static bool PlayerIsSprinting
    {
      get
      {
        return Database.PlayerPed.get_IsSprinting();
      }
    }

    public static int PlayerHealth
    {
      get
      {
        return ((Entity) Database.PlayerPed).get_Health();
      }
      set
      {
        ((Entity) Database.PlayerPed).set_Health(value);
      }
    }

    public static int PlayerMaxHealth
    {
      get
      {
        return ((Entity) Database.PlayerPed).get_MaxHealth();
      }
    }

    public static Vector3 PlayerPosition
    {
      get
      {
        return ((Entity) Database.PlayerPed).get_Position();
      }
    }

    public static VehicleHash GetRandomVehicleByClass(VehicleClass vClass)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Database.\u003C\u003Ec__DisplayClass29_0 cDisplayClass290 = new Database.\u003C\u003Ec__DisplayClass29_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass290.vClass = vClass;
      VehicleHash[] values = Enum.GetValues(typeof (VehicleHash)) as VehicleHash[];
      if (values == null)
        return (VehicleHash) -1216765807;
      // ISSUE: method pointer
      VehicleHash[] array = ((IEnumerable<VehicleHash>) Enumerable.Where<VehicleHash>((IEnumerable<M0>) values, (Func<M0, bool>) new Func<VehicleHash, bool>((object) cDisplayClass290, __methodptr(\u003CGetRandomVehicleByClass\u003Eb__0)))).ToArray<VehicleHash>();
      return (VehicleHash) (int) (uint) array[Database.Random.Next(array.Length)];
    }

    public static Model GetRandomVehicleModel()
    {
      Model model;
      ((Model) ref model).\u002Ector(Database.VehicleModels[Database.Random.Next(Database.VehicleModels.Length)]);
      return ((Model) ref model).Request(1500) ? model : Model.op_Implicit((string) null);
    }

    static Database()
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
