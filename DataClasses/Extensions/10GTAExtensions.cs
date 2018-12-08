using GTA;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class PropExt
  {
    public static void SetStateOfDoor(this Prop prop, bool locked, DoorState heading)
    {
      long num = -563637040166458307;
      InputArgument[] inputArgumentArray = new InputArgument[7];
      int index = 0;
      Model model = ((Entity) prop).get_Model();
      InputArgument inputArgument = InputArgument.op_Implicit(((Model) ref model).get_Hash());
      inputArgumentArray[index] = inputArgument;
      inputArgumentArray[1] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().X);
      inputArgumentArray[2] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().Y);
      inputArgumentArray[3] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().Z);
      inputArgumentArray[4] = InputArgument.op_Implicit(locked);
      inputArgumentArray[5] = InputArgument.op_Implicit((int) heading);
      inputArgumentArray[6] = InputArgument.op_Implicit(1);
      Function.Call((Hash) num, inputArgumentArray);
    }

    public static unsafe bool GetDoorLockState(this Prop prop)
    {
      bool flag = false;
      int num1 = 0;
      long num2 = -1314587405265718273;
      InputArgument[] inputArgumentArray = new InputArgument[6];
      int index = 0;
      Model model = ((Entity) prop).get_Model();
      InputArgument inputArgument = InputArgument.op_Implicit(((Model) ref model).get_Hash());
      inputArgumentArray[index] = inputArgument;
      inputArgumentArray[1] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().X);
      inputArgumentArray[2] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().Y);
      inputArgumentArray[3] = InputArgument.op_Implicit((float) ((Entity) prop).get_Position().Z);
      inputArgumentArray[4] = InputArgument.op_Implicit(&flag);
      inputArgumentArray[5] = InputArgument.op_Implicit(&num1);
      Function.Call((Hash) num2, inputArgumentArray);
      return flag;
    }
  }
}
