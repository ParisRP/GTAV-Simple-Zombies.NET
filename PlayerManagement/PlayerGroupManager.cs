using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.PlayerManagement
{
  public class PlayerGroupManager : Script
  {
    private readonly UIMenu _pedMenu;
    private Ped _selectedPed;
    private PedCollection _peds;
    private readonly Dictionary<Ped, PedTask> _pedTasks;

    public PlayerGroupManager()
    {
      base.\u002Ector();
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerGroupManager.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new PlayerGroupManager.\u003C\u003Ec__DisplayClass4_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass40.\u003C\u003E4__this = this;
      PlayerGroupManager.Instance = this;
      this._pedMenu = new UIMenu("Guard", "SELECT AN OPTION");
      MenuConrtoller.MenuPool.Add(this._pedMenu);
      // ISSUE: method pointer
      this._pedMenu.add_OnMenuClose(new MenuCloseEvent((object) this, __methodptr(\u003C\u002Ector\u003Eb__4_0)));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass40.tasksItem = new UIMenuListItem("Tasks", Enum.GetNames(typeof (PedTask)).Cast<object>().ToList<object>(), 0, "Give peds a specific task to perform.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UIMenuItem) cDisplayClass40.tasksItem).add_Activated(new ItemActivatedEvent((object) cDisplayClass40, __methodptr(\u003C\u002Ector\u003Eb__1)));
      UIMenuItem uiMenuItem1 = new UIMenuItem("Apply To Nearby", "Apply the selected task to nearby peds within 50 meters.");
      // ISSUE: method pointer
      uiMenuItem1.add_Activated(new ItemActivatedEvent((object) cDisplayClass40, __methodptr(\u003C\u002Ector\u003Eb__2)));
      UIMenuItem uiMenuItem2 = new UIMenuItem("Give Weapon", "Give this ped your current weapon.");
      // ISSUE: method pointer
      uiMenuItem2.add_Activated(new ItemActivatedEvent((object) this, __methodptr(\u003C\u002Ector\u003Eb__4_5)));
      UIMenuItem uiMenuItem3 = new UIMenuItem("Take Weapon", "Take the ped's current weapon.");
      // ISSUE: method pointer
      uiMenuItem3.add_Activated(new ItemActivatedEvent((object) this, __methodptr(\u003C\u002Ector\u003Eb__4_6)));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass40.globalTasks = new UIMenuListItem("Guard Tasks", Enum.GetNames(typeof (PedTask)).Cast<object>().ToList<object>(), 0, "Give all gurads a specific task to perform.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UIMenuItem) cDisplayClass40.globalTasks).add_Activated(new ItemActivatedEvent((object) cDisplayClass40, __methodptr(\u003C\u002Ector\u003Eb__7)));
      // ISSUE: reference to a compiler-generated field
      this._pedMenu.AddItem((UIMenuItem) cDisplayClass40.tasksItem);
      this._pedMenu.AddItem(uiMenuItem1);
      this._pedMenu.AddItem(uiMenuItem2);
      this._pedMenu.AddItem(uiMenuItem3);
      // ISSUE: reference to a compiler-generated field
      ModController.Instance.MainMenu.AddItem((UIMenuItem) cDisplayClass40.globalTasks);
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted(new EventHandler(this.OnAborted));
    }

    public Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    public Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    public static PlayerGroupManager Instance { get; private set; }

    private void SetTask(Ped ped, PedTask task)
    {
      if (task == ~PedTask.StandStill || ped.get_IsPlayer())
        return;
      if (!this._pedTasks.ContainsKey(ped))
        this._pedTasks.Add(ped, task);
      else
        this._pedTasks[ped] = task;
      ped.get_Task().ClearAll();
      switch (task)
      {
        case PedTask.StandStill:
          ped.get_Task().StandStill(-1);
          break;
        case PedTask.Guard:
          ped.get_Task().GuardCurrentPosition();
          break;
        case PedTask.VehicleFollow:
          Vehicle closestVehicle = World.GetClosestVehicle(((Entity) ped).get_Position(), 100f);
          if (Entity.op_Equality((Entity) closestVehicle, (Entity) null))
          {
            UI.Notify("There's no vehicle near this ped.", true);
            return;
          }
          Function.Call((Hash) -264486839058504778L, new InputArgument[6]
          {
            InputArgument.op_Implicit(((Entity) ped).get_Handle()),
            InputArgument.op_Implicit(((Entity) closestVehicle).get_Handle()),
            InputArgument.op_Implicit(((Entity) this.PlayerPed).get_Handle()),
            InputArgument.op_Implicit(1074528293),
            InputArgument.op_Implicit(262144),
            InputArgument.op_Implicit(15)
          });
          break;
        case PedTask.Combat:
          ped.get_Task().FightAgainstHatedTargets(100f);
          break;
        case PedTask.Chill:
          Function.Call((Hash) 2846071673660833803L, new InputArgument[6]
          {
            InputArgument.op_Implicit(((Entity) ped).get_Handle()),
            InputArgument.op_Implicit((float) ((Entity) ped).get_Position().X),
            InputArgument.op_Implicit((float) ((Entity) ped).get_Position().Y),
            InputArgument.op_Implicit((float) ((Entity) ped).get_Position().Z),
            InputArgument.op_Implicit(100f),
            InputArgument.op_Implicit(-1)
          });
          break;
        case PedTask.Leave:
          ped.LeaveGroup();
          ((Entity) ped).get_CurrentBlip()?.Remove();
          ((Entity) ped).MarkAsNoLongerNeeded();
          EntityEventWrapper.Dispose((Entity) ped);
          break;
      }
      ped.set_BlockPermanentEvents(task == PedTask.Follow);
    }

    private void OnAborted(object sender, EventArgs eventArgs)
    {
      PedGroup currentPedGroup = this.PlayerPed.get_CurrentPedGroup();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      List<Ped> list = ((IEnumerable<Ped>) Enumerable.Where<Ped>((IEnumerable<M0>) currentPedGroup, (Func<M0, bool>) (PlayerGroupManager.\u003C\u003Ec.\u003C\u003E9__14_0 ?? (PlayerGroupManager.\u003C\u003Ec.\u003C\u003E9__14_0 = new Func<Ped, bool>((object) PlayerGroupManager.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003COnAborted\u003Eb__14_0)))))).ToList<Ped>();
      currentPedGroup.Dispose();
      while (list.Count > 0)
      {
        ((Entity) list[0]).Delete();
        list.RemoveAt(0);
      }
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      if (this.PlayerPed.IsInVehicle() || MenuConrtoller.MenuPool.IsAnyMenuOpen() || this.PlayerPed.get_CurrentPedGroup().get_MemberCount() <= 0)
        return;
      Ped closest = (Ped) World.GetClosest<Ped>(this.PlayerPosition, (M0[]) World.GetNearbyPeds(this.PlayerPed, 1.5f));
      if (Entity.op_Equality((Entity) closest, (Entity) null) || closest.IsInVehicle() || PedGroup.op_Inequality(closest.get_CurrentPedGroup(), this.PlayerPed.get_CurrentPedGroup()))
        return;
      Game.DisableControlThisFrame(2, (Control) 51);
      UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to configure this ped.", false);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 51))
        return;
      this._selectedPed = closest;
      this._pedMenu.set_Visible(!this._pedMenu.get_Visible());
    }

    public void Deserialize()
    {
      if (this._peds != null)
        return;
      this._peds = Serializer.Deserialize<PedCollection>("./scripts/Guards.dat") ?? new PedCollection();
      this._peds.ListChanged += (PedCollection.ListChangedEvent) (count => Serializer.Serialize<PedCollection>("./scripts/Guards.dat", this._peds));
      this._peds.ToList<PedData>().ForEach((Action<PedData>) (data =>
      {
        Ped ped = World.CreatePed(Model.op_Implicit(data.Hash), data.Position);
        if (Entity.op_Equality((Entity) ped, (Entity) null))
          return;
        ((Entity) ped).set_Rotation(data.Rotation);
        ped.Recruit(this.PlayerPed);
        data.Weapons.ForEach((Action<Weapon>) (w => ped.get_Weapons().Give(w.Hash, w.Ammo, true, true)));
        data.Handle = ((Entity) ped).get_Handle();
        this.SetTask(ped, data.Task);
      }));
    }

    public void SavePeds()
    {
      if (this._peds == null)
        this.Deserialize();
      List<Ped> list = this.PlayerPed.get_CurrentPedGroup().ToList(false);
      if (list.Count <= 0)
      {
        UI.Notify("You have no bodyguards.");
      }
      else
      {
        List<PedData> pedDatas = this._peds.ToList<PedData>();
        list.ConvertAll<PedData>((Converter<Ped, PedData>) (ped =>
        {
          PedData data = pedDatas.Find((Predicate<PedData>) (pedData => pedData.Handle == ((Entity) ped).get_Handle()));
          return this.UpdatePedData(ped, data);
        })).ToList<PedData>().ForEach((Action<PedData>) (data =>
        {
          if (this._peds.Contains(data))
            return;
          this._peds.Add(data);
        }));
        Serializer.Serialize<PedCollection>("./scripts/Guards.dat", this._peds);
        UI.Notify("~b~Guards~s~ saved!");
      }
    }

    private PedData UpdatePedData(Ped ped, PedData data)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerGroupManager.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180 = new PlayerGroupManager.\u003C\u003Ec__DisplayClass18_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.ped = ped;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PedTask pedTask = this._pedTasks.ContainsKey(cDisplayClass180.ped) ? this._pedTasks[cDisplayClass180.ped] : ~PedTask.StandStill;
      // ISSUE: method pointer
      IEnumerable<WeaponHash> source = (IEnumerable<WeaponHash>) Enumerable.Where<WeaponHash>((IEnumerable<M0>) Enum.GetValues(typeof (WeaponHash)), (Func<M0, bool>) new Func<WeaponHash, bool>((object) cDisplayClass180, __methodptr(\u003CUpdatePedData\u003Eb__0)));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.componentHashes = (WeaponComponent[]) Enum.GetValues(typeof (WeaponComponent));
      // ISSUE: reference to a compiler-generated method
      List<Weapon> list = source.ToList<WeaponHash>().ConvertAll<Weapon>(new Converter<WeaponHash, Weapon>(cDisplayClass180.\u003CUpdatePedData\u003Eb__1)).ToList<Weapon>();
      bool flag = data == null;
      if (flag)
      {
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          int handle = ((Entity) cDisplayClass180.ped).get_Handle();
          // ISSUE: reference to a compiler-generated field
          Model model = ((Entity) cDisplayClass180.ped).get_Model();
          int hash = ((Model) ref model).get_Hash();
          // ISSUE: reference to a compiler-generated field
          Vector3 rotation = ((Entity) cDisplayClass180.ped).get_Rotation();
          // ISSUE: reference to a compiler-generated field
          Vector3 position = ((Entity) cDisplayClass180.ped).get_Position();
          int num = (int) pedTask;
          List<Weapon> weapons = list;
          data = new PedData(handle, hash, rotation, position, (PedTask) num, weapons);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        data.Position = ((Entity) cDisplayClass180.ped).get_Position();
        // ISSUE: reference to a compiler-generated field
        data.Rotation = ((Entity) cDisplayClass180.ped).get_Rotation();
        data.Task = pedTask;
        data.Weapons = list;
      }
      return data;
    }

    private static void TradeWeapons(Ped trader, Ped reviever)
    {
      if (trader.get_Weapons().get_Current() == trader.get_Weapons().get_Item((WeaponHash) -1569615261))
        return;
      Weapon current = trader.get_Weapons().get_Current();
      if (reviever.get_Weapons().HasWeapon(current.get_Hash()))
        return;
      if (!reviever.get_IsPlayer())
        reviever.get_Weapons().Drop();
      Weapon weapon = reviever.get_Weapons().Give(current.get_Hash(), 0, true, true);
      weapon.set_Ammo(current.get_Ammo());
      weapon.set_InfiniteAmmo(false);
      trader.get_Weapons().Remove(current);
    }
  }
}
