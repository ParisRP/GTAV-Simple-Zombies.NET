using GTA;
using ZombiesMod.Extensions;

namespace ZombiesMod.Zombies.ZombieTypes
{
  public class Walker : ZombiePed
  {
    private readonly Ped _ped;

    public Walker(int handle)
      : base(handle)
    {
      this._ped = (Ped) ((ZombiePed) this);
    }

    public override string MovementStyle { get; set; } = "move_m@drunk@verydrunk";

    public override void OnAttackTarget(Ped target)
    {
      if (((Entity) target).get_IsDead())
      {
        if (((Entity) this._ped).IsPlayingAnim("amb@world_human_bum_wash@male@high@idle_a", "idle_b"))
          return;
        this._ped.get_Task().PlayAnimation("amb@world_human_bum_wash@male@high@idle_a", "idle_b", 8f, -1, (AnimationFlags) 1);
      }
      else
      {
        if (((Entity) this._ped).IsPlayingAnim("rcmbarry", "bar_1_teleport_aln"))
          return;
        this._ped.get_Task().PlayAnimation("rcmbarry", "bar_1_teleport_aln", 8f, 1000, (AnimationFlags) 16);
        if (!((Entity) target).get_IsInvincible())
          target.ApplyDamage(ZombiePed.ZombieDamage);
        this.InfectTarget(target);
      }
    }

    public override void OnGoToTarget(Ped target)
    {
      this._ped.get_Task().GoTo((Entity) target);
    }
  }
}
