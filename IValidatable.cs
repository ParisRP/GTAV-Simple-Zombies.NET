using System;

namespace ZombiesMod
{
  public interface IValidatable
  {
    Func<bool> Validation { get; set; }
  }
}
