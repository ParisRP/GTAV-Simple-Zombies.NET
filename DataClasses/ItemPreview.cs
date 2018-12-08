using GTA;
using GTA.Math;
using GTA.Native;
using System;
using ZombiesMod.Extensions;
using ZombiesMod.Scripts;
using ZombiesMod.Static;

namespace ZombiesMod.DataClasses
{
  public class ItemPreview
  {
    private Vector3 _currentOffset;
    private Prop _currentPreview;
    private Prop _resultProp;
    private bool _preview;
    private bool _isDoor;
    private string _currnetPropHash;

    public bool PreviewComplete { get; private set; }

    public ItemPreview()
    {
      ScriptEventHandler.Instance.RegisterScript(new EventHandler(this.OnTick));
      ScriptEventHandler.Instance.add_Aborted((EventHandler) ((sender, args) => this.Abort()));
    }

    public void OnTick(object sender, EventArgs eventArgs)
    {
      if (!this._preview)
        return;
      this.CreateItemPreview();
    }

    public Prop GetResult()
    {
      return this._resultProp;
    }

    public void StartPreview(string propHash, Vector3 offset, bool isDoor)
    {
      if (this._preview)
        return;
      this._preview = true;
      this._currnetPropHash = propHash;
      this._isDoor = isDoor;
    }

    private void CreateItemPreview()
    {
      if (Entity.op_Equality((Entity) this._currentPreview, (Entity) null))
      {
        this.PreviewComplete = false;
        this._currentOffset = Vector3.get_Zero();
        Prop prop = World.CreateProp(Model.op_Implicit(this._currnetPropHash), (Vector3) null, (Vector3) null, false, false);
        if (Entity.op_Equality((Entity) prop, (Entity) null))
        {
          UI.Notify(string.Format("Failed to load prop, even after request.\nProp Name: {0}", (object) this._currnetPropHash));
          this._resultProp = (Prop) null;
          this._preview = false;
          this.PreviewComplete = true;
        }
        else
        {
          ((Entity) prop).set_HasCollision(false);
          this._currentPreview = prop;
          ((Entity) this._currentPreview).set_Alpha(150);
          Database.PlayerPed.get_Weapons().Select((WeaponHash) -1569615261, true);
          this._resultProp = (Prop) null;
        }
      }
      else
      {
        UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_AIM~ to cancel.\nPress ~INPUT_ATTACK~ to place the item.", true);
        Game.DisableControlThisFrame(2, (Control) 25);
        Game.DisableControlThisFrame(2, (Control) 24);
        Game.DisableControlThisFrame(2, (Control) 257);
        Game.DisableControlThisFrame(2, (Control) 152);
        Game.DisableControlThisFrame(2, (Control) 153);
        Game.DisableControlThisFrame(2, (Control) 44);
        Game.DisableControlThisFrame(2, (Control) 27);
        Game.DisableControlThisFrame(2, (Control) 172);
        Game.DisableControlThisFrame(2, (Control) 173);
        Game.DisableControlThisFrame(2, (Control) 21);
        GameExtended.DisableWeaponWheel();
        if (Game.IsDisabledControlPressed(2, (Control) 25))
        {
          ((Entity) this._currentPreview).Delete();
          this._currentPreview = this._resultProp = (Prop) null;
          this._preview = false;
          this.PreviewComplete = true;
          ScriptEventHandler.Instance.UnregisterScript(new EventHandler(this.OnTick));
        }
        else
        {
          Vector3 position = GameplayCamera.get_Position();
          Vector3 direction = GameplayCamera.get_Direction();
          RaycastResult raycastResult = World.Raycast(position, Vector3.op_Addition(position, Vector3.op_Multiply(direction, 15f)), (IntersectOptions) -1, (Entity) Database.PlayerPed);
          Vector3 hitCoords = ((RaycastResult) ref raycastResult).get_HitCoords();
          if (Vector3.op_Inequality(hitCoords, Vector3.get_Zero()) && (double) ((Vector3) ref hitCoords).DistanceTo(Database.PlayerPosition) > 1.5)
          {
            ItemPreview.DrawScaleForms();
            float num = Game.IsControlPressed(2, (Control) 21) ? 1.5f : 1f;
            if (Game.IsControlPressed(2, (Control) 152))
            {
              Vector3 rotation = ((Entity) this._currentPreview).get_Rotation();
              ref __Null local = ref rotation.Z;
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              ^(float&) ref local = ^(float&) ref local + Game.get_LastFrameTime() * 50f * num;
              ((Entity) this._currentPreview).set_Rotation(rotation);
            }
            else if (Game.IsControlPressed(2, (Control) 153))
            {
              Vector3 rotation = ((Entity) this._currentPreview).get_Rotation();
              ref __Null local = ref rotation.Z;
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              ^(float&) ref local = ^(float&) ref local - Game.get_LastFrameTime() * 50f * num;
              ((Entity) this._currentPreview).set_Rotation(rotation);
            }
            if (Game.IsControlPressed(2, (Control) 172))
            {
              ref __Null local = ref this._currentOffset.Z;
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              ^(float&) ref local = ^(float&) ref local + Game.get_LastFrameTime() * num;
            }
            else if (Game.IsControlPressed(2, (Control) 173))
            {
              ref __Null local = ref this._currentOffset.Z;
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              // ISSUE: cast to a reference type
              // ISSUE: explicit reference operation
              ^(float&) ref local = ^(float&) ref local - Game.get_LastFrameTime() * num;
            }
            ((Entity) this._currentPreview).set_Position(Vector3.op_Addition(hitCoords, this._currentOffset));
            ((Entity) this._currentPreview).set_IsVisible(true);
            if (!Game.IsDisabledControlJustPressed(2, (Control) 24))
              return;
            ((Entity) this._currentPreview).ResetAlpha();
            this._resultProp = this._currentPreview;
            ((Entity) this._resultProp).set_HasCollision(true);
            ((Entity) this._resultProp).set_FreezePosition(!this._isDoor);
            this._preview = false;
            this._currentPreview = (Prop) null;
            this._currnetPropHash = string.Empty;
            this.PreviewComplete = true;
            ScriptEventHandler.Instance.UnregisterScript(new EventHandler(this.OnTick));
          }
          else
            ((Entity) this._currentPreview).set_IsVisible(false);
        }
      }
    }

