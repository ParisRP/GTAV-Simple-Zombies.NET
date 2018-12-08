using GTA.Math;
using GTA.Native;
using ZombiesMod.DataClasses;
using ZombiesMod.Static;

namespace ZombiesMod.Extensions
{
  public static class WorldExtended
  {
    public static void SetParkedVehicleDensityMultiplierThisFrame(float multiplier)
    {
      Function.Call((Hash) -1520285072926450915L, new InputArgument[1]
      {
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void SetVehicleDensityMultiplierThisFrame(float multiplier)
    {
      Function.Call((Hash) 2619521048766764343L, new InputArgument[1]
      {
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void SetRandomVehicleDensityMultiplierThisFrame(float multiplier)
    {
      Function.Call((Hash) -5497991812566059053L, new InputArgument[1]
      {
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void SetPedDensityThisMultiplierFrame(float multiplier)
    {
      Function.Call((Hash) -7646032285877768974L, new InputArgument[1]
      {
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void SetScenarioPedDensityThisMultiplierFrame(float multiplier)
    {
      Function.Call((Hash) 8815058788752046232L, new InputArgument[1]
      {
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void RemoveAllShockingEvents(bool toggle)
    {
      Function.Call((Hash) -1536878670296045748L, new InputArgument[1]
      {
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void SetFrontendRadioActive(bool active)
    {
      Function.Call((Hash) -580282179644691528L, new InputArgument[1]
      {
        InputArgument.op_Implicit(active ? 1 : 0)
      });
    }

    public static void ClearCops(float radius = 9000f)
    {
      Vector3 playerPosition = Database.PlayerPosition;
      Function.Call((Hash) 358313864965191821L, new InputArgument[5]
      {
        InputArgument.op_Implicit((float) playerPosition.X),
        InputArgument.op_Implicit((float) playerPosition.Y),
        InputArgument.op_Implicit((float) playerPosition.Z),
        InputArgument.op_Implicit(radius),
        InputArgument.op_Implicit(0)
      });
    }

    public static void ClearAreaOfEverything(Vector3 position, float radius)
    {
      Function.Call((Hash) -7676323257878064851L, new InputArgument[8]
      {
        InputArgument.op_Implicit((float) position.X),
        InputArgument.op_Implicit((float) position.Y),
        InputArgument.op_Implicit((float) position.Z),
        InputArgument.op_Implicit(radius),
        InputArgument.op_Implicit(false),
        InputArgument.op_Implicit(false),
        InputArgument.op_Implicit(false),
        InputArgument.op_Implicit(false)
      });
    }

    public static ParticleEffect CreateParticleEffectAtCoord(Vector3 coord, string name)
    {
      Function.Call((Hash) 7798175403732277905L, new InputArgument[1]
      {
        InputArgument.op_Implicit("core")
      });
      return new ParticleEffect((int) Function.Call<int>((Hash) -2196361402923806489L, new InputArgument[12]
      {
        InputArgument.op_Implicit(name),
        InputArgument.op_Implicit((float) coord.X),
        InputArgument.op_Implicit((float) coord.Y),
        InputArgument.op_Implicit((float) coord.Z),
        InputArgument.op_Implicit(0.0),
        InputArgument.op_Implicit(0.0),
        InputArgument.op_Implicit(0.0),
        InputArgument.op_Implicit(1f),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      }));
    }
  }
}
