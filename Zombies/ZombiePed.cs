using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.Scripts;
using ZombiesMod.Static;
using ZombiesMod.Wrappers;

namespace ZombiesMod.Zombies
{
  public abstract class ZombiePed : Entity, IEquatable<Ped>
  {
    public static int ZombieDamage = 15;
    public static float SensingRange = 120f;
    public static float SilencerEffectiveRange = 15f;
    public static float BehindZombieNoticeDistance = 5f;
    public static float RunningNoticeDistance = 25f;
    public static float AttackRange = 1.2f;
    public static float VisionDistance = 35f;
    public static float WanderRadius = 100f;
    public const int MovementUpdateInterval = 5;
    private Ped _target;
    private readonly Ped _ped;
    private EntityEventWrapper _eventWrapper;
    private bool _goingToTarget;
    private bool _attackingTarget;
    private DateTime _currentMovementUpdateTime;

    public event ZombiePed.OnGoingToTargetEvent GoToTarget;

    public event ZombiePed.OnAttackingTargetEvent AttackTarget;

    protected ZombiePed(int handle)
    {
      base.\u002Ector(handle);
      this._ped = new Ped(handle);
      this._eventWrapper = new EntityEventWrapper((Entity) this._ped);
      this._eventWrapper.Died += new EntityEventWrapper.OnDeathEvent(this.OnDied);
      this._eventWrapper.Updated += new EntityEventWrapper.OnWrapperUpdateEvent(this.Update);
      this._eventWrapper.Aborted += new EntityEventWrapper.OnWrapperAbortedEvent(this.Abort);
      this._currentMovementUpdateTime = DateTime.UtcNow;
      this.GoToTarget += new ZombiePed.OnGoingToTargetEvent(this.OnGoToTarget);
      this.AttackTarget += new ZombiePed.OnAttackingTargetEvent(this.OnAttackTarget);
    }

    public virtual bool PlayAudio { get; set; }

    public Ped Target
    {
      get
      {
        return this._target;
      }
      private set
      {
        if (Entity.op_Equality((Entity) value, (Entity) null) && Entity.op_Inequality((Entity) this._target, (Entity) null))
        {
          this._ped.get_Task().WanderAround(this.get_Position(), ZombiePed.WanderRadius);
          this.GoingToTarget = this.AttackingTarget = false;
        }
        this._target = value;
      }
    }

    public bool GoingToTarget
    {
      get
      {
        return this._goingToTarget;
      }
      set
      {
        if (value && !this._goingToTarget)
        {
          ZombiePed.OnGoingToTargetEvent goToTarget = this.GoToTarget;
          if (goToTarget != null)
            goToTarget(this.Target);
        }
        this._goingToTarget = value;
      }
    }

    public bool AttackingTarget
    {
      get
      {
        return this._attackingTarget;
      }
      set
      {
        if (value && !this._ped.get_IsRagdoll() && (!this.get_IsDead() && !this._ped.get_IsClimbing()) && (!this._ped.get_IsFalling() && !this._ped.get_IsBeingStunned()) && !this._ped.get_IsGettingUp())
        {
          ZombiePed.OnAttackingTargetEvent attackTarget = this.AttackTarget;
          if (attackTarget != null)
            attackTarget(this.Target);
        }
        this._attackingTarget = value;
      }
    }

    public virtual string MovementStyle { get; set; }

    public abstract void OnAttackTarget(Ped target);

    public abstract void OnGoToTarget(Ped target);

    private void OnDied(EntityEventWrapper sender, Entity entity)
    {
      this.get_CurrentBlip()?.Remove();
      if (!ZombieVehicleSpawner.Instance.IsInvalidZone(entity.get_Position()) || !ZombieVehicleSpawner.Instance.IsValidSpawn(entity.get_Position()))
        return;
      ZombieVehicleSpawner.Instance.SpawnBlocker.Add(entity.get_Position());
    }

    public void Update(EntityEventWrapper entityEventWrapper, Entity entity)
    {
      if ((double) this.get_Position().VDist(Database.PlayerPosition) > 120.0 && (!this.get_IsOnScreen() || this.get_IsDead()))
        this.Delete();
      if (this.PlayAudio && this._ped.get_IsRunning())
      {
        this._ped.DisablePainAudio(false);
        this._ped.PlayPain(8);
        this._ped.PlayFacialAnim("facials@gen_male@base", "burning_1");
      }
      this.GetTarget();
      this.SetWalkStyle();
      if (((Entity) this._ped).get_IsOnFire() && !((Entity) this._ped).get_IsDead())
        this._ped.Kill();
      this._ped.StopAmbientSpeechThisFrame();
      if (!this.PlayAudio)
        this._ped.StopSpeaking(true);
      if (Entity.op_Equality((Entity) this.Target, (Entity) null))
        return;
      if ((double) this.get_Position().VDist(((Entity) this.Target).get_Position()) > (double) ZombiePed.AttackRange)
      {
        this.AttackingTarget = false;
        this.GoingToTarget = true;
      }
      else
      {
        this.AttackingTarget = true;
        this.GoingToTarget = false;
      }
    }

