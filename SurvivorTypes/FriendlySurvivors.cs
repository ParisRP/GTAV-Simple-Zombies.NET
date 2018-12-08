using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using ZombiesMod.DataClasses;
using ZombiesMod.Extensions;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.SurvivorTypes
{
  public class FriendlySurvivors : Survivors
  {
    private readonly List<Ped> _peds = new List<Ped>();
    private readonly PedGroup _pedGroup = new PedGroup();

    public FriendlySurvivors()
    {
      FriendlySurvivors.Instance = this;
    }

    public static FriendlySurvivors Instance { get; private set; }

    public void RemovePed(Ped item)
    {
      if (!this._peds.Contains(item))
        return;
      this._peds.Remove(item);
      item.LeaveGroup();
      ((Entity) item).get_CurrentBlip()?.Remove();
      EntityEventWrapper.Dispose((Entity) item);
    }

    public override void Update()
    {
      if (this._peds.Count > 0)
        return;
      this.Complete();
    }

    public override void SpawnEntities()
    {
      int num = Database.Random.Next(3, 6);
      Vector3 spawnPoint = this.GetSpawnPoint();
      if (!this.IsValidSpawn(spawnPoint))
        return;
      for (int index = 0; index <= num; ++index)
      {
        Ped randomPed = World.CreateRandomPed(((Vector3) ref spawnPoint).Around(5f));
        if (!Entity.op_Equality((Entity) randomPed, (Entity) null))
        {
          Blip blip = ((Entity) randomPed).AddBlip();
          blip.set_Color((BlipColor) 3);
          blip.set_Name("Friendly");
          randomPed.set_RelationshipGroup(Relationships.FriendlyRelationship);
          randomPed.get_Task().FightAgainstHatedTargets(9000f);
          randomPed.SetAlertness(Alertness.FullyAlert);
          randomPed.SetCombatAttributes(CombatAttributes.AlwaysFight, true);
          randomPed.get_Weapons().Give((WeaponHash) (int) (uint) Database.WeaponHashes[Database.Random.Next(Database.WeaponHashes.Length)], 25, true, true);
          this._pedGroup.Add(randomPed, index == 0);
          this._pedGroup.set_FormationType((FormationType) 0);
          this._peds.Add(randomPed);
          EntityEventWrapper entityEventWrapper = new EntityEventWrapper((Entity) randomPed);
          entityEventWrapper.Died += new EntityEventWrapper.OnDeathEvent(this.EventWrapperOnDied);
          entityEventWrapper.Disposed += new EntityEventWrapper.OnWrapperDisposedEvent(this.EventWrapperOnDisposed);
        }
      }
      UI.Notify("~b~Friendly~s~ survivors nearby.", true);
    }

    private void EventWrapperOnDisposed(EntityEventWrapper sender, Entity entity)
    {
      if (!this._peds.Contains(entity as Ped))
        return;
      this._peds.Remove(entity as Ped);
    }

    private void EventWrapperOnDied(EntityEventWrapper sender, Entity entity)
    {
      this._peds.Remove(entity as Ped);
      entity.get_CurrentBlip()?.Remove();
      entity.MarkAsNoLongerNeeded();
      sender.Dispose();
    }

    public override void CleanUp()
    {
      this._peds.ForEach((Action<Ped>) (ped =>
      {
        ((Entity) ped).get_CurrentBlip()?.Remove();
        ((Entity) ped).MarkAsNoLongerNeeded();
        EntityEventWrapper.Dispose((Entity) ped);
      }));
    }

    public override void Abort()
    {
      while (this._peds.Count > 0)
      {
        ((Entity) this._peds[0]).Delete();
        this._peds.RemoveAt(0);
      }
    }
  }
}
