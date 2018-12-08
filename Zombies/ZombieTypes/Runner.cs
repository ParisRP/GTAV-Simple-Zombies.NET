using GTA;
using GTA.Math;
using GTA.Native;
using ZombiesMod.Extensions;
using ZombiesMod.Static;

namespace ZombiesMod.Zombies.ZombieTypes
{
  public class Runner : ZombiePed
  {
    private readonly Ped _ped;
    private bool _jumpAttack;

    public Runner(int handle)
      : base(handle)
    {
      this._ped = (Ped) ((ZombiePed) this);
    }

    public override bool PlayAudio { get; set; } = true;

    public override string MovementStyle { get; set; } = "move_m@injured";

    public override void OnAttackTarget(Ped target)
    {
      if (((Entity) target).get_IsDead())
      {
        if (((Entity) this._ped).IsPlayingAnim("amb@world_human_bum_wash@male@high@idle_a", "idle_b"))
          return;
        this._ped.get_Task().PlayAnimation("amb@world_human_bum_wash@male@high@idle_a", "idle_b", 8f, -1, (AnimationFlags) 1);
      }
      else if (Database.Random.NextDouble() < 0.300000011920929 && !this._jumpAttack && !target.get_IsPerformingStealthKill() && !target.get_IsGettingUp() && !target.get_IsRagdoll())
      {
        this._ped.Jump();
        Ped ped = this._ped;
        Vector3 vector3 = Vector3.op_Subtraction(((Entity) target).get_Position(), this.get_Position());
        double heading = (double) ((Vector3) ref vector3).ToHeading();
        ((Entity) ped).set_Heading((float) heading);
        this._jumpAttack = true;
        target.SetToRagdoll(2000);
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
      Function.Call((Hash) 7640095384362883202L, new InputArgument[7]
      {
        InputArgument.op_Implicit(((Entity) this._ped).get_Handle()),
        InputArgument.op_Implicit(((Entity) target).get_Handle()),
        InputArgument.op_Implicit(-1),
        InputArgument.op_Implicit(0.0f),
        InputArgument.op_Implicit(5f),
        InputArgument.op_Implicit(1073741824),
        InputArgument.op_Implicit(0)
      });
    }
  }
}
