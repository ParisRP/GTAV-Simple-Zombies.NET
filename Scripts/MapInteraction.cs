using GTA;
using GTA.Math;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.PlayerManagement;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.Scripts
{
  public class MapInteraction : Script
  {
    private const int AmmoPerPart = 10;
    private readonly float _enemyRangeForSleeping;
    private readonly int _sleepHours;
    private readonly UIMenu _weaponsMenu;
    private readonly UIMenu _storageMenu;
    private readonly UIMenu _myWeaponsMenu;
    private readonly UIMenu _craftWeaponsMenu;
    private readonly Dictionary<WeaponGroup, int> _requiredAmountDictionary;

    public MapInteraction()
    {
      base.\u002Ector();
      PlayerMap.Interacted += new PlayerMap.InteractedEvent(this.MapOnInteracted);
      MenuConrtoller.MenuPool.Add(this._weaponsMenu);
      MenuConrtoller.MenuPool.Add(this._craftWeaponsMenu);
      this._storageMenu = MenuConrtoller.MenuPool.AddSubMenu(this._weaponsMenu, "Storage");
      this._myWeaponsMenu = MenuConrtoller.MenuPool.AddSubMenu(this._weaponsMenu, "Give");
      this._enemyRangeForSleeping = (float) this.get_Settings().GetValue<float>("map_interaction", "enemy_range_for_sleeping", (M0) (double) this._enemyRangeForSleeping);
      this._sleepHours = (int) this.get_Settings().GetValue<int>("map_interaction", "sleep_hours", (M0) this._sleepHours);
      this.get_Settings().SetValue<float>("map_interaction", "enemy_range_for_sleeping", (M0) (double) this._enemyRangeForSleeping);
      this.get_Settings().SetValue<int>("map_interaction", "sleep_hours", (M0) this._sleepHours);
      Dictionary<WeaponGroup, int> dictionary = new Dictionary<WeaponGroup, int>();
      dictionary.Add((WeaponGroup) -1212426201, 2);
      dictionary.Add((WeaponGroup) -1569042529, 5);
      dictionary.Add((WeaponGroup) 1159398588, 3);
      dictionary.Add((WeaponGroup) 1595662460, 1);
      this._requiredAmountDictionary = dictionary;
      this.add_Aborted(new EventHandler(MapInteraction.OnAborted));
    }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    private static Player Player
    {
      get
      {
        return Database.Player;
      }
    }

    private static void OnAborted(object sender, EventArgs eventArgs)
    {
      ((Entity) MapInteraction.PlayerPed).set_IsVisible(true);
      ((Entity) MapInteraction.PlayerPed).set_FreezePosition(false);
      MapInteraction.Player.set_CanControlCharacter(true);
      if (((Entity) MapInteraction.PlayerPed).get_IsDead())
        return;
      Game.FadeScreenIn(0);
    }

    private void MapOnInteracted(MapProp mapProp, InventoryItemBase inventoryItem)
    {
      BuildableInventoryItem buildableInventoryItem = inventoryItem as BuildableInventoryItem;
      if (buildableInventoryItem == null)
        return;
      string id = buildableInventoryItem.Id;
      if (!(id == "Tent"))
      {
        if (!(id == "Weapons Crate"))
        {
          if (id == "Work Bench")
            this.CraftAmmo();
        }
        else
          this.UseWeaponsCrate(mapProp);
      }
      else
        this.Sleep(mapProp.Position);
      if (!buildableInventoryItem.IsDoor)
        return;
      Prop prop = new Prop(mapProp.Handle);
      prop.SetStateOfDoor(!prop.GetDoorLockState(), DoorState.Closed);
    }

    private void CraftAmmo()
    {
      this._craftWeaponsMenu.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      List<WeaponGroup> list = ((IEnumerable<WeaponGroup>) ((IEnumerable<WeaponGroup>) Enumerable.Where<WeaponGroup>((IEnumerable<M0>) Enum.GetValues(typeof (WeaponGroup)), (Func<M0, bool>) (MapInteraction.\u003C\u003Ec.\u003C\u003E9__15_0 ?? (MapInteraction.\u003C\u003Ec.\u003C\u003E9__15_0 = new Func<WeaponGroup, bool>((object) MapInteraction.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CCraftAmmo\u003Eb__15_0)))))).ToArray<WeaponGroup>()).ToList<WeaponGroup>();
      list.Add((WeaponGroup) 970310034);
      foreach (uint num in list.ToArray())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MapInteraction.\u003C\u003Ec__DisplayClass15_1 cDisplayClass151 = new MapInteraction.\u003C\u003Ec__DisplayClass15_1();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass151.weaponGroup = (WeaponGroup) (int) num;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MapInteraction.\u003C\u003Ec__DisplayClass15_0 cDisplayClass150 = new MapInteraction.\u003C\u003Ec__DisplayClass15_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass150.CS\u0024\u003C\u003E8__locals1 = cDisplayClass151;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UIMenuItem uiMenuItem = new UIMenuItem(string.Format("{0}", cDisplayClass150.CS\u0024\u003C\u003E8__locals1.weaponGroup == 970310034 ? (object) "Assult Rifle" : (object) cDisplayClass150.CS\u0024\u003C\u003E8__locals1.weaponGroup.ToString()), string.Format("Craft ammo for {0}", (object) cDisplayClass150.CS\u0024\u003C\u003E8__locals1.weaponGroup));
        uiMenuItem.SetLeftBadge((UIMenuItem.BadgeStyle) 6);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass150.required = this.GetRequiredPartsForWeaponGroup(cDisplayClass150.CS\u0024\u003C\u003E8__locals1.weaponGroup);
        // ISSUE: reference to a compiler-generated field
        uiMenuItem.set_Description(string.Format("Required Weapon Parts: ~y~{0}~s~", (object) cDisplayClass150.required));
        this._craftWeaponsMenu.AddItem(uiMenuItem);
        // ISSUE: method pointer
        uiMenuItem.add_Activated(new ItemActivatedEvent((object) cDisplayClass150, __methodptr(\u003CCraftAmmo\u003Eb__1)));
      }
      this._craftWeaponsMenu.set_Visible(!this._craftWeaponsMenu.get_Visible());
    }

    private int GetRequiredPartsForWeaponGroup(WeaponGroup group)
    {
      return this._requiredAmountDictionary.ContainsKey(group) ? this._requiredAmountDictionary[group] : 1;
    }

    private void UseWeaponsCrate(MapProp prop)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapInteraction.\u003C\u003Ec__DisplayClass17_0 cDisplayClass170 = new MapInteraction.\u003C\u003Ec__DisplayClass17_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.prop = prop;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass170.prop?.Weapons == null)
        return;
      // ISSUE: method pointer
      this._weaponsMenu.add_OnMenuChange(new MenuChangeEvent((object) cDisplayClass170, __methodptr(\u003CUseWeaponsCrate\u003Eb__0)));
      this._weaponsMenu.set_Visible(!this._weaponsMenu.get_Visible());
    }

    private static void TradeOffWeapons(MapProp item, List<Weapon> weapons, UIMenu currentMenu, bool giveToPlayer)
    {
      UIMenuItem uiMenuItem1 = new UIMenuItem("Back");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      uiMenuItem1.add_Activated(MapInteraction.\u003C\u003Ec.\u003C\u003E9__18_0 ?? (MapInteraction.\u003C\u003Ec.\u003C\u003E9__18_0 = new ItemActivatedEvent((object) MapInteraction.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CTradeOffWeapons\u003Eb__18_0))));
      currentMenu.Clear();
      currentMenu.AddItem(uiMenuItem1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      Action notify = MapInteraction.\u003C\u003Ec.\u003C\u003E9__18_1 ?? (MapInteraction.\u003C\u003Ec.\u003C\u003E9__18_1 = new Action((object) MapInteraction.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CTradeOffWeapons\u003Eb__18_1)));
      weapons.ForEach((Action<Weapon>) (weapon =>
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MapInteraction.\u003C\u003Ec__DisplayClass18_1 cDisplayClass181 = new MapInteraction.\u003C\u003Ec__DisplayClass18_1();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass181.CS\u0024\u003C\u003E8__locals1 = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass181.weapon = weapon;
        // ISSUE: reference to a compiler-generated field
        UIMenuItem uiMenuItem2 = new UIMenuItem(string.Format("{0}", (object) cDisplayClass181.weapon.Hash));
        currentMenu.AddItem(uiMenuItem2);
        // ISSUE: method pointer
        uiMenuItem2.add_Activated(new ItemActivatedEvent((object) cDisplayClass181, __methodptr(\u003CTradeOffWeapons\u003Eb__3)));
      }));
      currentMenu.RefreshIndex();
    }

    private void Sleep(Vector3 position)
    {
      // ISSUE: method pointer
      Ped[] array1 = ((IEnumerable<Ped>) Enumerable.Where<Ped>((IEnumerable<M0>) World.GetNearbyPeds(position, this._enemyRangeForSleeping), (Func<M0, bool>) new Func<Ped, bool>((object) null, __methodptr(IsEnemy)))).ToArray<Ped>();
      if (!((IEnumerable<Ped>) array1).Any<Ped>())
      {
        TimeSpan timeSpan = World.get_CurrentDayTime() + new TimeSpan(0, this._sleepHours, 0, 0);
        ((Entity) MapInteraction.PlayerPed).set_IsVisible(false);
        MapInteraction.Player.set_CanControlCharacter(false);
        ((Entity) MapInteraction.PlayerPed).set_FreezePosition(true);
        Game.FadeScreenOut(2000);
        Script.Wait(2000);
        World.set_CurrentDayTime(timeSpan);
        ((Entity) MapInteraction.PlayerPed).set_IsVisible(true);
        MapInteraction.Player.set_CanControlCharacter(true);
        ((Entity) MapInteraction.PlayerPed).set_FreezePosition(false);
        MapInteraction.PlayerPed.ClearBloodDamage();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        Weather[] array2 = ((IEnumerable<Weather>) Enumerable.Where<Weather>((IEnumerable<M0>) Enum.GetValues(typeof (Weather)), (Func<M0, bool>) (MapInteraction.\u003C\u003Ec.\u003C\u003E9__19_0 ?? (MapInteraction.\u003C\u003Ec.\u003C\u003E9__19_0 = new Func<Weather, bool>((object) MapInteraction.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CSleep\u003Eb__19_0)))))).ToArray<Weather>();
        World.set_Weather((Weather) (int) array2[Database.Random.Next(array2.Length)]);
        Script.Wait(2000);
        Game.FadeScreenIn(2000);
      }
      else
      {
        UI.Notify("There are ~r~enemies~s~ nearby.");
        UI.Notify("Marking them on your map.");
        Array.ForEach<Ped>(array1, new Action<Ped>(MapInteraction.AddBlip));
      }
    }

    private static void AddBlip(Ped ped)
    {
      if (((Entity) ped).get_CurrentBlip().Exists())
        return;
      ((Entity) ped).AddBlip().set_Name("Enemy Ped");
      EntityEventWrapper entityEventWrapper = new EntityEventWrapper((Entity) ped);
      entityEventWrapper.Died += (EntityEventWrapper.OnDeathEvent) ((sender, entity) =>
      {
        entity.get_CurrentBlip()?.Remove();
        sender.Dispose();
      });
      entityEventWrapper.Aborted += (EntityEventWrapper.OnWrapperAbortedEvent) ((sender, entity) => entity.get_CurrentBlip()?.Remove());
    }

    private static bool IsEnemy(Ped ped)
    {
      return ped.get_IsHuman() && !((Entity) ped).get_IsDead() && ped.GetRelationshipWithPed(MapInteraction.PlayerPed) == 5 || ped.IsInCombatAgainst(MapInteraction.PlayerPed);
    }
  }
}
