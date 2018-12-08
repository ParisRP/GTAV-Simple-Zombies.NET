using GTA;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class GameExtended
  {
    public static void DisableWeaponWheel()
    {
      Game.DisableControlThisFrame(2, (Control) 13);
      Game.DisableControlThisFrame(2, (Control) 14);
      Game.DisableControlThisFrame(2, (Control) 15);
      Game.DisableControlThisFrame(2, (Control) 12);
      Game.DisableControlThisFrame(2, (Control) 262);
      Game.DisableControlThisFrame(2, (Control) 56);
      Game.DisableControlThisFrame(2, (Control) 261);
      Game.DisableControlThisFrame(2, (Control) 53);
      Game.DisableControlThisFrame(2, (Control) 54);
      Game.DisableControlThisFrame(2, (Control) 37);
    }

    public static int GetMobilePhoneId()
    {
      OutputArgument outputArgument = new OutputArgument();
      Function.Call((Hash) -5429865580393173087L, new InputArgument[1]
      {
        (InputArgument) outputArgument
      });
      return (int) outputArgument.GetResult<int>();
    }
  }
}
