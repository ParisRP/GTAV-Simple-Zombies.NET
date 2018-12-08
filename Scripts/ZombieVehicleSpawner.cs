using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.DataClasses;
using ZombiesMod.Extensions;
using ZombiesMod.Static;

namespace ZombiesMod.Scripts
{
  public class ZombieVehicleSpawner : Script, ISpawner
  {
    public const int SpawnBlockingDistance = 150;
    private readonly int _maxVehicles;
    private readonly int _maxZombies;
    private readonly int _minVehicles;
    private readonly int _minZombies;
    private readonly int _spawnDistance;
    private readonly int _minSpawnDistance;
    private readonly int _zombieHealth;
    private bool _nightFall;
    private List<Ped> _peds;
    private List<Vehicle> _vehicles;
    private readonly VehicleClass[] _classes;
    public string[] InvalidZoneNames;

    public ZombieVehicleSpawner()
    {
      // ISSUE: unable to decompile the method.
    }

    public bool Spawn { get; set; }

    public SpawnBlocker SpawnBlocker { get; }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    private static Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    public static ZombieVehicleSpawner Instance { get; private set; }

    private void OnTick(object sender, EventArgs e)
    {
      if (this.Spawn)
      {
        if (!MenuConrtoller.MenuPool.IsAnyMenuOpen())
        {
          if (ZombieCreator.IsNightFall() && !this._nightFall)
          {
            UiExtended.DisplayHelpTextThisFrame("Nightfall approaches. Zombies are far more ~r~aggressive~s~ at night.", false);
            this._nightFall = true;
          }
          else if (!ZombieCreator.IsNightFall())
            this._nightFall = false;
        }
        this.SpawnVehicles();
        this.SpawnPeds();
      }
      else
        this.ClearAll();
    }

    private void SpawnPeds()
    {
      // ISSUE: method pointer
      this._peds = ((IEnumerable<Ped>) Enumerable.Where<Ped>((IEnumerable<M0>) this._peds, (Func<M0, bool>) new Func<Ped, bool>((object) null, __methodptr(Exists)))).ToList<Ped>();
      if (this._peds.Count >= this._maxZombies)
        return;
      for (int index = 0; index < this._maxZombies - this._peds.Count; ++index)
      {
        Vector3 position = ((Entity) ZombieVehicleSpawner.PlayerPed).get_Position();
        Vector3 spawnPoint = ((Vector3) ref position).Around((float) this._spawnDistance);
        spawnPoint = World.GetNextPositionOnStreet(spawnPoint);
        if (!this.IsValidSpawn(spawnPoint))
          break;
        Vector3 vector3 = ((Vector3) ref spawnPoint).Around(5f);
        if (vector3.IsOnScreen() || (double) vector3.VDist(ZombieVehicleSpawner.PlayerPosition) < (double) this._minSpawnDistance)
          break;
        Ped randomPed = World.CreateRandomPed(vector3);
        if (!Entity.op_Equality((Entity) randomPed, (Entity) null))
          this._peds.Add((Ped) ZombieCreator.InfectPed(randomPed, this._zombieHealth, false));
      }
    }

    private void SpawnVehicles()
    {
      // ISSUE: method pointer
      this._vehicles = ((IEnumerable<Vehicle>) Enumerable.Where<Vehicle>((IEnumerable<M0>) this._vehicles, (Func<M0, bool>) new Func<Vehicle, bool>((object) null, __methodptr(Exists)))).ToList<Vehicle>();
      if (this._vehicles.Count >= this._maxVehicles)
        return;
      for (int index = 0; index < this._maxVehicles - this._vehicles.Count; ++index)
      {
        Vector3 position = ((Entity) ZombieVehicleSpawner.PlayerPed).get_Position();
        Vector3 vector3_1 = ((Vector3) ref position).Around((float) this._spawnDistance);
        vector3_1 = World.GetNextPositionOnStreet(vector3_1);
        if (this.IsInvalidZone(vector3_1) || !this.IsValidSpawn(vector3_1))
          break;
        Vector3 vector3_2 = ((Vector3) ref vector3_1).Around(2.5f);
        if (vector3_2.IsOnScreen() || (double) vector3_2.VDist(ZombieVehicleSpawner.PlayerPosition) < (double) this._minSpawnDistance)
          break;
        Vehicle vehicle = World.CreateVehicle(Database.GetRandomVehicleModel(), vector3_2);
        if (!Entity.op_Equality((Entity) vehicle, (Entity) null))
        {
          vehicle.set_EngineHealth(0.0f);
          ((Entity) vehicle).MarkAsNoLongerNeeded();
          vehicle.set_DirtLevel(14f);
          ZombieVehicleSpawner.SmashRandomWindow(vehicle);
          if (Database.Random.NextDouble() < 0.5)
            ZombieVehicleSpawner.SmashRandomWindow(vehicle);
          if (Database.Random.NextDouble() < 0.200000002980232)
            ZombieVehicleSpawner.OpenRandomDoor(vehicle);
          ((Entity) vehicle).set_Heading((float) Database.Random.Next(1, 360));
          this._vehicles.Add(vehicle);
        }
      }
    }

    private static void OpenRandomDoor(Vehicle veh)
    {
      VehicleDoor[] doors = veh.GetDoors();
      VehicleDoor vehicleDoor = (VehicleDoor) (int) doors[Database.Random.Next(doors.Length)];
      veh.OpenDoor(vehicleDoor, false, true);
    }

    private static void SmashRandomWindow(Vehicle veh)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ZombieVehicleSpawner.\u003C\u003Ec__DisplayClass33_0 cDisplayClass330 = new ZombieVehicleSpawner.\u003C\u003Ec__DisplayClass33_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass330.veh = veh;
      // ISSUE: method pointer
      VehicleWindow[] array = ((IEnumerable<VehicleWindow>) Enumerable.Where<VehicleWindow>((IEnumerable<M0>) Enum.GetValues(typeof (VehicleWindow)), (Func<M0, bool>) new Func<VehicleWindow, bool>((object) cDisplayClass330, __methodptr(\u003CSmashRandomWindow\u003Eb__0)))).ToArray<VehicleWindow>();
      VehicleWindow vehicleWindow = (VehicleWindow) (int) array[Database.Random.Next(array.Length)];
      // ISSUE: reference to a compiler-generated field
      cDisplayClass330.veh.SmashWindow(vehicleWindow);
    }

    public bool IsInvalidZone(Vector3 spawn)
    {
      return Array.Find<string>(this.InvalidZoneNames, (Predicate<string>) (z => z == World.GetZoneName(spawn))) != null;
    }

    private static bool Exists(Entity arg)
    {
      return Entity.op_Inequality(arg, (Entity) null) && arg.Exists();
    }

    private void ClearAll()
    {
      while (this._peds.Count > 0)
      {
        ((Entity) this._peds[0]).Delete();
        this._peds.RemoveAt(0);
      }
      while (this._vehicles.Count > 0)
      {
        ((Entity) this._vehicles[0]).Delete();
        this._vehicles.RemoveAt(0);
      }
    }

    public bool IsValidSpawn(Vector3 spawnPoint)
    {
      return this.SpawnBlocker.FindIndex((Predicate<Vector3>) (spawn => (double) spawn.VDist(spawnPoint) < 150.0)) <= -1;
    }
  }
}
