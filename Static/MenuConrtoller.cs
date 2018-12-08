using GTA;
using NativeUI;
using System;

namespace ZombiesMod.Static
{
  public class MenuConrtoller : Script
  {
    private static MenuPool _menuPool;
    private static TimerBarPool _barPool;

    public MenuConrtoller()
    {
      base.\u002Ector();
      this.add_Tick(new EventHandler(this.OnTick));
    }

    public static MenuPool MenuPool
    {
      get
      {
        return MenuConrtoller._menuPool ?? (MenuConrtoller._menuPool = new MenuPool());
      }
    }

    public static TimerBarPool BarPool
    {
      get
      {
        return MenuConrtoller._barPool ?? (MenuConrtoller._barPool = new TimerBarPool());
      }
    }

    public void OnTick(object sender, EventArgs eventArgs)
    {
      if (MenuConrtoller._barPool != null && (MenuConrtoller._menuPool == null || MenuConrtoller._menuPool != null && !MenuConrtoller._menuPool.IsAnyMenuOpen()))
        MenuConrtoller._barPool.Draw();
      MenuConrtoller._menuPool?.ProcessMenus();
    }
  }
}
