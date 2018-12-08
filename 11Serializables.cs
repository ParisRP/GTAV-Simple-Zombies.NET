using System;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class Stats
  {
    public List<Stat> StatList;

    public Stats()
    {
      this.StatList = new List<Stat>()
      {
        new Stat() { Name = "Hunger", Value = 1f, MaxVal = 1f },
        new Stat() { Name = "Thirst", Value = 1f, MaxVal = 1f },
        new Stat() { Name = "Stamina", Value = 1f, MaxVal = 1f }
      };
    }
  }
}
