using GTA.Native;
using System;
using ZombiesMod.Static;

namespace ZombiesMod.Extensions
{
  public static class UiExtended
  {
    public static bool IsAnyHelpTextOnScreen()
    {
      return (bool) Function.Call<bool>((Hash) 5582567543607241831L, new InputArgument[0]);
    }

    public static void ClearAllHelpText()
    {
      Function.Call((Hash) 7023634693725934496L, new InputArgument[0]);
    }

    public static void DisplayHelpTextThisFrame(string helpText, bool ignoreMenus = false)
    {
      if (!ignoreMenus && MenuConrtoller.MenuPool.IsAnyMenuOpen())
        return;
      Function.Call((Hash) -8860350453193909743L, new InputArgument[1]
      {
        InputArgument.op_Implicit("CELL_EMAIL_BCON")
      });
      int startIndex = 0;
      while (startIndex < helpText.Length)
      {
        Function.Call((Hash) 7789129354908300458L, new InputArgument[1]
        {
          InputArgument.op_Implicit(helpText.Substring(startIndex, Math.Min(99, helpText.Length - startIndex)))
        });
        startIndex += 99;
      }
      Function.Call((Hash) 2562546386151446694L, new InputArgument[4]
      {
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(Function.Call<bool>((Hash) 5582567543607241831L, new InputArgument[0]) != null ? 0 : 1),
        InputArgument.op_Implicit(-1)
      });
    }
  }
}
