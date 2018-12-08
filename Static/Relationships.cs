using GTA;

namespace ZombiesMod.Static
{
  public class Relationships
  {
    public static int InfectedRelationship;
    public static int FriendlyRelationship;
    public static int MilitiaRelationship;
    public static int HostileRelationship;
    public static int PlayerRelationship;

    public static void SetRelationships()
    {
      Relationships.InfectedRelationship = World.AddRelationshipGroup("Zombie");
      Relationships.FriendlyRelationship = World.AddRelationshipGroup("Friendly");
      Relationships.MilitiaRelationship = World.AddRelationshipGroup("Private_Militia");
      Relationships.HostileRelationship = World.AddRelationshipGroup("Hostile");
      Relationships.PlayerRelationship = Database.PlayerPed.get_RelationshipGroup();
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.InfectedRelationship, Relationships.FriendlyRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.InfectedRelationship, Relationships.MilitiaRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.InfectedRelationship, Relationships.HostileRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.InfectedRelationship, Relationships.PlayerRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.FriendlyRelationship, Relationships.MilitiaRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.FriendlyRelationship, Relationships.HostileRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.HostileRelationship, Relationships.MilitiaRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.HostileRelationship, Relationships.PlayerRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.PlayerRelationship, Relationships.MilitiaRelationship);
      Relationships.SetRelationshipBothWays((Relationship) 2, Relationships.PlayerRelationship, Relationships.FriendlyRelationship);
      Database.PlayerPed.set_IsPriorityTargetForEnemies(true);
    }

    public static void SetRelationshipBothWays(Relationship rel, int group1, int group2)
    {
      World.SetRelationshipBetweenGroups(rel, group1, group2);
      World.SetRelationshipBetweenGroups(rel, group2, group1);
    }
  }
}
