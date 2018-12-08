using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZombiesMod.DataClasses;
using ZombiesMod.Extensions;
using ZombiesMod.Static;

namespace ZombiesMod.PlayerManagement
{
  public class PlayerInventory : Script
  {
    public const float InteractDistance = 1.5f;
    private readonly UIMenu _mainMenu;
    private readonly List<Ped> _lootedPeds;
    private Inventory _inventory;
    private readonly Keys _inventoryKey;

    public static event PlayerInventory.OnUsedFoodEvent FoodUsed;

    public static event PlayerInventory.OnUsedWeaponEvent WeaponUsed;

    public static event PlayerInventory.OnUsedBuildableEvent BuildableUsed;

    public static event PlayerInventory.OnLootedEvent LootedPed;

    public PlayerInventory()
    {
      base.\u002Ector();
      this._inventoryKey = (Keys) this.get_Settings().GetValue<Keys>("keys", "inventory_key", (M0) this._inventoryKey);
      this.get_Settings().SetValue<Keys>("keys", "inventory_key", (M0) this._inventoryKey);
      this.get_Settings().Save();
      this._inventory = Serializer.Deserialize<Inventory>("./scripts/Inventory.dat") ?? new Inventory(MenuType.Player, false);
      this._inventory.LoadMenus();
      PlayerInventory.Instance = this;
      MenuConrtoller.MenuPool.Add(this._mainMenu);
      this._mainMenu.SetBannerType(new UIResRectangle());
      UIMenuItem uiMenuItem1 = new UIMenuItem("Inventory");
      UIMenuItem uiMenuItem2 = new UIMenuItem("Resources");
      UIMenuCheckboxItem menuCheckboxItem1 = new UIMenuCheckboxItem("Edit Mode", true, "Allow yourself to pickup objects.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem1.add_CheckboxEvent(PlayerInventory.\u003C\u003Ec.\u003C\u003E9__21_0 ?? (PlayerInventory.\u003C\u003Ec.\u003C\u003E9__21_0 = new ItemCheckboxEvent((object) PlayerInventory.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u002Ector\u003Eb__21_0))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      new UIMenuItem("Main Menu", "Navigate to the main menu. (For gamepad users)").add_Activated(PlayerInventory.\u003C\u003Ec.\u003C\u003E9__21_1 ?? (PlayerInventory.\u003C\u003Ec.\u003C\u003E9__21_1 = new ItemActivatedEvent((object) PlayerInventory.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u002Ector\u003Eb__21_1))));
      UIMenuCheckboxItem menuCheckboxItem2 = new UIMenuCheckboxItem("Developer Mode", this._inventory.DeveloperMode, "Enable/Disable infinite items and resources.");
      // ISSUE: method pointer
      menuCheckboxItem2.add_CheckboxEvent(new ItemCheckboxEvent((object) this, __methodptr(\u003C\u002Ector\u003Eb__21_2)));
      this._mainMenu.AddItem(uiMenuItem1);
      this._mainMenu.AddItem(uiMenuItem2);
      this._mainMenu.BindMenuToItem(this._inventory.InventoryMenu, uiMenuItem1);
      this._mainMenu.BindMenuToItem(this._inventory.ResourceMenu, uiMenuItem2);
      this._mainMenu.AddItem((UIMenuItem) menuCheckboxItem1);
      this._mainMenu.AddItem((UIMenuItem) menuCheckboxItem2);
      this._inventory.ItemUsed += new Inventory.ItemUsedEvent(this.InventoryOnItemUsed);
      this._inventory.AddedItem += (Inventory.AddedItemEvent) ((item, amount) => Serializer.Serialize<Inventory>("./scripts/Inventory.dat", this._inventory));
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_KeyUp(new KeyEventHandler(this.OnKeyUp));
      PlayerInventory.LootedPed += new PlayerInventory.OnLootedEvent(this.OnLootedPed);
    }

    public static PlayerInventory Instance { get; private set; }

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

    private void OnLootedPed(Ped ped)
    {
      if (ped.get_IsHuman())
        this.PickupLoot(ped, ItemType.Resource, 1, 3, 0.2f);
      else
        this.AnimalLoot(ped);
    }

    private void AnimalLoot(Ped ped)
    {
      if (!PlayerInventory.PlayerPed.get_Weapons().HasWeapon((WeaponHash) -1716189206))
      {
        UI.Notify("You need a knife!");
      }
      else
      {
        if (!this._inventory.AddItem(this.ItemFromName("Raw Meat"), 2, ItemType.Resource))
          return;
        PlayerInventory.PlayerPed.get_Weapons().Select((WeaponHash) -1716189206, true);
        UI.Notify("You gutted the animal for ~g~raw meat~s~.");
        PlayerInventory.PlayerPed.get_Task().PlayAnimation("amb@world_human_gardener_plant@male@base", "base", 8f, 3000, (AnimationFlags) 0);
        this._lootedPeds.Add(ped);
      }
    }

    public void PickupLoot(Ped ped, ItemType type = ItemType.Resource, int amountPerItemMin = 1, int amountPerItemMax = 3, float successChance = 0.2f)
    {
      List<InventoryItemBase> inventoryItemBaseList = type == ItemType.Resource ? this._inventory.Resources : this._inventory.Items;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      if (Enumerable.All<InventoryItemBase>((IEnumerable<M0>) inventoryItemBaseList, (Func<M0, bool>) (PlayerInventory.\u003C\u003Ec.\u003C\u003E9__32_0 ?? (PlayerInventory.\u003C\u003Ec.\u003C\u003E9__32_0 = new Func<InventoryItemBase, bool>((object) PlayerInventory.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CPickupLoot\u003Eb__32_0))))))
      {
        UI.Notify(string.Format("Your {0}s are full!", (object) type));
      }
      else
      {
        int amount = 0;
        inventoryItemBaseList.ForEach((Action<InventoryItemBase>) (i =>
        {
          if (i.Id == "Cooked Meat" || Database.Random.NextDouble() > (double) successChance)
            return;
          int amount = Database.Random.Next(amountPerItemMin, amountPerItemMax);
          if (i.Amount + amount > i.MaxAmount)
            amount = i.MaxAmount - i.Amount;
          this._inventory.AddItem(i, amount, type);
          amount += amount;
        }));
        UI.Notify(string.Format("{0}", amount > 0 ? (object) string.Format("{0}s: +~g~{1}", (object) type, (object) amount) : (object) "Nothing found."), true);
        PlayerInventory.PlayerPed.get_Task().PlayAnimation("pickup_object", "pickup_low");
        if (Entity.op_Equality((Entity) ped, (Entity) null))
          return;
        this._lootedPeds.Add(ped);
      }
    }

