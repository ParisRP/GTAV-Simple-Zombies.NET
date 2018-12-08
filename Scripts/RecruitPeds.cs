using GTA;
using GTA.Math;
using System;
using ZombiesMod.Extensions;
using ZombiesMod.Static;
using ZombiesMod.SurvivorTypes;

namespace ZombiesMod.Scripts
{
  public class RecruitPeds : Script
  {
    public const float InteractDistance = 1.5f;

    public RecruitPeds()
    {
      base.\u002Ector();
      this.add_Tick(new EventHandler(RecruitPeds.OnTick));
    }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    private static Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    private static void OnTick(object sender, EventArgs eventArgs)
    {
      if (MenuConrtoller.MenuPool.IsAnyMenuOpen() || RecruitPeds.PlayerPed.get_CurrentPedGroup().get_MemberCount() >= 6)
        return;
      Ped closest = (Ped) World.GetClosest<Ped>(RecruitPeds.PlayerPosition, (M0[]) World.GetNearbyPeds(RecruitPeds.PlayerPed, 1.5f));
      if (Entity.op_Equality((Entity) closest, (Entity) null) || ((Entity) closest).get_IsDead() || (closest.IsInCombatAgainst(RecruitPeds.PlayerPed) || closest.GetRelationshipWithPed(RecruitPeds.PlayerPed) == 5) || (closest.get_RelationshipGroup() != Relationships.FriendlyRelationship || PedGroup.op_Equality(closest.get_CurrentPedGroup(), RecruitPeds.PlayerPed.get_CurrentPedGroup())))
        return;
      Game.DisableControlThisFrame(2, (Control) 23);
      UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_ENTER~ to recruit this ped.", false);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 23))
        return;
      if (FriendlySurvivors.Instance != null)
        FriendlySurvivors.Instance.RemovePed(closest);
      closest.Recruit(RecruitPeds.PlayerPed);
      if (RecruitPeds.PlayerPed.get_CurrentPedGroup().get_MemberCount() < 6)
        return;
      UI.Notify("You've reached the max amount of ~b~guards~s~.");
    }
  }
}
