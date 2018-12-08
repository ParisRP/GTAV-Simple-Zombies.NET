using GTA;
using GTA.Native;
using System.Drawing;

namespace ZombiesMod.DataClasses
{
  public class ParticleEffect : IHandleable, IDeletable
  {
    internal ParticleEffect(int handle)
    {
      this.Handle = handle;
    }

    public int Handle { get; }

    public Color Color
    {
      set
      {
        Function.Call((Hash) 9191676997121112123L, new InputArgument[5]
        {
          InputArgument.op_Implicit(this.Handle),
          InputArgument.op_Implicit(value.R),
          InputArgument.op_Implicit(value.G),
          InputArgument.op_Implicit(value.B),
          InputArgument.op_Implicit(true)
        });
      }
    }

    public bool Exists()
    {
      return (bool) Function.Call<bool>((Hash) 8408201869211353243L, new InputArgument[1]
      {
        InputArgument.op_Implicit(this.Handle)
      });
    }

    public void Delete()
    {
      Function.Call((Hash) -4323085940105063473L, new InputArgument[2]
      {
        InputArgument.op_Implicit(this.Handle),
        InputArgument.op_Implicit(1)
      });
    }
  }
}
