using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZombiesMod.DataClasses;
using ZombiesMod.Extensions;
using ZombiesMod.PlayerManagement;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.SurvivorTypes
{
  public class MerryweatherSurvivors : Survivors
  {
    public const float InteractDistance = 2.3f;
    private const int BlipRadius = 145;
    private readonly int _timeOut;
    private ParticleEffect _particle;
    private readonly PedGroup _pedGroup;
    private readonly List<Ped> _peds;
    private Blip _blip;
    private Prop _prop;
    private MerryweatherSurvivors.DropType _dropType;
    private bool _notify;
    private Vector3 _dropZone;
    private readonly BarTimerBar _timerBar;
    private float _currentTime;
    private readonly PedHash[] _pedHashes;
    private readonly WeaponHash[] _weapons;

    public MerryweatherSurvivors(int timeout)
    {
      // ISSUE: unable to decompile the method.
    }

    public override void Update()
    {
      if (Entity.op_Equality((Entity) this._prop, (Entity) null))
        return;
      this.TryInteract((Entity) this._prop);
      this.UpdateTimer();
      if (this.CantSeeCrate())
        return;
      Blip blip1 = ((Entity) this._prop).AddBlip();
      if (Blip.op_Equality(blip1, (Blip) null))
        return;
      blip1.set_Sprite((BlipSprite) 306);
      blip1.set_Color((BlipColor) 66);
      blip1.set_Name("Crate Drop");
      this._blip.Remove();
      this._peds.ForEach((Action<Ped>) (ped =>
      {
        Blip blip2 = ((Entity) ped).AddBlip();
        blip2.set_Color((BlipColor) 66);
        blip2.set_Name("Merryweather Security");
      }));
    }

    private bool CantSeeCrate()
    {
      return !((Entity) this._prop).get_IsOnScreen() || ((Entity) this._prop).get_IsOccluded() || ((Entity) this._prop).get_CurrentBlip().Exists() || (double) ((Entity) this._prop).get_Position().VDist(this.PlayerPosition) > 50.0;
    }

    private void UpdateTimer()
    {
      if ((double) this.PlayerPosition.VDist(this._dropZone) < 145.0)
      {
        if (!this._notify)
        {
          BigMessageThread.get_MessageInstance().ShowMissionPassedMessage("~r~Entering Hostile Zone", 5000);
          this._notify = true;
        }
        if (!MenuConrtoller.BarPool.ToList().Contains((TimerBarBase) this._timerBar))
          return;
        MenuConrtoller.BarPool.Remove((TimerBarBase) this._timerBar);
      }
      else
      {
        if (!MenuConrtoller.BarPool.ToList().Contains((TimerBarBase) this._timerBar))
          MenuConrtoller.BarPool.Add((TimerBarBase) this._timerBar);
        this._timerBar.set_Percentage(this._currentTime / (float) this._timeOut);
        this._currentTime -= Game.get_LastFrameTime();
        if ((double) this._currentTime > 0.0)
          return;
        this.Complete();
        UI.Notify("~r~Failed~s~ to retrieve crate.");
      }
    }

    public override void SpawnEntities()
    {
      Vector3 spawnPoint = this.GetSpawnPoint();
      if (!this.IsValidSpawn(spawnPoint))
        return;
      MerryweatherSurvivors.DropType[] values = (MerryweatherSurvivors.DropType[]) Enum.GetValues(typeof (MerryweatherSurvivors.DropType));
      this._dropType = values[Database.Random.Next(values.Length)];
      this._prop = World.CreateProp(Model.op_Implicit(this._dropType == MerryweatherSurvivors.DropType.Weapons ? "prop_mil_crate_01" : "ex_prop_crate_closed_bc"), spawnPoint, Vector3.get_Zero(), false, false);
      if (Entity.op_Equality((Entity) this._prop, (Entity) null))
      {
        this.Complete();
      }
      else
      {
        Vector3 position1 = ((Entity) this._prop).get_Position();
        this._blip = World.CreateBlip(((Vector3) ref position1).Around(45f), 145f);
        this._blip.set_Color((BlipColor) 66);
        this._blip.set_Alpha(150);
        this._dropZone = this._blip.get_Position();
        Vector3 position2 = ((Entity) this._prop).get_Position();
        this._particle = WorldExtended.CreateParticleEffectAtCoord(((Vector3) ref position2).Around(5f), "exp_grd_flare");
        this._particle.Color = Color.LightGoldenrodYellow;
        int num = Database.Random.Next(3, 6);
        for (int index = 0; index <= num; ++index)
        {
          Vector3 vector3 = ((Vector3) ref spawnPoint).Around(10f);
          Ped ped = World.CreatePed(Model.op_Implicit((PedHash) (int) (uint) this._pedHashes[Database.Random.Next(this._pedHashes.Length)]), vector3);
          if (!Entity.op_Equality((Entity) ped, (Entity) null))
          {
            if (index > 0)
              ped.get_Weapons().Give((WeaponHash) (int) (uint) this._weapons[Database.Random.Next(this._weapons.Length)], 45, true, true);
            else
              ped.get_Weapons().Give((WeaponHash) 100416529, 15, true, true);
            ped.set_Accuracy(100);
            ped.get_Task().GuardCurrentPosition();
            ped.set_RelationshipGroup(Relationships.MilitiaRelationship);
            this._pedGroup.Add(ped, index == 0);
            this._peds.Add(ped);
            new EntityEventWrapper((Entity) ped).Died += new EntityEventWrapper.OnDeathEvent(this.PedWrapperOnDied);
          }
        }
        Model model = Model.op_Implicit("mesa3");
        Vector3 position3 = ((Entity) this._prop).get_Position();
        Vector3 positionOnStreet = World.GetNextPositionOnStreet(((Vector3) ref position3).Around(25f));
        World.CreateVehicle(model, positionOnStreet);
        UI.Notify(string.Format("~y~Merryweather~s~ {0} drop nearby.", this._dropType == MerryweatherSurvivors.DropType.Loot ? (object) "loot" : (object) "weapons"));
      }
    }

    private void PedWrapperOnDied(EntityEventWrapper sender, Entity entity)
    {
      this._peds.Remove(entity as Ped);
      entity.MarkAsNoLongerNeeded();
      entity.get_CurrentBlip()?.Remove();
      sender.Dispose();
    }

    private void TryInteract(Entity prop)
    {
      if (Entity.op_Equality(prop, (Entity) null) || (double) prop.get_Position().VDist(this.PlayerPosition) >= 2.29999995231628)
        return;
      UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to loot.", false);
      Game.DisableControlThisFrame(2, (Control) 51);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 51))
        return;
      prop.Delete();
      switch (this._dropType)
      {
        case MerryweatherSurvivors.DropType.Weapons:
          int num1 = Database.Random.Next(3, 5);
          int num2 = 0;
          for (int index = 0; index <= num1; ++index)
          {
            // ISSUE: method pointer
            WeaponHash[] array = ((IEnumerable<WeaponHash>) Enumerable.Where<WeaponHash>((IEnumerable<M0>) Enum.GetValues(typeof (WeaponHash)), (Func<M0, bool>) new Func<WeaponHash, bool>((object) this, __methodptr(IsGoodHash)))).ToArray<WeaponHash>();
            if (array.Length != 0)
            {
              this.PlayerPed.get_Weapons().Give((WeaponHash) (int) (uint) array[Database.Random.Next(array.Length)], Database.Random.Next(20, 45), false, true);
              ++num2;
            }
          }
          UI.Notify(string.Format("Found ~g~{0}~s~ weapons.", (object) num2));
          break;
        case MerryweatherSurvivors.DropType.Loot:
          int num3 = Database.Random.Next(1, 3);
          PlayerInventory.Instance.PickupLoot((Ped) null, ItemType.Item, num3, num3, 0.4f);
          break;
      }
      this.Complete();
    }

    public override void CleanUp()
    {
      this._particle.Delete();
      this._peds?.ForEach((Action<Ped>) (ped =>
      {
        ((Entity) ped).get_CurrentBlip()?.Remove();
        ped.set_AlwaysKeepTask(true);
        ((Entity) ped).set_IsPersistent(false);
      }));
      this._blip?.Remove();
      if (!MenuConrtoller.BarPool.ToList().Contains((TimerBarBase) this._timerBar))
        return;
      MenuConrtoller.BarPool.Remove((TimerBarBase) this._timerBar);
    }

    public override void Abort()
    {
      this._particle?.Delete();
      ((Entity) this._prop)?.Delete();
      this._peds?.ForEach((Action<Ped>) (ped =>
      {
        ((Entity) ped).get_CurrentBlip()?.Remove();
        ((Entity) ped).Delete();
      }));
      this._blip?.Remove();
      if (!MenuConrtoller.BarPool.ToList().Contains((TimerBarBase) this._timerBar))
        return;
      MenuConrtoller.BarPool.Remove((TimerBarBase) this._timerBar);
    }

    private bool IsGoodHash(WeaponHash hash)
    {
      return hash != -1569615261 && hash != -1600701090 && (hash != 600439132 && hash != 126349499) && !this.PlayerPed.get_Weapons().HasWeapon(hash);
    }

    private enum DropType
    {
      Weapons,
      Loot,
    }
  }
}
