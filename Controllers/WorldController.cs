using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.Static;

namespace ZombiesMod.Controllers
{
  public class WorldController : Script
  {
    private bool _reset;

    public WorldController()
    {
      base.\u002Ector();
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted(new EventHandler(WorldController.OnAborted));
    }

    public static bool Configure { get; set; }

    public static bool StopPedsFromSpawning { get; set; }

    public Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    private static void OnAborted(object sender, EventArgs e)
    {
      WorldController.Reset();
    }

    private static void Reset()
    {
      Function.Call((Hash) 6837250382999549709L, new InputArgument[1]
      {
        InputArgument.op_Implicit(true)
      });
      Function.Call((Hash) -8916161055282871969L, new InputArgument[1]
      {
        InputArgument.op_Implicit(true)
      });
      Function.Call((Hash) -9162020105814915111L, new InputArgument[1]
      {
        InputArgument.op_Implicit(true)
      });
      Function.Call((Hash) 3097765567273685773L, new InputArgument[1]
      {
        InputArgument.op_Implicit(true)
      });
    }

    private void OnTick(object sender, EventArgs e)
    {
      if (WorldController.Configure)
      {
        WorldExtended.ClearCops(10000f);
        WorldExtended.SetScenarioPedDensityThisMultiplierFrame(0.0f);
        WorldExtended.SetVehicleDensityMultiplierThisFrame(0.0f);
        WorldExtended.SetRandomVehicleDensityMultiplierThisFrame(0.0f);
        WorldExtended.SetParkedVehicleDensityMultiplierThisFrame(0.0f);
        WorldExtended.SetPedDensityThisMultiplierFrame(0.0f);
        WorldExtended.SetScenarioPedDensityThisMultiplierFrame(0.0f);
        Game.set_MaxWantedLevel(0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        Vehicle[] array = ((IEnumerable<Vehicle>) Enumerable.Where<Vehicle>((IEnumerable<M0>) World.GetAllVehicles(), (Func<M0, bool>) (WorldController.\u003C\u003Ec.\u003C\u003E9__14_0 ?? (WorldController.\u003C\u003Ec.\u003C\u003E9__14_0 = new Func<Vehicle, bool>((object) WorldController.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003COnTick\u003Eb__14_0)))))).ToArray<Vehicle>();
        Vehicle[] all1 = Array.FindAll<Vehicle>(array, (Predicate<Vehicle>) (v => v.get_ClassType() == 16));
        Vehicle[] all2 = Array.FindAll<Vehicle>(array, (Predicate<Vehicle>) (v => v.get_ClassType() == 21));
        Array.ForEach<Vehicle>(Array.FindAll<Vehicle>(array, (Predicate<Vehicle>) (v =>
        {
          if (((Entity) v.get_Driver()).Exists())
            return !v.get_Driver().get_IsPlayer();
          return false;
        })), (Action<Vehicle>) (vehicle => ((Entity) vehicle).Delete()));
        Array.ForEach<Vehicle>(all1, (Action<Vehicle>) (plane =>
        {
          if (!((Entity) plane.get_Driver()).Exists() || plane.get_Driver().get_IsPlayer() || ((Entity) plane.get_Driver()).get_IsDead())
            return;
          plane.get_Driver().Kill();
        }));
        Array.ForEach<Vehicle>(all2, (Action<Vehicle>) (t => Function.Call((Hash) -6193635740946557213L, new InputArgument[2]
        {
          InputArgument.op_Implicit(((Entity) t).get_Handle()),
          InputArgument.op_Implicit(0.0f)
        })));
        ScriptExtended.TerminateScriptByName("re_prison");
        ScriptExtended.TerminateScriptByName("am_prison");
        ScriptExtended.TerminateScriptByName("gb_biker_free_prisoner");
        ScriptExtended.TerminateScriptByName("re_prisonvanbreak");
        ScriptExtended.TerminateScriptByName("am_vehicle_spawn");
        ScriptExtended.TerminateScriptByName("am_taxi");
        ScriptExtended.TerminateScriptByName("audiotest");
        ScriptExtended.TerminateScriptByName("freemode");
        ScriptExtended.TerminateScriptByName("re_prisonerlift");
        ScriptExtended.TerminateScriptByName("am_prison");
        ScriptExtended.TerminateScriptByName("re_lossantosintl");
        ScriptExtended.TerminateScriptByName("re_armybase");
        ScriptExtended.TerminateScriptByName("restrictedareas");
        ScriptExtended.TerminateScriptByName("stripclub");
        ScriptExtended.TerminateScriptByName("re_gangfight");
        ScriptExtended.TerminateScriptByName("re_gang_intimidation");
        ScriptExtended.TerminateScriptByName("spawn_activities");
        ScriptExtended.TerminateScriptByName("am_vehiclespawn");
        ScriptExtended.TerminateScriptByName("traffick_air");
        ScriptExtended.TerminateScriptByName("traffick_ground");
        ScriptExtended.TerminateScriptByName("emergencycall");
        ScriptExtended.TerminateScriptByName("emergencycalllauncher");
        ScriptExtended.TerminateScriptByName("clothes_shop_sp");
        ScriptExtended.TerminateScriptByName("gb_rob_shop");
        ScriptExtended.TerminateScriptByName("gunclub_shop");
        ScriptExtended.TerminateScriptByName("hairdo_shop_sp");
        ScriptExtended.TerminateScriptByName("re_shoprobbery");
        ScriptExtended.TerminateScriptByName("shop_controller");
        ScriptExtended.TerminateScriptByName("re_crashrescue");
        ScriptExtended.TerminateScriptByName("re_rescuehostage");
        ScriptExtended.TerminateScriptByName("fm_mission_controller");
        ScriptExtended.TerminateScriptByName("player_scene_m_shopping");
        ScriptExtended.TerminateScriptByName("shoprobberies");
        ScriptExtended.TerminateScriptByName("re_atmrobbery");
        ScriptExtended.TerminateScriptByName("ob_vend1");
        ScriptExtended.TerminateScriptByName("ob_vend2");
        Function.Call((Hash) -6788369613215147455L, new InputArgument[2]
        {
          InputArgument.op_Implicit("PRISON_ALARMS"),
          InputArgument.op_Implicit(0)
        });
        Function.Call((Hash) 2417821992125818111L, new InputArgument[3]
        {
          InputArgument.op_Implicit("AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_GENERAL"),
          InputArgument.op_Implicit(0),
          InputArgument.op_Implicit(0)
        });
        Function.Call((Hash) 2417821992125818111L, new InputArgument[3]
        {
          InputArgument.op_Implicit("AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_WARNING"),
          InputArgument.op_Implicit(0),
          InputArgument.op_Implicit(0)
        });
        Function.Call((Hash) -563637040166458307L, new InputArgument[7]
        {
          InputArgument.op_Implicit((int) Function.Call<int>((Hash) -3292914402564945716L, new InputArgument[1]
          {
            InputArgument.op_Implicit("prop_gate_prison_01")
          })),
          InputArgument.op_Implicit(1845f),
          InputArgument.op_Implicit(2605f),
          InputArgument.op_Implicit(45f),
          InputArgument.op_Implicit(false),
          InputArgument.op_Implicit(0),
          InputArgument.op_Implicit(0)
        });
        Function.Call((Hash) -7272475972733243984L, new InputArgument[7]
        {
          InputArgument.op_Implicit((int) Function.Call<int>((Hash) -3292914402564945716L, new InputArgument[1]
          {
            InputArgument.op_Implicit("prop_gate_prison_01")
          })),
          InputArgument.op_Implicit(1819.27f),
          InputArgument.op_Implicit(2608.53f),
          InputArgument.op_Implicit(44.61f),
          InputArgument.op_Implicit(false),
          InputArgument.op_Implicit(0),
          InputArgument.op_Implicit(0)
        });
        if (!this._reset)
          return;
        Function.Call((Hash) 6837250382999549709L, new InputArgument[1]
        {
          InputArgument.op_Implicit(false)
        });
        Function.Call((Hash) -8916161055282871969L, new InputArgument[1]
        {
          InputArgument.op_Implicit(false)
        });
        Function.Call((Hash) -9162020105814915111L, new InputArgument[1]
        {
          InputArgument.op_Implicit(false)
        });
        Function.Call((Hash) 3097765567273685773L, new InputArgument[1]
        {
          InputArgument.op_Implicit(false)
        });
        Function.Call((Hash) -606238161783359907L, new InputArgument[1]
        {
          InputArgument.op_Implicit(false)
        });
        this._reset = false;
      }
      else
      {
        if (this._reset)
          return;
        WorldController.Reset();
        this._reset = true;
      }
    }
  }
}
