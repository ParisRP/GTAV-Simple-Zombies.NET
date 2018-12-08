using System;

namespace ZombiesMod.Extensions
{
  internal static class RandomExtensions
  {
    public static void Shuffle<T>(this Random rng, T[] array)
    {
      int length = array.Length;
      while (length > 1)
      {
        int index = rng.Next(length--);
        T obj = array[length];
        array[length] = array[index];
        array[index] = obj;
      }
    }
  }
}