    private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
    {
      if (MenuConrtoller.MenuPool.IsAnyMenuOpen() || keyEventArgs.KeyCode != this._inventoryKey)
        return;
      this._mainMenu.set_Visible(!this._mainMenu.get_Visible());
    }

    private void InventoryOnItemUsed(InventoryItemBase item, ItemType type)
    {
      if (item == null || type == ItemType.Resource)
        return;
      int? eventArgument;
      if (System.Type.op_Equality(item.GetType(), typeof (FoodInventoryItem)))
      {
        FoodInventoryItem foodInventoryItem = (FoodInventoryItem) item;
        PlayerInventory.PlayerPed.get_Task().PlayAnimation(foodInventoryItem.AnimationDict, foodInventoryItem.AnimationName, 8f, foodInventoryItem.AnimationDuration, foodInventoryItem.AnimationFlags);
        // ISSUE: reference to a compiler-generated field
        PlayerInventory.OnUsedFoodEvent foodUsed = PlayerInventory.FoodUsed;
        if (foodUsed != null)
          foodUsed(foodInventoryItem, foodInventoryItem.FoodType);
      }
      else if (System.Type.op_Equality(item.GetType(), typeof (WeaponInventoryItem)))
      {
        WeaponInventoryItem weapon = (WeaponInventoryItem) item;
        PlayerInventory.PlayerPed.get_Weapons().Give(weapon.Hash, weapon.Ammo, true, true);
        // ISSUE: reference to a compiler-generated field
        PlayerInventory.OnUsedWeaponEvent weaponUsed = PlayerInventory.WeaponUsed;
        if (weaponUsed != null)
          weaponUsed(weapon);
      }
      else if (System.Type.op_Equality(item.GetType(), typeof (BuildableInventoryItem)) || System.Type.op_Equality(item.GetType(), typeof (WeaponStorageInventoryItem)))
      {
        if (PlayerInventory.PlayerPed.IsInVehicle())
        {
          UI.Notify("You can't build while in a vehicle!");
          return;
        }
        BuildableInventoryItem buildableInventoryItem = (BuildableInventoryItem) item;
        ItemPreview itemPreview = new ItemPreview();
        itemPreview.StartPreview(buildableInventoryItem.PropName, buildableInventoryItem.GroundOffset, buildableInventoryItem.IsDoor);
        while (!itemPreview.PreviewComplete)
          Script.Yield();
        Prop result = itemPreview.GetResult();
        if (Entity.op_Equality((Entity) result, (Entity) null))
          return;
        PlayerInventory.AddBlipToProp((IProp) buildableInventoryItem, buildableInventoryItem.Id, (Entity) result);
        // ISSUE: reference to a compiler-generated field
        PlayerInventory.OnUsedBuildableEvent buildableUsed = PlayerInventory.BuildableUsed;
        if (buildableUsed != null)
          buildableUsed(buildableInventoryItem, result);
      }
      else if (System.Type.op_Equality(item.GetType(), typeof (UsableInventoryItem)))
      {
        foreach (UsableItemEvent itemEvent in ((UsableInventoryItem) item).ItemEvents)
        {
          switch (itemEvent.Event)
          {
            case ItemEvent.GiveArmor:
              eventArgument = itemEvent.EventArgument as int?;
              int num1 = eventArgument ?? 0;
              Ped playerPed1 = PlayerInventory.PlayerPed;
              ((Entity) playerPed1).set_Health(((Entity) playerPed1).get_Health() + num1);
              break;
            case ItemEvent.GiveHealth:
              eventArgument = itemEvent.EventArgument as int?;
              int num2 = eventArgument ?? 0;
              Ped playerPed2 = PlayerInventory.PlayerPed;
              playerPed2.set_Armor(playerPed2.get_Armor() + num2);
              break;
          }
        }
      }
      else if (System.Type.op_Equality(item.GetType(), typeof (CraftableInventoryItem)) && !((CraftableInventoryItem) item).Validation.Invoke())
        return;
      this._inventory.AddItem(item, -1, type);
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      this._inventory.ProcessKeys();
      this.GetWater();
      this.LootDeadPeds();
    }

