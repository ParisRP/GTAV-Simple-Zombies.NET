using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.PlayerManagement
{
  public class PlayerVehicles : Script
  {
    private VehicleCollection _vehicleCollection;
    private readonly List<Vehicle> _vehicles;

    public PlayerVehicles()
    {
      base.\u002Ector();
      PlayerVehicles.Instance = this;
      this.add_Aborted(new EventHandler(this.OnAborted));
    }

    private void OnAborted(object sender, EventArgs eventArgs)
    {
      this._vehicleCollection.ToList<VehicleData>().ForEach((Action<VehicleData>) (vehicle => vehicle.Delete()));
    }

    public static PlayerVehicles Instance { get; private set; }

    public void Deserialize()
    {
      if (this._vehicleCollection != null)
        return;
      this._vehicleCollection = Serializer.Deserialize<VehicleCollection>("./scripts/Vehicles.dat") ?? new VehicleCollection();
      this._vehicleCollection.ListChanged += (VehicleCollection.ListChangedEvent) (sender => this.Serialize(false));
      foreach (VehicleData vehicle1 in this._vehicleCollection)
      {
        Vehicle vehicle2 = World.CreateVehicle(Model.op_Implicit(vehicle1.Hash), vehicle1.Position);
        if (Entity.op_Equality((Entity) vehicle2, (Entity) null))
        {
          UI.Notify("Failed to load vehicle.");
          break;
        }
        vehicle2.set_PrimaryColor(vehicle1.PrimaryColor);
        vehicle2.set_SecondaryColor(vehicle1.SecondaryColor);
        ((Entity) vehicle2).set_Health(vehicle1.Health);
        vehicle2.set_EngineHealth(vehicle1.EngineHealth);
        ((Entity) vehicle2).set_Rotation(vehicle1.Rotation);
        vehicle1.Handle = ((Entity) vehicle2).get_Handle();
        PlayerVehicles.AddKit(vehicle2, vehicle1);
        PlayerVehicles.AddBlipToVehicle(vehicle2);
        this._vehicles.Add(vehicle2);
        ((Entity) vehicle2).set_IsPersistent(true);
        new EntityEventWrapper((Entity) vehicle2).Died += new EntityEventWrapper.OnDeathEvent(this.WrapperOnDied);
      }
    }

    private static void AddKit(Vehicle vehicle, VehicleData data)
    {
      if (data == null || Entity.op_Equality((Entity) vehicle, (Entity) null))
        return;
      vehicle.InstallModKit();
      VehicleNeonLight[] neonLights = data.NeonLights;
      if (neonLights != null)
        ((IEnumerable<VehicleNeonLight>) neonLights).ToList<VehicleNeonLight>().ForEach((Action<VehicleNeonLight>) (h => vehicle.SetNeonLightsOn(h, true)));
      data.Mods?.ForEach((Action<Tuple<VehicleMod, int>>) (m => vehicle.SetMod(m.get_Item1(), m.get_Item2(), true)));
      VehicleToggleMod[] toggleMods = data.ToggleMods;
      if (toggleMods != null)
        ((IEnumerable<VehicleToggleMod>) toggleMods).ToList<VehicleToggleMod>().ForEach((Action<VehicleToggleMod>) (h => vehicle.ToggleMod(h, true)));
      vehicle.set_WindowTint(data.WindowTint);
      vehicle.set_WheelType(data.WheelType);
      vehicle.set_NeonLightsColor(data.NeonColor);
      Function.Call((Hash) 6971396915951620534L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) vehicle).get_Handle()),
        InputArgument.op_Implicit(data.Livery)
      });
    }

    public void Serialize(bool notify = false)
    {
      if (this._vehicleCollection == null)
        return;
      this.UpdateVehicleData();
      Serializer.Serialize<VehicleCollection>("./scripts/Vehicles.dat", this._vehicleCollection);
      if (!notify)
        return;
      UI.Notify(this._vehicleCollection.Count <= 0 ? "No vehicles." : "~p~Vehicles~s~ saved!");
    }

    private void UpdateVehicleData()
    {
      if (this._vehicleCollection.Count <= 0)
        return;
      this._vehicleCollection.ToList<VehicleData>().ForEach((Action<VehicleData>) (v =>
      {
        Vehicle vehicle = this._vehicles.Find((Predicate<Vehicle>) (i => ((Entity) i).get_Handle() == v.Handle));
        if (Entity.op_Equality((Entity) vehicle, (Entity) null))
          return;
        PlayerVehicles.UpdateDataSpecific(v, vehicle);
      }));
    }

    private static void UpdateDataSpecific(VehicleData vehicleData, Vehicle vehicle)
    {
      vehicleData.Position = ((Entity) vehicle).get_Position();
      vehicleData.Rotation = ((Entity) vehicle).get_Rotation();
      vehicleData.Health = ((Entity) vehicle).get_Health();
      vehicleData.EngineHealth = vehicle.get_EngineHealth();
      vehicleData.PrimaryColor = vehicle.get_PrimaryColor();
      vehicleData.SecondaryColor = vehicle.get_SecondaryColor();
    }

    public void SaveVehicle(Vehicle vehicle)
    {
      if (this._vehicleCollection == null)
        this.Deserialize();
      VehicleData vehicleData = this._vehicleCollection.ToList<VehicleData>().Find((Predicate<VehicleData>) (v => v.Handle == ((Entity) vehicle).get_Handle()));
      if (vehicleData != null)
      {
        PlayerVehicles.UpdateDataSpecific(vehicleData, vehicle);
        this.Serialize(true);
      }
      else
      {
        // ISSUE: method pointer
        VehicleNeonLight[] array1 = ((IEnumerable<VehicleNeonLight>) Enumerable.Where<VehicleNeonLight>((IEnumerable<M0>) Enum.GetValues(typeof (VehicleNeonLight)), (Func<M0, bool>) new Func<VehicleNeonLight, bool>((object) vehicle, __methodptr(IsNeonLightsOn)))).ToArray<VehicleNeonLight>();
        VehicleMod[] values = (VehicleMod[]) Enum.GetValues(typeof (VehicleMod));
        List<Tuple<VehicleMod, int>> mods = new List<Tuple<VehicleMod, int>>();
        ((IEnumerable<VehicleMod>) values).ToList<VehicleMod>().ForEach((Action<VehicleMod>) (h =>
        {
          int mod = vehicle.GetMod(h);
          if (mod == -1)
            return;
          mods.Add(new Tuple<VehicleMod, int>(h, mod));
        }));
        // ISSUE: method pointer
        VehicleToggleMod[] array2 = ((IEnumerable<VehicleToggleMod>) Enumerable.Where<VehicleToggleMod>((IEnumerable<M0>) Enum.GetValues(typeof (VehicleToggleMod)), (Func<M0, bool>) new Func<VehicleToggleMod, bool>((object) vehicle, __methodptr(IsToggleModOn)))).ToArray<VehicleToggleMod>();
        int handle = ((Entity) vehicle).get_Handle();
        Model model = ((Entity) vehicle).get_Model();
        int hash = ((Model) ref model).get_Hash();
        Vector3 rotation = ((Entity) vehicle).get_Rotation();
        Vector3 position = ((Entity) vehicle).get_Position();
        VehicleColor primaryColor = vehicle.get_PrimaryColor();
        VehicleColor secondaryColor = vehicle.get_SecondaryColor();
        int health = ((Entity) vehicle).get_Health();
        double engineHealth = (double) vehicle.get_EngineHealth();
        double heading = (double) ((Entity) vehicle).get_Heading();
        VehicleNeonLight[] neonLights = array1;
        List<Tuple<VehicleMod, int>> mods1 = mods;
        VehicleToggleMod[] toggleMods = array2;
        VehicleWindowTint windowTint = vehicle.get_WindowTint();
        VehicleWheelType wheelType = vehicle.get_WheelType();
        Color neonLightsColor = vehicle.get_NeonLightsColor();
        M0 m0_1 = Function.Call<int>((Hash) 3150587921134411402L, new InputArgument[1]
        {
          InputArgument.op_Implicit(((Entity) vehicle).get_Handle())
        });
        M0 m0_2 = Function.Call<bool>((Hash) -5507252750051666468L, new InputArgument[2]
        {
          InputArgument.op_Implicit(((Entity) vehicle).get_Handle()),
          InputArgument.op_Implicit(23)
        });
        M0 m0_3 = Function.Call<bool>((Hash) -5507252750051666468L, new InputArgument[2]
        {
          InputArgument.op_Implicit(((Entity) vehicle).get_Handle()),
          InputArgument.op_Implicit(24)
        });
        this._vehicleCollection.Add(new VehicleData(handle, hash, rotation, position, primaryColor, secondaryColor, health, (float) engineHealth, (float) heading, neonLights, mods1, toggleMods, windowTint, wheelType, neonLightsColor, (int) m0_1, (bool) m0_2, (bool) m0_3));
        this._vehicles.Add(vehicle);
        ((Entity) vehicle).set_IsPersistent(true);
        new EntityEventWrapper((Entity) vehicle).Died += new EntityEventWrapper.OnDeathEvent(this.WrapperOnDied);
        PlayerVehicles.AddBlipToVehicle(vehicle);
      }
    }

    private static void AddBlipToVehicle(Vehicle vehicle)
    {
      Blip blip = ((Entity) vehicle).AddBlip();
      blip.set_Sprite(PlayerVehicles.GetSprite(vehicle));
      blip.set_Color((BlipColor) 58);
      blip.set_Name(vehicle.get_FriendlyName());
      blip.set_Scale(0.85f);
    }

    private static BlipSprite GetSprite(Vehicle vehicle)
    {
      return vehicle.get_ClassType() == 8 ? (BlipSprite) 226 : (vehicle.get_ClassType() == 14 ? (BlipSprite) 410 : (vehicle.get_ClassType() == 15 ? (BlipSprite) 64 : (vehicle.get_ClassType() == 16 ? (BlipSprite) 423 : (BlipSprite) 225)));
    }

    private void WrapperOnDied(EntityEventWrapper sender, Entity entity)
    {
      UI.Notify("Your vehicle was ~r~destroyed~s~!");
      this._vehicleCollection.Remove(this._vehicleCollection.ToList<VehicleData>().Find((Predicate<VehicleData>) (v => v.Handle == entity.get_Handle())));
      entity.get_CurrentBlip()?.Remove();
      sender.Dispose();
    }
  }
}
