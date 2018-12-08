using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombiesMod.Static;

namespace ZombiesMod
{
  [Serializable]
  public class Inventory
  {
    public List<InventoryItemBase> Items = new List<InventoryItemBase>();
    public List<InventoryItemBase> Resources = new List<InventoryItemBase>();
    public const float InteractDist = 2.5f;
    [NonSerialized]
    public readonly MenuType MenuType;
    [NonSerialized]
    public UIMenu InventoryMenu;
    [NonSerialized]
    public UIMenu ResourceMenu;

    public event Inventory.CraftedItemEvent TryCraft;

    public event Inventory.ItemUsedEvent ItemUsed;

    public event Inventory.AddedItemEvent AddedItem;

    public Inventory(MenuType menuType, bool ignoreContainers = false)
    {
      this.MenuType = menuType;
      InventoryItemBase resource1 = new InventoryItemBase(0, 20, "Alcohol", "A colorless volatile flammable liquid.");
      InventoryItemBase resource2 = new InventoryItemBase(0, 25, "Battery", "A resource that can provide an electrical charge.");
      InventoryItemBase resource3 = new InventoryItemBase(0, 25, "Binding", "A strong adhesive.");
      InventoryItemBase resource4 = new InventoryItemBase(0, 10, "Bottle", "A container used for storing drinks or other liquids..");
      InventoryItemBase resource5 = new InventoryItemBase(0, 25, "Cloth", "Woven or felted fabric.");
      InventoryItemBase resource6 = new InventoryItemBase(0, 25, "Dirty Water", "Liquid obtained from an undrinkable source of water.");
      InventoryItemBase resource7 = new InventoryItemBase(0, 25, "Metal", "It's freaking metal.");
      InventoryItemBase resource8 = new InventoryItemBase(0, 25, "Wood", "It's freaking wood.");
      InventoryItemBase resource9 = new InventoryItemBase(0, 25, "Plastic", "A synthetic material made from a wide range of organic polymers.");
      InventoryItemBase resource10 = new InventoryItemBase(0, 15, "Raw Meat", "Can be cooked to create ~g~Cooked Meat~s~.");
      InventoryItemBase resource11 = new InventoryItemBase(0, 10, "Matches", "Can be used to create fire.");
      InventoryItemBase resource12 = new InventoryItemBase(25, 25, "Weapon Parts", "Used to create weapon components, and weapons. (Weapons crafting coming soon)");
      InventoryItemBase resource13 = new InventoryItemBase(0, 25, "Vehicle Parts", "USed to repair vehicles.");
      UsableInventoryItem usableInventoryItem = new UsableInventoryItem(0, 10, "Bandage", "A strip of material used to bind a wound or to protect an injured part of the body.", new UsableItemEvent[2]
      {
        new UsableItemEvent(ItemEvent.GiveHealth, (object) 25),
        new UsableItemEvent(ItemEvent.GiveArmor, (object) 15)
      })
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource3, 1),
          new CraftableItemComponent(resource1, 2),
          new CraftableItemComponent(resource5, 2)
        }
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      CraftableInventoryItem craftableInventoryItem1 = new CraftableInventoryItem(0, 5, "Suppressor", "Can be used to suppress a rifle, pistol, shotgun, or SMG.", Inventory.\u003C\u003Ec.\u003C\u003E9__18_0 ?? (Inventory.\u003C\u003Ec.\u003C\u003E9__18_0 = new Func<bool>((object) Inventory.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u002Ector\u003Eb__18_0))))
      {
        RequiredComponents = new CraftableItemComponent[2]
        {
          new CraftableItemComponent(resource12, 2),
          new CraftableItemComponent(resource3, 1)
        }
      };
      BuildableInventoryItem buildableInventoryItem1 = new BuildableInventoryItem(0, 5, "Sand Block", "Used to provide cover in combat situations", "prop_mb_sandblock_02", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), false, false, true)
      {
        RequiredComponents = new CraftableItemComponent[4]
        {
          new CraftableItemComponent(resource7, 3),
          new CraftableItemComponent(resource3, 2),
          new CraftableItemComponent(resource5, 1),
          new CraftableItemComponent(resource8, 2)
        }
      };
      BuildableInventoryItem buildableInventoryItem2 = new BuildableInventoryItem(0, 2, "Work Bench", "Can be used to craft ammunition.", "prop_tool_bench02", (BlipSprite) 110, (BlipColor) 66, Vector3.get_Zero(), true, false, true)
      {
        RequiredComponents = new CraftableItemComponent[4]
        {
          new CraftableItemComponent(resource7, 15),
          new CraftableItemComponent(resource8, 5),
          new CraftableItemComponent(resource9, 5),
          new CraftableItemComponent(resource3, 5)
        }
      };
      BuildableInventoryItem buildableInventoryItem3 = new BuildableInventoryItem(0, 3, "Gate", "A metal gate that can be opened by vehicles or peds.", "prop_gate_prison_01", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), true, true, true)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource7, 5),
          new CraftableItemComponent(resource3, 3),
          new CraftableItemComponent(resource2, 1)
        }
      };
      WeaponInventoryItem weaponInventoryItem1 = new WeaponInventoryItem(0, 25, "Molotov", "A bottle-based improvised incendiary weapon.", 1, (WeaponHash) 615608432, (WeaponComponent[]) null)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource1, 1),
          new CraftableItemComponent(resource5, 1),
          new CraftableItemComponent(resource4, 1)
        }
      };
      WeaponInventoryItem weaponInventoryItem2 = new WeaponInventoryItem(0, 1, "Knife", "An improvised knife.", 1, (WeaponHash) -1716189206, (WeaponComponent[]) null)
      {
        RequiredComponents = new CraftableItemComponent[2]
        {
          new CraftableItemComponent(resource7, 3),
          new CraftableItemComponent(resource3, 1)
        }
      };
      WeaponInventoryItem weaponInventoryItem3 = new WeaponInventoryItem(0, 5, "Flashlight", "A battery-operated portable light.", 1, (WeaponHash) -1951375401, (WeaponComponent[]) null)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource2, 4),
          new CraftableItemComponent(resource9, 4),
          new CraftableItemComponent(resource3, 4)
        }
      };
      FoodInventoryItem foodInventoryItem1 = new FoodInventoryItem(0, 15, "Cooked Meat", "Can be creating from cooking raw meat.", "mp_player_inteat@burger", "mp_player_int_eat_burger", (AnimationFlags) 16, -1, FoodType.Food, 0.25f)
      {
        RequiredComponents = new CraftableItemComponent[2]
        {
          new CraftableItemComponent(resource10, 1),
          new CraftableItemComponent(resource1, 2)
        },
        NearbyResource = NearbyResource.CampFire
      };
      FoodInventoryItem foodInventoryItem2 = new FoodInventoryItem(0, 15, "Packaged Food", "Usually obtained from stores around Los Santos.", "mp_player_inteat@pnq", "loop", (AnimationFlags) 16, -1, FoodType.SpecialFood, 0.3f);
      FoodInventoryItem foodInventoryItem3 = new FoodInventoryItem(0, 15, "Clean Water", "Can be made from dirty water or obtained from stores around Los Santos.", "mp_player_intdrink", "loop_bottle", (AnimationFlags) 16, -1, FoodType.Water, 0.35f)
      {
        RequiredComponents = new CraftableItemComponent[1]
        {
          new CraftableItemComponent(resource6, 1)
        },
        NearbyResource = NearbyResource.CampFire
      };
      BuildableInventoryItem buildableInventoryItem4 = new BuildableInventoryItem(1, 5, "Tent", "A portable shelter made of cloth, supported by one or more poles and stretched tight by cords or loops attached to pegs driven into the ground.", "prop_skid_tent_01", (BlipSprite) 411, (BlipColor) 0, Vector3.get_Zero(), true, false, true)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource8, 3),
          new CraftableItemComponent(resource5, 4),
          new CraftableItemComponent(resource3, 3)
        }
      };
      BuildableInventoryItem buildableInventoryItem5 = new BuildableInventoryItem(1, 5, "Camp Fire", "An open-air fire in a camp, used for cooking and as a focal point for social activity.", "prop_beach_fire", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), false, false, true)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource8, 3),
          new CraftableItemComponent(resource1, 1),
          new CraftableItemComponent(resource11, 1)
        }
      };
      BuildableInventoryItem buildableInventoryItem6 = new BuildableInventoryItem(0, 15, "Wall", "A wooden wall that can be used for creating shelters.", "prop_fncconstruc_01d", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), false, false, true)
      {
        RequiredComponents = new CraftableItemComponent[2]
        {
          new CraftableItemComponent(resource8, 4),
          new CraftableItemComponent(resource3, 3)
        }
      };
      BuildableInventoryItem buildableInventoryItem7 = new BuildableInventoryItem(0, 10, "Barrier", "A wooden barrier that can be used to barricade gaps in your safe zone.", "prop_fncwood_16b", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), false, false, true)
      {
        RequiredComponents = new CraftableItemComponent[2]
        {
          new CraftableItemComponent(resource8, 5),
          new CraftableItemComponent(resource3, 2)
        }
      };
      BuildableInventoryItem buildableInventoryItem8 = new BuildableInventoryItem(0, 5, "Door", "A  hinged, sliding, or revolving barrier at the entrance to a building, room, or vehicle, or in the framework of a cupboard.", "ex_p_mp_door_office_door01", (BlipSprite) 1, (BlipColor) 0, Vector3.get_Zero(), true, true, true)
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource8, 3),
          new CraftableItemComponent(resource3, 1),
          new CraftableItemComponent(resource7, 1)
        }
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      CraftableInventoryItem craftableInventoryItem2 = new CraftableInventoryItem(0, 10, "Vehicle Repair Kit", "Used to repair vehicle engines.", Inventory.\u003C\u003Ec.\u003C\u003E9__18_1 ?? (Inventory.\u003C\u003Ec.\u003C\u003E9__18_1 = new Func<bool>((object) Inventory.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u002Ector\u003Eb__18_1))))
      {
        RequiredComponents = new CraftableItemComponent[3]
        {
          new CraftableItemComponent(resource13, 5),
          new CraftableItemComponent(resource7, 5),
          new CraftableItemComponent(resource3, 2)
        }
      };
      this.Items.AddRange((IEnumerable<InventoryItemBase>) new InventoryItemBase[17]
      {
        (InventoryItemBase) usableInventoryItem,
        (InventoryItemBase) weaponInventoryItem1,
        (InventoryItemBase) weaponInventoryItem2,
        (InventoryItemBase) weaponInventoryItem3,
        (InventoryItemBase) foodInventoryItem1,
        (InventoryItemBase) foodInventoryItem2,
        (InventoryItemBase) foodInventoryItem3,
        (InventoryItemBase) buildableInventoryItem4,
        (InventoryItemBase) buildableInventoryItem5,
        (InventoryItemBase) buildableInventoryItem6,
        (InventoryItemBase) buildableInventoryItem7,
        (InventoryItemBase) buildableInventoryItem8,
        (InventoryItemBase) craftableInventoryItem1,
        (InventoryItemBase) buildableInventoryItem3,
        (InventoryItemBase) buildableInventoryItem1,
        (InventoryItemBase) buildableInventoryItem2,
        (InventoryItemBase) craftableInventoryItem2
      });
      this.Resources.AddRange((IEnumerable<InventoryItemBase>) new InventoryItemBase[13]
      {
        resource3,
        resource1,
        resource5,
        resource4,
        resource7,
        resource8,
        resource2,
        resource9,
        resource10,
        resource6,
        resource11,
        resource12,
        resource13
      });
      this.Items.Sort((Comparison<InventoryItemBase>) ((c1, c2) => string.Compare(c1.Id, c2.Id, StringComparison.Ordinal)));
      this.Resources.Sort((Comparison<InventoryItemBase>) ((c1, c2) => string.Compare(c1.Id, c2.Id, StringComparison.Ordinal)));
      this.LoadMenus();
      if (ignoreContainers)
        return;
      WeaponStorageInventoryItem storageInventoryItem = new WeaponStorageInventoryItem(0, 1, "Weapons Crate", "A crate specifically used to store weapons.", "hei_prop_carrier_crate_01a", (BlipSprite) 150, (BlipColor) 0, Vector3.get_Zero(), true, false, true, new List<Weapon>());
      storageInventoryItem.RequiredComponents = new CraftableItemComponent[4]
      {
        new CraftableItemComponent(resource7, 15),
        new CraftableItemComponent(resource8, 15),
        new CraftableItemComponent(resource9, 15),
        new CraftableItemComponent(resource3, 10)
      };
      this.Items.Add((InventoryItemBase) storageInventoryItem);
    }

    private static Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    public bool DeveloperMode { get; set; }

    public void LoadMenus()
    {
      this.InventoryMenu = new UIMenu(nameof (Inventory), "SELECT AN ITEM");
      this.ResourceMenu = new UIMenu("Resources", "SELECT AN ITEM");
      this.AddItemsToMenu(this.InventoryMenu, this.Items, ItemType.Item);
      this.AddItemsToMenu(this.ResourceMenu, this.Resources, ItemType.Resource);
      MenuConrtoller.MenuPool.Add(this.InventoryMenu);
      MenuConrtoller.MenuPool.Add(this.ResourceMenu);
      this.RefreshMenu();
      this.InventoryMenu.RefreshIndex();
      this.ResourceMenu.RefreshIndex();
      if ((uint) this.MenuType > 0U)
        return;
      this.InventoryMenu.AddInstructionalButton(new InstructionalButton((Control) 23, "Blueprint"));
      this.InventoryMenu.AddInstructionalButton(new InstructionalButton((Control) 26, "Craft"));
    }

    public void RefreshMenu()
    {
      this.UpdateMenuSpecific(this.InventoryMenu, this.Items, this.MenuType == MenuType.Player);
      this.UpdateMenuSpecific(this.ResourceMenu, this.Resources, false);
    }

    public void ProcessKeys()
    {
      if ((uint) this.MenuType > 0U || !this.InventoryMenu.get_Visible())
        return;
      Game.DisableControlThisFrame(2, (Control) 23);
      Game.DisableControlThisFrame(2, (Control) 75);
      Game.DisableControlThisFrame(2, (Control) 26);
      if (Game.IsDisabledControlJustPressed(2, (Control) 23))
      {
        ICraftable selectedInventoryItem = this.GetSelectedInventoryItem<ICraftable>();
        if (selectedInventoryItem == null)
          return;
        StringBuilder str = new StringBuilder("Blueprint:\n");
        if (selectedInventoryItem.RequiredComponents == null || !((IEnumerable<CraftableItemComponent>) selectedInventoryItem.RequiredComponents).Any<CraftableItemComponent>() && !this.DeveloperMode)
          return;
        Array.ForEach<CraftableItemComponent>(selectedInventoryItem.RequiredComponents, (Action<CraftableItemComponent>) (i => str.Append(string.Format("{0}{1}~s~ / {2} {3}\n", i.Resource.Amount >= i.RequiredAmount ? (object) "~g~" : (object) "~r~", (object) i.Resource.Amount, (object) i.RequiredAmount, (object) i.Resource.Id))));
        UI.Notify(str.ToString());
      }
      else
      {
        if (!Game.IsDisabledControlJustPressed(2, (Control) 26))
          return;
        InventoryItemBase selectedInventoryItem = this.GetSelectedInventoryItem<InventoryItemBase>();
        ICraftable craftable = selectedInventoryItem as ICraftable;
        if (selectedInventoryItem == null)
          throw new NullReferenceException("item");
        if (craftable == null)
          return;
        this.Craft(selectedInventoryItem, craftable);
      }
    }

    public bool AddItem(InventoryItemBase item, int amount, ItemType type)
    {
      if (!this.DeveloperMode)
      {
        if (item.Amount + amount < 0)
          return false;
        if (item.Amount + amount > item.MaxAmount)
        {
          UI.Notify(string.Format("There's not enough room for anymore ~g~{0}s~s~.", (object) item.Id), true);
          return false;
        }
      }
      item.Amount += amount;
      switch (type)
      {
        case ItemType.Resource:
          this.UpdateMenuSpecific(this.ResourceMenu, this.Resources, false);
          break;
        case ItemType.Item:
          this.UpdateMenuSpecific(this.InventoryMenu, this.Items, this.MenuType == MenuType.Player);
          break;
      }
      this.RefreshMenu();
      // ISSUE: reference to a compiler-generated field
      Inventory.AddedItemEvent addedItem = this.AddedItem;
      if (addedItem != null)
        addedItem(item, item.Amount);
      return true;
    }

    private void Craft(InventoryItemBase item, ICraftable craftable)
    {
      if ((uint) this.MenuType > 0U || item == null || craftable == null || !this.DeveloperMode && (!this.CanCraftItem(craftable) || item.Amount >= item.MaxAmount))
        return;
      FoodInventoryItem foodInventoryItem = item as FoodInventoryItem;
      if (!this.DeveloperMode)
      {
        NearbyResource? nearbyResource = foodInventoryItem?.NearbyResource;
        if (nearbyResource.HasValue && nearbyResource.GetValueOrDefault() == NearbyResource.CampFire && !((IEnumerable<Prop>) World.GetNearbyProps(Inventory.PlayerPosition, 2.5f, Model.op_Implicit("prop_beach_fire"))).Any<Prop>())
        {
          UI.Notify("There's no ~g~Camp Fire~s~ nearby.");
          return;
        }
      }
      this.AddItem(item, 1, ItemType.Item);
      Array.ForEach<CraftableItemComponent>(craftable.RequiredComponents, (Action<CraftableItemComponent>) (c => this.AddItem(c.Resource, -c.RequiredAmount, ItemType.Resource)));
      // ISSUE: reference to a compiler-generated field
      Inventory.CraftedItemEvent tryCraft = this.TryCraft;
      if (tryCraft == null)
        return;
      tryCraft(item);
    }

    private void AddItemsToMenu(UIMenu menu, List<InventoryItemBase> items, ItemType type)
    {
      items.ForEach((Action<InventoryItemBase>) (i =>
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Inventory.\u003C\u003Ec__DisplayClass32_1 cDisplayClass321 = new Inventory.\u003C\u003Ec__DisplayClass32_1();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass321.CS\u0024\u003C\u003E8__locals1 = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass321.i = i;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass321.i.CreateMenuItem();
        // ISSUE: reference to a compiler-generated field
        menu.AddItem(cDisplayClass321.i.MenuItem);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        cDisplayClass321.i.MenuItem.add_Activated(new ItemActivatedEvent((object) cDisplayClass321, __methodptr(\u003CAddItemsToMenu\u003Eb__1)));
      }));
    }

    private void UpdateMenuSpecific(UIMenu menu, List<InventoryItemBase> collection, bool leftBadges)
    {
      ((List<UIMenuItem>) menu.MenuItems).ForEach((Action<UIMenuItem>) (menuItem =>
      {
        InventoryItemBase itemFromMenuItem = Inventory.GetItemFromMenuItem<InventoryItemBase>(collection, menuItem);
        if (itemFromMenuItem == null)
          return;
        if (((this.CanCraftItem(itemFromMenuItem as ICraftable) ? 1 : (this.DeveloperMode ? 1 : 0)) & (leftBadges ? 1 : 0)) != 0)
          menuItem.SetLeftBadge((UIMenuItem.BadgeStyle) 22);
        else if (leftBadges)
          menuItem.SetLeftBadge((UIMenuItem.BadgeStyle) 21);
        menuItem.SetRightLabel(string.Format("{0}/{1}", (object) itemFromMenuItem.Amount, (object) itemFromMenuItem.MaxAmount));
      }));
    }

    private bool CanCraftItem(ICraftable craftable)
    {
      if (craftable?.RequiredComponents == null)
        return false;
      foreach (CraftableItemComponent requiredComponent in craftable.RequiredComponents)
      {
        InventoryItemBase resource = requiredComponent.Resource;
        if (this.Resources.Contains(resource))
        {
          int? amount = this.Resources.Find((Predicate<InventoryItemBase>) (i => resource == i))?.Amount;
          int requiredAmount = requiredComponent.RequiredAmount;
          if (amount.GetValueOrDefault() < requiredAmount && amount.HasValue)
            return false;
        }
      }
      return true;
    }

    private T GetSelectedInventoryItem<T>() where T : class
    {
      return Inventory.GetItemFromMenuItem<T>(this.Items, ((List<UIMenuItem>) this.InventoryMenu.MenuItems)[this.InventoryMenu.get_CurrentSelection()]);
    }

    private static T GetItemFromMenuItem<T>(List<InventoryItemBase> collection, UIMenuItem menuItem) where T : class
    {
      return collection.Find((Predicate<InventoryItemBase>) (i => i.MenuItem == menuItem)) as T;
    }

    public delegate void CraftedItemEvent(InventoryItemBase item);

    public delegate void ItemUsedEvent(InventoryItemBase item, ItemType type);

    public delegate void AddedItemEvent(InventoryItemBase item, int newAmount);
  }
}
