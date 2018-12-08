using GTA;
using System;
using System.Collections.Generic;
using ZombiesMod.DataClasses;
using ZombiesMod.Static;
using ZombiesMod.SurvivorTypes;

namespace ZombiesMod.Controllers
{
  public class SurvivorController : Script, ISpawner
  {
    private readonly List<Survivors> _survivors;
    private readonly int _survivorInterval;
    private readonly float _survivorSpawnChance;
    private readonly int _merryweatherTimeout;
    private DateTime _currentDelayTime;

    public event SurvivorController.OnCreatedSurvivorsEvent CreatedSurvivors;

    public SurvivorController()
    {
      base.\u002Ector();
      SurvivorController.Instance = this;
      this._survivorInterval = (int) this.get_Settings().GetValue<int>("survivors", "survivor_interval", (M0) this._survivorInterval);
      this._survivorSpawnChance = (float) this.get_Settings().GetValue<float>("survivors", "survivor_spawn_chance", (M0) (double) this._survivorSpawnChance);
      this._merryweatherTimeout = (int) this.get_Settings().GetValue<int>("survivors", "merryweather_timeout", (M0) this._merryweatherTimeout);
      this.get_Settings().SetValue<int>("survivors", "survivor_interval", (M0) this._survivorInterval);
      this.get_Settings().SetValue<float>("survivors", "survivor_spawn_chance", (M0) (double) this._survivorSpawnChance);
      this.get_Settings().SetValue<int>("survivors", "merryweather_timeout", (M0) this._merryweatherTimeout);
      this.get_Settings().Save();
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted((EventHandler) ((sender, args) => this._survivors.ForEach((Action<Survivors>) (s => s.Abort()))));
      this.CreatedSurvivors += new SurvivorController.OnCreatedSurvivorsEvent(this.OnCreatedSurvivors);
    }

    private void OnCreatedSurvivors()
    {
      bool flag = Database.Random.NextDouble() <= (double) this._survivorSpawnChance;
      EventTypes[] values = (EventTypes[]) Enum.GetValues(typeof (EventTypes));
      EventTypes eventTypes = values[Database.Random.Next(values.Length)];
      Survivors survivors1 = (Survivors) null;
      switch (eventTypes)
      {
        case EventTypes.Friendly:
          survivors1 = this.TryCreateEvent((Survivors) new FriendlySurvivors());
          break;
        case EventTypes.Hostile:
          if (Database.Random.NextDouble() <= 0.200000002980232)
          {
            survivors1 = this.TryCreateEvent((Survivors) new HostileSurvivors());
            break;
          }
          break;
        case EventTypes.Merryweather:
          survivors1 = this.TryCreateEvent((Survivors) new MerryweatherSurvivors(this._merryweatherTimeout));
          break;
      }
      if (survivors1 == null)
        return;
      this._survivors.Add(survivors1);
      survivors1.SpawnEntities();
      survivors1.Completed += (Survivors.SurvivorCompletedEvent) (survivors =>
      {
        this.SetDelayTime();
        survivors.CleanUp();
        this._survivors.Remove(survivors);
      });
    }

    private Survivors TryCreateEvent(Survivors survivors)
    {
      Survivors survivors1 = (Survivors) null;
      if (this._survivors.FindIndex((Predicate<Survivors>) (s => Type.op_Equality(s.GetType(), survivors.GetType()))) <= -1)
        survivors1 = survivors;
      return survivors1;
    }

    public bool Spawn { get; set; }

    public static SurvivorController Instance { get; private set; }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      this.Create();
      this.Destroy();
      this._survivors.ForEach((Action<Survivors>) (s => s.Update()));
    }

    private void Destroy()
    {
      if (this.Spawn)
        return;
      this._survivors.ForEach((Action<Survivors>) (s =>
      {
        this._survivors.Remove(s);
        s.Abort();
      }));
      this._currentDelayTime = DateTime.UtcNow;
    }

    private void Create()
    {
      if (!this.Spawn || DateTime.UtcNow <= this._currentDelayTime)
        return;
      // ISSUE: reference to a compiler-generated field
      SurvivorController.OnCreatedSurvivorsEvent createdSurvivors = this.CreatedSurvivors;
      if (createdSurvivors != null)
        createdSurvivors();
      this.SetDelayTime();
    }

    private void SetDelayTime()
    {
      this._currentDelayTime = DateTime.UtcNow + new TimeSpan(0, 0, this._survivorInterval);
    }

    public delegate void OnCreatedSurvivorsEvent();
  }
}
