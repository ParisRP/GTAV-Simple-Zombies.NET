using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using ZombiesMod.DataClasses;
using ZombiesMod.Extensions;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.SurvivorTypes
{
  public class HostileSurvivors : Survivors
  {
    private readonly PedGroup _group = new PedGroup();
    private readonly List<Ped> _peds = new List<Ped>();
    private Vehicle _vehicle;

    public override void Update()
    {
      if (this._peds.Count > 0)
        return;
      this.Complete();
    }

    public override void SpawnEntities()
    {
      Vector3 spawnPoint = this.GetSpawnPoint();
      if (!this.IsValidSpawn(spawnPoint))
        return;
      Vehicle vehicle = World.CreateVehicle(Database.GetRandomVehicleModel(), spawnPoint, (float) Database.Random.Next(1, 360));
      if (Entity.op_Equality((Entity) vehicle, (Entity) null))
      {
        this.Complete();
      }
      else
      {
        this._vehicle = vehicle;
        Blip blip = ((Entity) this._vehicle).AddBlip();
        blip.set_Name("Enemy Vehicle");
        blip.set_Sprite((BlipSprite) 225);
        blip.set_Color((BlipColor) 1);
        EntityEventWrapper entityEventWrapper1 = new EntityEventWrapper((Entity) this._vehicle);
        entityEventWrapper1.Died += new EntityEventWrapper.OnDeathEvent(this.VehicleWrapperOnDied);
        entityEventWrapper1.Updated += new EntityEventWrapper.OnWrapperUpdateEvent(this.VehicleWrapperOnUpdated);
        for (int index = 0; index < vehicle.get_PassengerSeats() + 1; ++index)
        {
          if (this._group.get_MemberCount() < 6 && vehicle.IsSeatFree((VehicleSeat) -2))
          {
            Ped randomPedOnSeat = vehicle.CreateRandomPedOnSeat(index == 0 ? (VehicleSeat) -1 : (VehicleSeat) -2);
            if (!Entity.op_Equality((Entity) randomPedOnSeat, (Entity) null))
            {
              randomPedOnSeat.get_Weapons().Give((WeaponHash) (int) (uint) Database.WeaponHashes[Database.Random.Next(Database.WeaponHashes.Length)], 25, true, true);
              randomPedOnSeat.SetCombatAttributes(CombatAttributes.AlwaysFight, true);
              randomPedOnSeat.SetAlertness(Alertness.FullyAlert);
              randomPedOnSeat.set_RelationshipGroup(Relationships.HostileRelationship);
              this._group.Add(randomPedOnSeat, index == 0);
              ((Entity) randomPedOnSeat).AddBlip().set_Name("Enemy");
              this._peds.Add(randomPedOnSeat);
              EntityEventWrapper entityEventWrapper2 = new EntityEventWrapper((Entity) randomPedOnSeat);
              entityEventWrapper2.Died += new EntityEventWrapper.OnDeathEvent(this.PedWrapperOnDied);
              entityEventWrapper2.Updated += new EntityEventWrapper.OnWrapperUpdateEvent(this.PedWrapperOnUpdated);
              entityEventWrapper2.Disposed += new EntityEventWrapper.OnWrapperDisposedEvent(this.PedWrapperOnDisposed);
            }
          }
        }
        UI.Notify("~r~Hostiles~s~ nearby!");
      }
    }

    private void PedWrapperOnDisposed(EntityEventWrapper sender, Entity entity)
    {
      if (!this._peds.Contains(entity as Ped))
        return;
      this._peds.Remove(entity as Ped);
    }

    private void VehicleWrapperOnUpdated(EntityEventWrapper sender, Entity entity)
    {
      if (Entity.op_Equality(entity, (Entity) null))
        return;
      entity.get_CurrentBlip().set_Alpha(((Entity) this._vehicle.get_Driver()).Exists() ? (int) byte.MaxValue : 0);
    }

    private void VehicleWrapperOnDied(EntityEventWrapper sender, Entity entity)
    {
      entity.get_CurrentBlip()?.Remove();
      sender.Dispose();
      ((Entity) this._vehicle).MarkAsNoLongerNeeded();
      this._vehicle = (Vehicle) null;
    }

    private void PedWrapperOnUpdated(EntityEventWrapper sender, Entity entity)
    {
      Ped ped = entity as Ped;
      if (Entity.op_Equality((Entity) ped, (Entity) null))
        return;
      if (Entity.op_Equality((Entity) ped.get_CurrentVehicle()?.get_Driver(), (Entity) ped) && !ped.get_IsInCombat())
        ped.get_Task().DriveTo(ped.get_CurrentVehicle(), this.PlayerPosition, 25f, 75f);
      if ((double) ((Entity) ped).get_Position().VDist(this.PlayerPosition) > (double) Survivors.DeleteRange)
        ((Entity) ped).Delete();
      if (!((Entity) ped).get_CurrentBlip().Exists())
        return;
      ((Entity) ped).get_CurrentBlip().set_Alpha(ped.IsInVehicle() ? 0 : (int) byte.MaxValue);
    }

    private void PedWrapperOnDied(EntityEventWrapper sender, Entity entity)
    {
      entity.get_CurrentBlip()?.Remove();
      this._peds.Remove(entity as Ped);
    }

    public override void CleanUp()
    {
      ((Entity) this._vehicle)?.get_CurrentBlip()?.Remove();
      EntityEventWrapper.Dispose((Entity) this._vehicle);
    }

    public override void Abort()
    {
      ((Entity) this._vehicle)?.Delete();
      while (this._peds.Count > 0)
      {
        ((Entity) this._peds[0])?.Delete();
        this._peds.RemoveAt(0);
      }
    }
  }
}
