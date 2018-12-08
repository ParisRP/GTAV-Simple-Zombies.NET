using GTA.Native;

namespace ZombiesMod
{
  public interface IWeapon
  {
    int Ammo { get; set; }

    WeaponHash Hash { get; set; }

    WeaponComponent[] Components { get; set; }
  }
}
