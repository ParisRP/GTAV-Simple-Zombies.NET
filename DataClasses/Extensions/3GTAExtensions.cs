using GTA;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class PlayerExtended
  {
    public static void IgnoreLowPriorityShockingEvents(this Player player, bool toggle)
    {
      Function.Call((Hash) 6442811240944981760L, new InputArgument[2]
      {
        InputArgument.op_Implicit(player.get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }
  }
}
