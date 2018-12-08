using GTA.Math;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class SystemExtended
  {
    public static float VDist(this Vector3 v, Vector3 to)
    {
      return (float) Function.Call<float>((Hash) 3046839180162419877L, new InputArgument[6]
      {
        InputArgument.op_Implicit((float) v.X),
        InputArgument.op_Implicit((float) v.Y),
        InputArgument.op_Implicit((float) v.Z),
        InputArgument.op_Implicit((float) to.X),
        InputArgument.op_Implicit((float) to.Y),
        InputArgument.op_Implicit((float) to.Z)
      });
    }
  }
}
