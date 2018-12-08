using GTA;
using System.IO;

namespace ZombiesMod.Static
{
  public class Config
  {
    public static string VersionId = "1.0.2d";
    public const string ScriptFilePath = "./scripts/";
    public const string IniFilePath = "./scripts/ZombiesMod.ini";
    public const string InventoryFilePath = "./scripts/Inventory.dat";
    public const string MapFilePath = "./scripts/Map.dat";
    public const string VehicleFilePath = "./scripts/Vehicles.dat";
    public const string GuardsFilePath = "./scripts/Guards.dat";

    public static void Check()
    {
      ScriptSettings scriptSettings = ScriptSettings.Load("./scripts/ZombiesMod.ini");
      if (scriptSettings.GetValue("mod", "version_id", "0") == Config.VersionId)
        return;
      if (File.Exists("./scripts/ZombiesMod.ini"))
        File.Delete("./scripts/ZombiesMod.ini");
      if (File.Exists("./scripts/Inventory.dat"))
        File.Delete("./scripts/Inventory.dat");
      UI.Notify(string.Format("Updating Simple Zombies to version ~g~{0}~s~. Overwritting the ", (object) Config.VersionId) + "inventory file since there are new items.");
      scriptSettings.SetValue("mod", "version_id", Config.VersionId);
      scriptSettings.Save();
    }
  }
}
