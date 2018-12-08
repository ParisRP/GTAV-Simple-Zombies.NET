using GTA;
using GTA.Math;
using GTA.Native;
using System;
using ZombiesMod.Extensions;
using ZombiesMod.PlayerManagement;
using ZombiesMod.Static;

namespace ZombiesMod.Scripts
{
  public class VehicleRepair : Script
  {
    private Vehicle _selectedVehicle;
    private InventoryItemBase _item;
    private readonly int _repairTimeMs;

    public VehicleRepair()
    {
      base.\u002Ector();
      this._repairTimeMs = (int) this.get_Settings().GetValue<int>("interaction", "vehicle_repair_time_ms", (M0) this._repairTimeMs);
      this.get_Settings().SetValue<int>("interaction", "vehicle_repair_time_ms", (M0) this._repairTimeMs);
      this.get_Settings().Save();
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted(new EventHandler(VehicleRepair.OnAborted));
    }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    private static void OnAborted(object sender, EventArgs e)
    {
      VehicleRepair.PlayerPed.get_Task().ClearAll();
    }

    private void OnTick(object sender, EventArgs e)
    {
      if (Database.PlayerInVehicle)
        return;
      Vehicle closestVehicle = World.GetClosestVehicle(Database.PlayerPosition, 20f);
      if (this._item == null)
        this._item = PlayerInventory.Instance.ItemFromName("Vehicle Repair Kit");
      if (Entity.op_Inequality((Entity) this._selectedVehicle, (Entity) null))
      {
        Game.DisableControlThisFrame(2, (Control) 24);
        UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to cancel.", false);
        if (Game.IsDisabledControlJustPressed(2, (Control) 24))
        {
          VehicleRepair.PlayerPed.get_Task().ClearAllImmediately();
          this._selectedVehicle.CloseDoor((VehicleDoor) 4, false);
          this._selectedVehicle = (Vehicle) null;
        }
        else
        {
          if (VehicleRepair.PlayerPed.get_TaskSequenceProgress() != -1)
            return;
          this._selectedVehicle.set_EngineHealth(1000f);
          this._selectedVehicle.CloseDoor((VehicleDoor) 4, false);
          this._selectedVehicle = (Vehicle) null;
          PlayerInventory.Instance.AddItem(this._item, -1, ItemType.Item);
          UI.Notify("Items: -~r~1");
        }
      }
      else
      {
        if (!Entity.op_Inequality((Entity) closestVehicle, (Entity) null))
          return;
        Model model = ((Entity) closestVehicle).get_Model();
        if (!((Model) ref model).get_IsCar() || (double) closestVehicle.get_EngineHealth() >= 1000.0 || MenuConrtoller.MenuPool.IsAnyMenuOpen() || ((Entity) closestVehicle).get_IsUpsideDown() || !((Entity) closestVehicle).HasBone("engine"))
          return;
        Vector3 boneCoord = ((Entity) closestVehicle).GetBoneCoord(((Entity) closestVehicle).GetBoneIndex("engine"));
        if (Vector3.op_Equality(boneCoord, Vector3.get_Zero()) || !((Entity) VehicleRepair.PlayerPed).IsInRangeOf(boneCoord, 1.5f))
          return;
        if (!PlayerInventory.Instance.HasItem(this._item, ItemType.Item))
        {
          UiExtended.DisplayHelpTextThisFrame("You need a vehicle repair kit to fix this engine.", false);
        }
        else
        {
          Game.DisableControlThisFrame(2, (Control) 51);
          UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to repair engine.", false);
          if (!Game.IsDisabledControlJustPressed(2, (Control) 51))
            return;
          closestVehicle.OpenDoor((VehicleDoor) 4, false, false);
          VehicleRepair.PlayerPed.get_Weapons().Select((WeaponHash) -1569615261, true);
          Vector3 vector3_1 = Vector3.op_Addition(boneCoord, ((Entity) closestVehicle).get_ForwardVector());
          Vector3 vector3_2 = Vector3.op_Subtraction(((Entity) closestVehicle).get_Position(), Database.PlayerPosition);
          float heading = ((Vector3) ref vector3_2).ToHeading();
          TaskSequence taskSequence = new TaskSequence();
          taskSequence.get_AddTask().ClearAllImmediately();
          taskSequence.get_AddTask().GoTo(vector3_1, false, 1500);
          taskSequence.get_AddTask().AchieveHeading(heading, 2000);
          taskSequence.get_AddTask().PlayAnimation("mp_intro_seq@", "mp_mech_fix", 8f, -8f, this._repairTimeMs, (AnimationFlags) 1, 1f);
          taskSequence.get_AddTask().ClearAll();
          taskSequence.Close();
          VehicleRepair.PlayerPed.get_Task().PerformSequence(taskSequence);
          taskSequence.Dispose();
          this._selectedVehicle = closestVehicle;
        }
      }
    }
  }
}
