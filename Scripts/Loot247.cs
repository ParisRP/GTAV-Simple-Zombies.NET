using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.PlayerManagement;
using ZombiesMod.Static;

namespace ZombiesMod.Scripts
{
  public class Loot247 : Script, ISpawner
  {
    public const float InteractDistance = 1.5f;
    private readonly List<Blip> _blips;
    private readonly List<Prop> _lootedShelfes;
    private readonly int[] _propHashes;

    public Loot247()
    {
      base.\u002Ector();
      Loot247.Instance = this;
      this._propHashes = new int[5]
      {
        Game.GenerateHash("v_ret_247shelves01"),
        Game.GenerateHash("v_ret_247shelves02"),
        Game.GenerateHash("v_ret_247shelves03"),
        Game.GenerateHash("v_ret_247shelves04"),
        Game.GenerateHash("v_ret_247shelves05")
      };
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted((EventHandler) ((sender, args) => this.Clear()));
    }

    public static Loot247 Instance { get; private set; }

    public bool Spawn { get; set; }

    private static Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    private static Ped PlayerPed
    {
      get
      {
        return Database.PlayerPed;
      }
    }

    private void OnTick(object sender, EventArgs e)
    {
      this.SpawnBlips();
      this.ClearBlips();
      this.LootShops();
    }

    private void LootShops()
    {
      if (!this.Spawn || ((Entity) Loot247.PlayerPed).IsPlayingAnim("oddjobs@shop_robbery@rob_till", "loop"))
        return;
      // ISSUE: method pointer
      Prop closest = (Prop) World.GetClosest<Prop>(Loot247.PlayerPosition, (M0[]) ((IEnumerable<Prop>) Enumerable.Where<Prop>((IEnumerable<M0>) World.GetNearbyProps(Loot247.PlayerPosition, 15f), (Func<M0, bool>) new Func<Prop, bool>((object) this, __methodptr(IsShelf)))).ToArray<Prop>());
      if (Entity.op_Equality((Entity) closest, (Entity) null) || (double) ((Entity) closest).get_Position().VDist(Loot247.PlayerPosition) > 1.5)
        return;
      Game.DisableControlThisFrame(2, (Control) 51);
      UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to loot the shelf.", false);
      if (!Game.IsDisabledControlJustPressed(2, (Control) 51))
        return;
      this._lootedShelfes.Add(closest);
      PlayerInventory.Instance.PickupItem(Database.Random.NextDouble() > 0.300000011920929 ? PlayerInventory.Instance.ItemFromName("Packaged Food") : PlayerInventory.Instance.ItemFromName("Clean Water"), ItemType.Item);
      Loot247.PlayerPed.get_Task().PlayAnimation("oddjobs@shop_robbery@rob_till", "loop");
      Ped playerPed = Loot247.PlayerPed;
      Vector3 vector3 = Vector3.op_Subtraction(((Entity) closest).get_Position(), Loot247.PlayerPosition);
      double heading = (double) ((Vector3) ref vector3).ToHeading();
      ((Entity) playerPed).set_Heading((float) heading);
    }

    private bool IsShelf(Prop arg)
    {
      int[] propHashes = this._propHashes;
      Model model = ((Entity) arg).get_Model();
      int hash = ((Model) ref model).get_Hash();
      return ((IEnumerable<int>) propHashes).Contains<int>(hash) && !this._lootedShelfes.Contains(arg);
    }

    private void ClearBlips()
    {
      if (this.Spawn)
        return;
      this.Clear();
    }

    private void SpawnBlips()
    {
      if (!this.Spawn || this._blips.Count >= Database.Shops247Locations.Length)
        return;
      foreach (Vector3 shops247Location in Database.Shops247Locations)
      {
        Blip blip = World.CreateBlip(shops247Location);
        blip.set_Sprite((BlipSprite) 52);
        blip.set_Name("Store");
        blip.set_IsShortRange(true);
        this._blips.Add(blip);
      }
    }

    public void Clear()
    {
      while (this._blips.Count > 0)
      {
        this._blips[0].Remove();
        this._blips.RemoveAt(0);
      }
    }
  }
}
