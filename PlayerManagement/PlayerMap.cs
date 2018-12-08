using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Linq;
using ZombiesMod.Extensions;
using ZombiesMod.Scripts;
using ZombiesMod.Static;

namespace ZombiesMod.PlayerManagement
{
  public class PlayerMap : Script
  {
    public const float InteractDistance = 3f;
    private Map _map;

    public static event PlayerMap.InteractedEvent Interacted;

    public PlayerMap()
    {
      base.\u002Ector();
      PlayerMap.Instance = this;
      this.add_Tick(new EventHandler(this.OnTick));
      this.add_Aborted(new EventHandler(this.OnAborted));
      PlayerInventory.BuildableUsed += new PlayerInventory.OnUsedBuildableEvent(this.InventoryOnBuildableUsed);
    }

    public static PlayerMap Instance { get; private set; }

    public bool EditMode { get; set; }

    public Vector3 PlayerPosition
    {
      get
      {
        return Database.PlayerPosition;
      }
    }

    public void Deserialize()
    {
      if (this._map != null)
        return;
      this._map = Serializer.Deserialize<Map>("./scripts/Map.dat") ?? new Map();
      this._map.ListChanged += (Map.OnListChangedEvent) (count => Serializer.Serialize<Map>("./scripts/Map.dat", this._map));
      this.LoadProps();
    }

    private void LoadProps()
    {
      if (this._map.Count <= 0)
        return;
      foreach (MapProp mapProp in this._map)
      {
        Model model = Model.op_Implicit(mapProp.PropName);
        if (!((Model) ref model).Request(1000))
        {
          UI.Notify(string.Format("Tried to request ~y~{0}~s~ but failed.", (object) mapProp.PropName));
        }
        else
        {
          Vector3 position = mapProp.Position;
          Prop prop1 = new Prop((int) Function.Call<int>((Hash) -7338251511766730620L, new InputArgument[7]
          {
            InputArgument.op_Implicit(((Model) ref model).get_Hash()),
            InputArgument.op_Implicit((float) position.X),
            InputArgument.op_Implicit((float) position.Y),
            InputArgument.op_Implicit((float) position.Z),
            InputArgument.op_Implicit(1),
            InputArgument.op_Implicit(1),
            InputArgument.op_Implicit(false)
          }));
          ((Entity) prop1).set_FreezePosition(!mapProp.IsDoor);
          ((Entity) prop1).set_Rotation(mapProp.Rotation);
          Prop prop2 = prop1;
          mapProp.Handle = ((Entity) prop2).get_Handle();
          if (mapProp.BlipSprite != 1)
          {
            Blip blip = ((Entity) prop2).AddBlip();
            blip.set_Sprite(mapProp.BlipSprite);
            blip.set_Color(mapProp.BlipColor);
            blip.set_Name(mapProp.Id);
            ZombieVehicleSpawner.Instance.SpawnBlocker.Add(mapProp.Position);
          }
        }
      }
    }

    private void InventoryOnBuildableUsed(BuildableInventoryItem item, Prop newProp)
    {
      if (this._map == null)
        this.Deserialize();
      WeaponStorageInventoryItem storageInventoryItem = item as WeaponStorageInventoryItem;
      MapProp mapProp = new MapProp(item.Id, item.PropName, item.BlipSprite, item.BlipColor, item.GroundOffset, item.Interactable, item.IsDoor, item.CanBePickedUp, ((Entity) newProp).get_Rotation(), ((Entity) newProp).get_Position(), ((Entity) newProp).get_Handle(), storageInventoryItem?.WeaponsList);
      this._map.Add(mapProp);
      ZombieVehicleSpawner.Instance.SpawnBlocker.Add(mapProp.Position);
    }

    private void OnAborted(object sender, EventArgs eventArgs)
    {
      this._map.Clear();
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      if (this._map == null || !this._map.Any<MapProp>() || MenuConrtoller.MenuPool.IsAnyMenuOpen())
        return;
      MapProp closest = (MapProp) World.GetClosest<MapProp>(this.PlayerPosition, (M0[]) this._map.ToArray<MapProp>());
      if (closest == null || !closest.CanBePickedUp || (double) closest.Position.VDist(this.PlayerPosition) > 3.0)
        return;
      this.TryUseMapProp(closest);
    }

    private void TryUseMapProp(MapProp mapProp)
    {
      bool flag = mapProp.CanBePickedUp && this.EditMode;
      if (!flag && !mapProp.Interactable)
        return;
      if (flag)
        Game.DisableControlThisFrame(2, (Control) 51);
      if (mapProp.Interactable)
        PlayerMap.DisableAttackActions();
      GameExtended.DisableWeaponWheel();
      UiExtended.DisplayHelpTextThisFrame(string.Format("{0}", flag ? (object) string.Format("Press ~INPUT_CONTEXT~ to pickup the {0}.\n", (object) mapProp.Id) : (!this.EditMode ? (object) "You're not in edit mode.\n" : (object) "")) + string.Format("{0}", mapProp.Interactable ? (object) string.Format("Press ~INPUT_ATTACK~ to {0}.", mapProp.IsDoor ? (object) "Lock/Unlock" : (object) "interact") : (object) ""), false);
      if (Game.IsDisabledControlJustPressed(2, (Control) 24) && mapProp.Interactable)
      {
        // ISSUE: reference to a compiler-generated field
        PlayerMap.InteractedEvent interacted = PlayerMap.Interacted;
        if (interacted != null)
          interacted(mapProp, PlayerInventory.Instance.ItemFromName(mapProp.Id));
      }
      if (!Game.IsDisabledControlJustPressed(2, (Control) 51) || !mapProp.CanBePickedUp || !PlayerInventory.Instance.PickupItem(PlayerInventory.Instance.ItemFromName(mapProp.Id), ItemType.Item))
        return;
      mapProp.Delete();
      this._map.Remove(mapProp);
      ZombieVehicleSpawner.Instance.SpawnBlocker.Remove(mapProp.Position);
    }

    public bool Find(Prop prop)
    {
      return this._map != null && this._map.Contains(prop);
    }

    private static void DisableAttackActions()
    {
      Game.DisableControlThisFrame(2, (Control) 257);
      Game.DisableControlThisFrame(2, (Control) 24);
      Game.DisableControlThisFrame(2, (Control) 25);
    }

    public void NotifyListChanged()
    {
      this._map.NotifyListChanged();
    }

    public delegate void InteractedEvent(MapProp mapProp, InventoryItemBase inventoryItem);
  }
}
