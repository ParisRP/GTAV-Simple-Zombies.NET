using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class ScriptExtended
  {
    public static void TerminateScriptByName(string name)
    {
      if (Function.Call<bool>((Hash) -286976521679683174L, new InputArgument[1]
      {
        InputArgument.op_Implicit(name)
      }) == 0)
        return;
      Function.Call((Hash) -7077668788463384353L, new InputArgument[1]
      {
        InputArgument.op_Implicit(name)
      });
    }
  }
}
