using GTA;
using GTA.Native;
using System;
using ZombiesMod.Extensions;
using ZombiesMod.Static;
using ZombiesMod.Zombies;
using ZombiesMod.Zombies.ZombieTypes;

namespace ZombiesMod.Scripts
{
  public static class ZombieCreator
  {
    public static bool Runners { get; set; }

    public static ZombiePed InfectPed(Ped ped, int health, bool overrideAsFastZombie = false)
    {
      ped.set_CanPlayGestures(false);
      ped.SetCanPlayAmbientAnims(false);
      ped.SetCanEvasiveDive(false);
      ped.SetPathCanUseLadders(false);
      ped.SetPathCanClimb(false);
      ped.DisablePainAudio(true);
      ped.ApplyDamagePack(0.0f, 1f, DamagePack.BigHitByVehicle);
      ped.ApplyDamagePack(0.0f, 1f, DamagePack.ExplosionMed);
      ped.set_AlwaysDiesOnLowHealth(false);
      ped.SetAlertness(Alertness.Nuetral);
      ped.SetCombatAttributes(CombatAttributes.AlwaysFight, true);
      Function.Call((Hash) 8116279360099375049L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      });
      ped.SetConfigFlag(281, true);
      ped.get_Task().WanderAround(((Entity) ped).get_Position(), ZombiePed.WanderRadius);
      ped.set_AlwaysKeepTask(true);
      ped.set_BlockPermanentEvents(true);
      ((Entity) ped).set_IsPersistent(false);
      ((Entity) ped).get_CurrentBlip()?.Remove();
      ((Entity) ped).set_IsPersistent(true);
      ped.set_RelationshipGroup(Relationships.InfectedRelationship);
      float num1 = 0.055f;
      if (ZombieCreator.IsNightFall())
        num1 = 0.5f;
      TimeSpan currentDayTime = World.get_CurrentDayTime();
      if (currentDayTime.Hours >= 20 || currentDayTime.Hours <= 3)
        num1 = 0.4f;
      if (Database.Random.NextDouble() < (double) num1 | overrideAsFastZombie && ZombieCreator.Runners)
        return (ZombiePed) new Runner(((Entity) ped).get_Handle());
      Ped ped1 = ped;
      int num2;
      ((Entity) ped).set_MaxHealth(num2 = health);
      int num3 = num2;
      ((Entity) ped1).set_Health(num3);
      return (ZombiePed) new Walker(((Entity) ped).get_Handle());
    }

    public static bool IsNightFall()
    {
      if (!ZombieCreator.Runners)
        return false;
      TimeSpan currentDayTime = World.get_CurrentDayTime();
      return currentDayTime.Hours >= 20 || currentDayTime.Hours <= 3;
    }
  }
}
