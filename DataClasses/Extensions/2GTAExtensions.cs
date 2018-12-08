using GTA;
using GTA.Native;

namespace ZombiesMod.Extensions
{
  public static class VehicleExtended
  {
    public static VehicleClass GetModelClass(Model vehicleModel)
    {
      return (VehicleClass) Function.Call<int>((Hash) -2387157890592136704L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Model) ref vehicleModel).get_Hash())
      });
    }
  }
}
