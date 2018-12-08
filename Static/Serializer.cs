using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZombiesMod.Static
{
  public static class Serializer
  {
    public static T Deserialize<T>(string path)
    {
      T obj = default (T);
      if (!File.Exists(path))
        return obj;
      try
      {
        FileStream fileStream = new FileStream(path, FileMode.Open);
        obj = (T) new BinaryFormatter().Deserialize((Stream) fileStream);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        File.WriteAllText("./scripts/ZombiesModCrashLog.txt", string.Format("\n[{0}] {1}", (object) DateTime.UtcNow.ToShortDateString(), (object) ex.Message));
      }
      return obj;
    }

    public static void Serialize<T>(string path, T obj)
    {
      try
      {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        new BinaryFormatter().Serialize((Stream) fileStream, (object) obj);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        File.WriteAllText("./scripts/ZombiesModCrashLog.txt", string.Format("\n[{0}] {1}", (object) DateTime.UtcNow.ToShortDateString(), (object) ex.Message));
      }
    }
  }
}