    private void GetWater()
    {
      if (PlayerInventory.PlayerPed.IsInVehicle() || PlayerInventory.PlayerPed.get_IsSwimming() || (!((Entity) PlayerInventory.PlayerPed).get_IsInWater() || ((Entity) PlayerInventory.PlayerPed).IsPlayingAnim("pickup_object", "pickup_low")))
        return;
      InventoryItemBase inventoryItemBase = this._inventory.Resources.Find((Predicate<InventoryItemBase>) (i => i.Id == "Bottle"));
      if (inventoryItemBase == null || inventoryItemBase.Amount <= 0)
        return;
      Game.DisableControlThisFrame(2, (Control) 23);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 23))
        return;
      PlayerInventory.PlayerPed.get_Task().PlayAnimation("pickup_object", "pickup_low");
      this.AddItem(this.ItemFromName("Dirty Water"), 1, ItemType.Resource);
      this.AddItem(inventoryItemBase, -1, ItemType.Resource);
      UI.Notify("Resources: -~r~1", true);
      UI.Notify("Resources: +~g~1", true);
    }

    private void LootDeadPeds()
    {
      if (PlayerInventory.PlayerPed.IsInVehicle())
        return;
      Ped closest = (Ped) World.GetClosest<Ped>(PlayerInventory.PlayerPosition, (M0[]) World.GetNearbyPeds(PlayerInventory.PlayerPed, 1.5f));
      if (Entity.op_Equality((Entity) closest, (Entity) null) || !((Entity) closest).get_IsDead() || this._lootedPeds.Contains(closest))
        return;
      UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to loot.", false);
      Game.DisableControlThisFrame(2, (Control) 51);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 51))
        return;
      // ISSUE: reference to a compiler-generated field
      PlayerInventory.OnLootedEvent lootedPed = PlayerInventory.LootedPed;
      if (lootedPed == null)
        return;
      lootedPed(closest);
    }

    private void Controller()
    {
    }

    public bool AddItem(InventoryItemBase item, int amount, ItemType type)
    {
      return item != null && this._inventory.AddItem(item, amount, type);
    }

    public bool PickupItem(InventoryItemBase item, ItemType type)
    {
      return item != null && this._inventory.AddItem(item, 1, type);
    }

    public InventoryItemBase ItemFromName(string id)
    {
      if (this._inventory?.Items == null || this._inventory?.Resources == null)
        return (InventoryItemBase) null;
      return Array.Find<InventoryItemBase>(this._inventory.Items.Concat<InventoryItemBase>((IEnumerable<InventoryItemBase>) this._inventory.Resources).ToArray<InventoryItemBase>(), (Predicate<InventoryItemBase>) (i => i.Id == id));
    }

    private static void AddBlipToProp(IProp item, string name, Entity entity)
    {
      if (item.BlipSprite == 1)
        return;
      Blip blip = entity.AddBlip();
      blip.set_Sprite(item.BlipSprite);
      blip.set_Color(item.BlipColor);
      blip.set_Name(name);
    }

    public bool HasItem(InventoryItemBase item, ItemType itemType)
    {
      if (item == null)
        return false;
      switch (itemType)
      {
        case ItemType.Resource:
          return this._inventory.Resources.Contains(item) && item.Amount > 0;
        case ItemType.Item:
          return this._inventory.Items.Contains(item) && item.Amount > 0;
        default:
          return false;
      }
    }

    public delegate void OnUsedFoodEvent(FoodInventoryItem item, FoodType foodType);

    public delegate void OnUsedWeaponEvent(WeaponInventoryItem weapon);

    public delegate void OnUsedBuildableEvent(BuildableInventoryItem item, Prop newProp);

    public delegate void OnLootedEvent(Ped ped);
  }
}
