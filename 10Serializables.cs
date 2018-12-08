using System;

namespace ZombiesMod
{
  [Serializable]
  public class Stat
  {
    public string Name { get; set; }

    public float Value { get; set; }

    public float MaxVal { get; set; }

    public bool Sustained { get; set; }
  }
}
