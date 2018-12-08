using GTA;
using NativeUI;
using System.Windows.Forms;
using ZombiesMod.Static;
using ZombiesMod.Zombies;

namespace ZombiesMod
{
  public class ModController : Script
  {
    private Keys _menuKey;

    public ModController()
    {
      base.\u002Ector();
      ModController.Instance = this;
      Config.Check();
      Relationships.SetRelationships();
      this.LoadSave();
      this.ConfigureMenu();
      this.add_KeyUp(new KeyEventHandler(this.OnKeyUp));
    }

    public UIMenu MainMenu { get; private set; }

    private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
    {
      if (MenuConrtoller.MenuPool.IsAnyMenuOpen() || keyEventArgs.KeyCode != this._menuKey)
        return;
      this.MainMenu.set_Visible(!this.MainMenu.get_Visible());
    }

    public static ModController Instance { get; private set; }

    private void LoadSave()
    {
      this._menuKey = (Keys) this.get_Settings().GetValue<Keys>("keys", "zombies_menu_key", (M0) this._menuKey);
      ZombiePed.ZombieDamage = (int) this.get_Settings().GetValue<int>("zombies", "zombie_damage", (M0) ZombiePed.ZombieDamage);
      this.get_Settings().SetValue<Keys>("keys", "zombies_menu_key", (M0) this._menuKey);
      this.get_Settings().SetValue<int>("zombies", "zombie_damage", (M0) ZombiePed.ZombieDamage);
      this.get_Settings().Save();
    }

    private void ConfigureMenu()
    {
      this.MainMenu = new UIMenu("Simple Zombies", "SELECT AN OPTION");
      MenuConrtoller.MenuPool.Add(this.MainMenu);
      UIMenuCheckboxItem menuCheckboxItem1 = new UIMenuCheckboxItem("Infection Mode", false, "Enable/Disable zombies.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem1.add_CheckboxEvent(ModController.\u003C\u003Ec.\u003C\u003E9__12_0 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_0 = new ItemCheckboxEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_0))));
      UIMenuCheckboxItem menuCheckboxItem2 = new UIMenuCheckboxItem("Fast Zombies", false, "Enable/Disable running zombies.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem2.add_CheckboxEvent(ModController.\u003C\u003Ec.\u003C\u003E9__12_1 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_1 = new ItemCheckboxEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_1))));
      UIMenuCheckboxItem menuCheckboxItem3 = new UIMenuCheckboxItem("Electricity", true, "Enables/Disable blackout mode.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem3.add_CheckboxEvent(ModController.\u003C\u003Ec.\u003C\u003E9__12_2 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_2 = new ItemCheckboxEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_2))));
      UIMenuCheckboxItem menuCheckboxItem4 = new UIMenuCheckboxItem("Survivors", false, "Enable/Disable survivors.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem4.add_CheckboxEvent(ModController.\u003C\u003Ec.\u003C\u003E9__12_3 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_3 = new ItemCheckboxEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_3))));
      UIMenuCheckboxItem menuCheckboxItem5 = new UIMenuCheckboxItem("Stats", true, "Enable/Disable stats.");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      menuCheckboxItem5.add_CheckboxEvent(ModController.\u003C\u003Ec.\u003C\u003E9__12_4 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_4 = new ItemCheckboxEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_4))));
      UIMenuItem uiMenuItem1 = new UIMenuItem("Load", "Load the map, your vehicles and your bodyguards.");
      uiMenuItem1.SetLeftBadge((UIMenuItem.BadgeStyle) 14);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      uiMenuItem1.add_Activated(ModController.\u003C\u003Ec.\u003C\u003E9__12_5 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_5 = new ItemActivatedEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_5))));
      UIMenuItem uiMenuItem2 = new UIMenuItem("Save", "Saves the vehicle you are currently in.");
      uiMenuItem2.SetLeftBadge((UIMenuItem.BadgeStyle) 12);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      uiMenuItem2.add_Activated(ModController.\u003C\u003Ec.\u003C\u003E9__12_6 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_6 = new ItemActivatedEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_6))));
      UIMenuItem uiMenuItem3 = new UIMenuItem("Save All", "Saves all marked vehicles, and their positions.");
      uiMenuItem3.SetLeftBadge((UIMenuItem.BadgeStyle) 12);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      uiMenuItem3.add_Activated(ModController.\u003C\u003Ec.\u003C\u003E9__12_7 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_7 = new ItemActivatedEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_7))));
      UIMenuItem uiMenuItem4 = new UIMenuItem("Save All", "Saves the player ped group (guards).");
      uiMenuItem4.SetLeftBadge((UIMenuItem.BadgeStyle) 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      uiMenuItem4.add_Activated(ModController.\u003C\u003Ec.\u003C\u003E9__12_8 ?? (ModController.\u003C\u003Ec.\u003C\u003E9__12_8 = new ItemActivatedEvent((object) ModController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CConfigureMenu\u003Eb__12_8))));
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem1);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem2);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem3);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem4);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem5);
      this.MainMenu.AddItem(uiMenuItem1);
      this.MainMenu.AddItem(uiMenuItem2);
      this.MainMenu.AddItem(uiMenuItem3);
      this.MainMenu.AddItem(uiMenuItem4);
      this.MainMenu.RefreshIndex();
    }
  }
}
