using GTA;
using System;
using System.Collections.Generic;
using ZombiesMod.Scripts;

namespace ZombiesMod.Wrappers
{
  public class EntityEventWrapper
  {
    private static readonly List<EntityEventWrapper> Wrappers = new List<EntityEventWrapper>();
    private bool _isDead;

    public event EntityEventWrapper.OnDeathEvent Died;

    public event EntityEventWrapper.OnWrapperAbortedEvent Aborted;

    public event EntityEventWrapper.OnWrapperUpdateEvent Updated;

    public event EntityEventWrapper.OnWrapperDisposedEvent Disposed;

    public EntityEventWrapper(Entity ent)
    {
      this.Entity = ent;
      ScriptEventHandler.Instance.RegisterWrapper(new EventHandler(this.OnTick));
      ScriptEventHandler.Instance.add_Aborted((EventHandler) ((sender, args) => this.Abort()));
      EntityEventWrapper.Wrappers.Add(this);
    }

    public Entity Entity { get; }

    public bool IsDead
    {
      get
      {
        return this.Entity.get_IsDead();
      }
      private set
      {
        if (value && !this._isDead)
        {
          EntityEventWrapper.OnDeathEvent died = this.Died;
          if (died != null)
            died(this, this.Entity);
        }
        this._isDead = value;
      }
    }

    public void OnTick(object sender, EventArgs eventArgs)
    {
      if (Entity.op_Equality(this.Entity, (Entity) null) || !this.Entity.Exists())
      {
        this.Dispose();
      }
      else
      {
        this.IsDead = this.Entity.get_IsDead();
        // ISSUE: reference to a compiler-generated field
        EntityEventWrapper.OnWrapperUpdateEvent updated = this.Updated;
        if (updated == null)
          return;
        updated(this, this.Entity);
      }
    }

    public void Abort()
    {
      // ISSUE: reference to a compiler-generated field
      EntityEventWrapper.OnWrapperAbortedEvent aborted = this.Aborted;
      if (aborted == null)
        return;
      aborted(this, this.Entity);
    }

    public void Dispose()
    {
      ScriptEventHandler.Instance.UnregisterWrapper(new EventHandler(this.OnTick));
      EntityEventWrapper.Wrappers.Remove(this);
      // ISSUE: reference to a compiler-generated field
      EntityEventWrapper.OnWrapperDisposedEvent disposed = this.Disposed;
      if (disposed == null)
        return;
      disposed(this, this.Entity);
    }

    public static void Dispose(Entity entity)
    {
      EntityEventWrapper entityEventWrapper = EntityEventWrapper.Wrappers.Find((Predicate<EntityEventWrapper>) (w => Entity.op_Equality(w.Entity, entity)));
      entityEventWrapper?.Dispose();
      EntityEventWrapper.Wrappers.Remove(entityEventWrapper);
    }

    public delegate void OnDeathEvent(EntityEventWrapper sender, Entity entity);

    public delegate void OnWrapperAbortedEvent(EntityEventWrapper sender, Entity entity);

    public delegate void OnWrapperUpdateEvent(EntityEventWrapper sender, Entity entity);

    public delegate void OnWrapperDisposedEvent(EntityEventWrapper sender, Entity entity);
  }
}
