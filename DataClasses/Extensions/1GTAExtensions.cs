using GTA;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class EntityExtended
  {
    public static bool IsPlayingAnim(this Entity entity, string animSet, string animName)
    {
      return (bool) Function.Call<bool>((Hash) 2237014829242392265L, new InputArgument[4]
      {
        InputArgument.op_Implicit(entity.get_Handle()),
        InputArgument.op_Implicit(animSet),
        InputArgument.op_Implicit(animName),
        InputArgument.op_Implicit(3)
      });
    }

    public static void Fade(this Entity entity, bool state)
    {
      Function.Call((Hash) 2255972746681902637L, new InputArgument[2]
      {
        InputArgument.op_Implicit(entity.get_Handle()),
        InputArgument.op_Implicit(state ? 1 : 0)
      });
    }

    public static bool HasClearLineOfSight(this Entity entity, Entity target, float visionDistance)
    {
      return Function.Call<bool>((Hash) 173335856089985402L, new InputArgument[2]
      {
        InputArgument.op_Implicit(entity.get_Handle()),
        InputArgument.op_Implicit(target.get_Handle())
      }) != null && (double) entity.get_Position().VDist(target.get_Position()) < (double) visionDistance;
    }
  }
}
