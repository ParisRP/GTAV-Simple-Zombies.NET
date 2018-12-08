using GTA;
using GTA.Math;
using ZombiesMod.Extensions;
using ZombiesMod.Scripts;
using ZombiesMod.Static;

namespace ZombiesMod.DataClasses
{
  public abstract class Survivors
  {
    public static float MaxSpawnDistance = 650f;
    public static float MinSpawnDistance = 400f;
    public static float DeleteRange = 1000f;

    public virtual event Survivors.SurvivorCompletedEvent Completed;

    public Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    public Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    public abstract void Update();

    public abstract void SpawnEntities();

    public abstract void CleanUp();

    public abstract void Abort();

    public void Complete()
    {
      // ISSUE: reference to a compiler-generated field
      Survivors.SurvivorCompletedEvent completed = this.Completed;
      if (completed == null)
        return;
      completed(this);
    }

    public bool IsValidSpawn(Vector3 spawn)
    {
      if ((double) spawn.VDist(this.PlayerPosition) >= (double) Survivors.MinSpawnDistance && !ZombieVehicleSpawner.Instance.IsInvalidZone(spawn))
        return true;
      this.Complete();
      return false;
    }

    public Vector3 GetSpawnPoint()
    {
      Vector3 playerPosition = this.PlayerPosition;
      return World.GetNextPositionOnStreet(((Vector3) ref playerPosition).Around(Survivors.MaxSpawnDistance));
    }

    public delegate void SurvivorCompletedEvent(Survivors survivors);
  }
}