    private static void DrawScaleForms()
    {
      Scaleform scaleform = new Scaleform("instructional_buttons");
      scaleform.CallFunction("CLEAR_ALL", new object[0]);
      scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", new object[1]
      {
        (object) 0
      });
      scaleform.CallFunction("CREATE_CONTAINER", new object[0]);
      scaleform.CallFunction("SET_DATA_SLOT", new object[3]
      {
        (object) 0,
        (object) Function.Call<string>((Hash) 331533201183454215L, new InputArgument[3]
        {
          InputArgument.op_Implicit(2),
          InputArgument.op_Implicit(152),
          InputArgument.op_Implicit(0)
        }),
        (object) string.Empty
      });
      scaleform.CallFunction("SET_DATA_SLOT", new object[3]
      {
        (object) 1,
        (object) Function.Call<string>((Hash) 331533201183454215L, new InputArgument[3]
        {
          InputArgument.op_Implicit(2),
          InputArgument.op_Implicit(153),
          InputArgument.op_Implicit(0)
        }),
        (object) "Rotate"
      });
      scaleform.CallFunction("SET_DATA_SLOT", new object[3]
      {
        (object) 2,
        (object) Function.Call<string>((Hash) 331533201183454215L, new InputArgument[3]
        {
          InputArgument.op_Implicit(2),
          InputArgument.op_Implicit(172),
          InputArgument.op_Implicit(0)
        }),
        (object) string.Empty
      });
      scaleform.CallFunction("SET_DATA_SLOT", new object[3]
      {
        (object) 3,
        (object) Function.Call<string>((Hash) 331533201183454215L, new InputArgument[3]
        {
          InputArgument.op_Implicit(2),
          InputArgument.op_Implicit(173),
          InputArgument.op_Implicit(0)
        }),
        (object) "Lift/Lower"
      });
      scaleform.CallFunction("SET_DATA_SLOT", new object[3]
      {
        (object) 4,
        (object) Function.Call<string>((Hash) 331533201183454215L, new InputArgument[3]
        {
          InputArgument.op_Implicit(2),
          InputArgument.op_Implicit(21),
          InputArgument.op_Implicit(0)
        }),
        (object) "Accelerate"
      });
      scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", new object[1]
      {
        (object) -1
      });
      scaleform.Render2D();
    }

    public void Abort()
    {
      ((Entity) this._currentPreview)?.Delete();
    }
  }
}