    public void Abort(EntityEventWrapper sender, Entity entity)
    {
      this.Delete();
    }

    public void InfectTarget(Ped target)
    {
      if (target.get_IsPlayer() || ((Entity) target).get_Health() > ((Entity) target).get_MaxHealth() / 4)
        return;
      target.SetToRagdoll(3000);
      ZombieCreator.InfectPed(target, this.get_MaxHealth(), true);
      this.ForgetTarget();
      target.LeaveGroup();
      target.get_Weapons().Drop();
      EntityEventWrapper.Dispose((Entity) target);
    }

    public void ForgetTarget()
    {
      this._target = (Ped) null;
    }

    private void SetWalkStyle()
    {
      if (DateTime.UtcNow <= this._currentMovementUpdateTime)
        return;
      this._ped.SetMovementAnimSet(this.MovementStyle);
      this.UpdateTime();
    }

    private void UpdateTime()
    {
      this._currentMovementUpdateTime = DateTime.UtcNow + new TimeSpan(0, 0, 0, 5);
    }

    private void GetTarget()
    {
      // ISSUE: method pointer
      Ped closest = (Ped) World.GetClosest<Ped>(this.get_Position(), (M0[]) ((IEnumerable<Ped>) Enumerable.Where<Ped>((IEnumerable<M0>) World.GetNearbyPeds(this._ped, ZombiePed.SensingRange), (Func<M0, bool>) new Func<Ped, bool>((object) this, __methodptr(IsGoodTarget)))).ToArray<Ped>());
      if (Entity.op_Inequality((Entity) closest, (Entity) null) && (((Entity) this._ped).HasClearLineOfSight((Entity) closest, ZombiePed.VisionDistance) || this.CanHearPed(closest)))
      {
        this.Target = closest;
      }
      else
      {
        if ((!Entity.op_Inequality((Entity) this.Target, (Entity) null) || this.IsGoodTarget(this.Target)) && !Entity.op_Inequality((Entity) closest, (Entity) this.Target))
          return;
        this.Target = (Ped) null;
      }
    }

    private bool CanHearPed(Ped ped)
    {
      float distance = ((Entity) ped).get_Position().VDist(this.get_Position());
      return !ZombiePed.IsWeaponWellSilenced(ped, distance) || ZombiePed.IsBehindZombie(distance) || ZombiePed.IsRunningNoticed(ped, distance);
    }

    private static bool IsRunningNoticed(Ped ped, float distance)
    {
      return ped.get_IsSprinting() && (double) distance < (double) ZombiePed.RunningNoticeDistance;
    }

    private static bool IsBehindZombie(float distance)
    {
      return (double) distance < (double) ZombiePed.BehindZombieNoticeDistance;
    }

    private static bool IsWeaponWellSilenced(Ped ped, float distance)
    {
      if (!ped.get_IsShooting())
        return true;
      return ped.IsCurrentWeaponSileced() && (double) distance > (double) ZombiePed.SilencerEffectiveRange;
    }

    private bool IsGoodTarget(Ped ped)
    {
      return ped.GetRelationshipWithPed(this._ped) == 5;
    }

    protected bool Equals(ZombiePed other)
    {
      return this.Equals((Entity) other) && object.Equals((object) this._ped, (object) other._ped);
    }

    public virtual bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return Type.op_Equality(obj.GetType(), ((object) this).GetType()) && this.Equals((ZombiePed) obj);
    }

    public virtual int GetHashCode()
    {
      return base.GetHashCode() * 397 ^ (Entity.op_Inequality((Entity) this._ped, (Entity) null) ? ((object) this._ped).GetHashCode() : 0);
    }

    public bool Equals(Ped other)
    {
      return object.Equals((object) this._ped, (object) other);
    }

    public static implicit operator Ped(ZombiePed v)
    {
      return v._ped;
    }

    public delegate void OnGoingToTargetEvent(Ped target);

    public delegate void OnAttackingTargetEvent(Ped target);
  }
}
