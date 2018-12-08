using GTA;
using GTA.Math;

namespace ZombiesMod.Extensions
{
  public static class V3Extended
  {
    public static bool IsOnScreen(this Vector3 vector3)
    {
      Vector3 position = GameplayCamera.get_Position();
      Vector3 direction = GameplayCamera.get_Direction();
      float fieldOfView = GameplayCamera.get_FieldOfView();
      return (double) Vector3.Angle(Vector3.op_Subtraction(vector3, position), direction) < (double) fieldOfView;
    }
  }
}
