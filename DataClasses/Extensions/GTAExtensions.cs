using GTA;
using GTA.Native;
using System;
using ZombiesMod.Wrappers;

namespace ZombiesMod.Extensions
{
  public static class PedExtended
  {
    internal static readonly string[] SpeechModifierNames = new string[37]
    {
      "SPEECH_PARAMS_STANDARD",
      "SPEECH_PARAMS_ALLOW_REPEAT",
      "SPEECH_PARAMS_BEAT",
      "SPEECH_PARAMS_FORCE",
      "SPEECH_PARAMS_FORCE_FRONTEND",
      "SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND",
      "SPEECH_PARAMS_FORCE_NORMAL",
      "SPEECH_PARAMS_FORCE_NORMAL_CLEAR",
      "SPEECH_PARAMS_FORCE_NORMAL_CRITICAL",
      "SPEECH_PARAMS_FORCE_SHOUTED",
      "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR",
      "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL",
      "SPEECH_PARAMS_FORCE_PRELOAD_ONLY",
      "SPEECH_PARAMS_MEGAPHONE",
      "SPEECH_PARAMS_HELI",
      "SPEECH_PARAMS_FORCE_MEGAPHONE",
      "SPEECH_PARAMS_FORCE_HELI",
      "SPEECH_PARAMS_INTERRUPT",
      "SPEECH_PARAMS_INTERRUPT_SHOUTED",
      "SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR",
      "SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL",
      "SPEECH_PARAMS_INTERRUPT_NO_FORCE",
      "SPEECH_PARAMS_INTERRUPT_FRONTEND",
      "SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND",
      "SPEECH_PARAMS_ADD_BLIP",
      "SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT",
      "SPEECH_PARAMS_ADD_BLIP_FORCE",
      "SPEECH_PARAMS_ADD_BLIP_SHOUTED",
      "SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE",
      "SPEECH_PARAMS_ADD_BLIP_INTERRUPT",
      "SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE",
      "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED",
      "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR",
      "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL",
      "SPEECH_PARAMS_SHOUTED",
      "SPEECH_PARAMS_SHOUTED_CLEAR",
      "SPEECH_PARAMS_SHOUTED_CRITICAL"
    };

    public static void PlayPain(this Ped ped, int type)
    {
      Function.Call((Hash) -4856321419903345428L, new InputArgument[4]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(type),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      });
    }

    public static void PlayFacialAnim(this Ped ped, string animSet, string animName)
    {
      Function.Call((Hash) -2168944291012149011L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(animName),
        InputArgument.op_Implicit(animSet)
      });
    }

    public static bool HasBeenDamagedByMelee(this Ped ped)
    {
      return (bool) Function.Call<bool>((Hash) 1377327512274689684L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(1)
      });
    }

    public static bool HasBeenDamagedBy(this Ped ped, WeaponHash weapon)
    {
      return (bool) Function.Call<bool>((Hash) 1377327512274689684L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) weapon),
        InputArgument.op_Implicit(0)
      });
    }

    public static unsafe Bone LastDamagedBone(this Ped ped)
    {
      int num;
      if ((bool) Function.Call<bool>((Hash) -2929203469768285028L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(&num)
      }))
        return (Bone) num;
      return (Bone) 0;
    }

    public static void SetPathAvoidWater(this Ped ped, bool toggle)
    {
      Function.Call((Hash) 4106753751182965052L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void SetStealthMovement(this Ped ped, bool toggle)
    {
      Function.Call((Hash) -8589571964800369710L, new InputArgument[2]
      {
        InputArgument.op_Implicit(toggle ? 1 : 0),
        InputArgument.op_Implicit("DEFAULT_ACTION")
      });
    }

    public static bool GetStealthMovement(this Ped ped)
    {
      return (bool) Function.Call<bool>((Hash) 8947185480862490559L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static void SetComponentVariation(this Ped ped, ComponentId id, int drawableId, int textureId, int paletteId)
    {
      Function.Call((Hash) 2750315038012726912L, new InputArgument[5]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) id),
        InputArgument.op_Implicit(drawableId),
        InputArgument.op_Implicit(textureId),
        InputArgument.op_Implicit(paletteId)
      });
    }

    public static int GetDrawableVariation(this Ped ped, ComponentId id)
    {
      return (int) Function.Call<int>((Hash) 7490462606036423932L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) id)
      });
    }

    public static int GetNumberOfDrawableVariations(this Ped ped, ComponentId id)
    {
      return (int) Function.Call<int>((Hash) 2834476523764480066L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) id)
      });
    }

    public static bool IsSubttaskActive(this Ped ped, Subtask task)
    {
      return (bool) Function.Call<bool>((Hash) -5731389963444272811L, new InputArgument[2]
      {
        InputArgument.op_Implicit(ped),
        InputArgument.op_Implicit((int) task)
      });
    }

    public static bool IsDriving(this Ped ped)
    {
      return ped.IsSubttaskActive(Subtask.DrivingWandering) || ped.IsSubttaskActive(Subtask.DrivingGoingToDestinationOrEscorting);
    }

    public static void SetPathCanUseLadders(this Ped ped, bool toggle)
    {
      Function.Call((Hash) 8621491691477485422L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void SetPathCanClimb(this Ped ped, bool toggle)
    {
      Function.Call((Hash) -8212693258618671116L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void SetMovementAnimSet(this Ped ped, string animation)
    {
      if (Entity.op_Equality((Entity) ped, (Entity) null))
        return;
      while (true)
      {
        if (Function.Call<bool>((Hash) -4257582536886376016L, new InputArgument[1]
        {
          InputArgument.op_Implicit(animation)
        }) == 0)
        {
          Function.Call((Hash) 7972635428772450029L, new InputArgument[1]
          {
            InputArgument.op_Implicit(animation)
          });
          Script.Yield();
        }
        else
          break;
      }
      Function.Call((Hash) -5797657820774978577L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(animation),
        InputArgument.op_Implicit(1048576000)
      });
    }

    public static void RemoveElegantly(this Ped ped)
    {
      Function.Call((Hash) -6022081966519748258L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static void SetRagdollOnCollision(this Ped ped, bool toggle)
    {
      Function.Call((Hash) -1106493818855066473L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle)
      });
    }

    public static void SetAlertness(this Ped ped, Alertness alertness)
    {
      Function.Call((Hash) -2619105872414424666L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) alertness)
      });
    }

    public static void SetCombatAblility(this Ped ped, CombatAbility ability)
    {
      Function.Call((Hash) -4079649877180351064L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) ability)
      });
    }

    public static void SetCanEvasiveDive(this Ped ped, bool toggle)
    {
      Function.Call((Hash) 7744612924842995801L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void StopAmbientSpeechThisFrame(this Ped ped)
    {
      if (!ped.IsAmbientSpeechPlaying())
        return;
      Function.Call((Hash) -5134454549476615409L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static bool IsAmbientSpeechPlaying(this Ped ped)
    {
      return (bool) Function.Call<bool>((Hash) -8038141706915823699L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static void DisablePainAudio(this Ped ped, bool toggle)
    {
      Function.Call((Hash) -6222817867460529944L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void StopSpeaking(this Ped ped, bool shaking)
    {
      Function.Call((Hash) -7105317640777702445L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(shaking ? 1 : 0)
      });
    }

    public static void SetCanPlayAmbientAnims(this Ped ped, bool toggle)
    {
      Function.Call((Hash) 7166301455914477326L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void SetCombatAttributes(this Ped ped, CombatAttributes attribute, bool enabled)
    {
      Function.Call((Hash) -6955927877681029095L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) attribute),
        InputArgument.op_Implicit(enabled)
      });
    }

    public static void SetPathAvoidFires(this Ped ped, bool toggle)
    {
      Function.Call((Hash) 4923931356997885536L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(toggle ? 1 : 0)
      });
    }

    public static void ApplyDamagePack(this Ped ped, float damage, float multiplier, DamagePack damagePack)
    {
      Function.Call((Hash) 5106960513763051839L, new InputArgument[4]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(damagePack.ToString()),
        InputArgument.op_Implicit(damage),
        InputArgument.op_Implicit(multiplier)
      });
    }

    public static void SetCanAttackFriendlies(this Ped ped, FirendlyFireType type)
    {
      switch (type)
      {
        case FirendlyFireType.CantAttack:
          Function.Call((Hash) -5498390243159980195L, new InputArgument[3]
          {
            InputArgument.op_Implicit(((Entity) ped).get_Handle()),
            InputArgument.op_Implicit(false),
            InputArgument.op_Implicit(false)
          });
          break;
        case FirendlyFireType.CanAttack:
          Function.Call((Hash) -5498390243159980195L, new InputArgument[3]
          {
            InputArgument.op_Implicit(((Entity) ped).get_Handle()),
            InputArgument.op_Implicit(true),
            InputArgument.op_Implicit(false)
          });
          break;
      }
    }

    public static void PlayAmbientSpeech(this Ped ped, string speechName, SpeechModifier modifier = SpeechModifier.Standard)
    {
      if (modifier < SpeechModifier.Standard || modifier >= (SpeechModifier) PedExtended.SpeechModifierNames.Length)
        throw new ArgumentOutOfRangeException(nameof (modifier));
      Function.Call((Hash) -8213159594590722974L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(speechName),
        InputArgument.op_Implicit(PedExtended.SpeechModifierNames[(int) modifier])
      });
    }

    public static void Recruit(this Ped ped, Ped leader, bool canBeTargeted, bool invincible, int accuracy)
    {
      if (Entity.op_Equality((Entity) leader, (Entity) null))
        return;
      ped.LeaveGroup();
      ped.SetRagdollOnCollision(false);
      ped.get_Task().ClearAll();
      PedGroup currentPedGroup = leader.get_CurrentPedGroup();
      currentPedGroup.set_SeparationRange((float) int.MaxValue);
      if (!currentPedGroup.Contains(leader))
        currentPedGroup.Add(leader, true);
      if (!currentPedGroup.Contains(ped))
        currentPedGroup.Add(ped, false);
      ped.set_CanBeTargetted(canBeTargeted);
      ped.set_Accuracy(accuracy);
      ((Entity) ped).set_IsInvincible(invincible);
      ((Entity) ped).set_IsPersistent(true);
      ped.set_RelationshipGroup(leader.get_RelationshipGroup());
      ped.set_NeverLeavesGroup(true);
      ((Entity) ped).get_CurrentBlip()?.Remove();
      Blip blip = ((Entity) ped).AddBlip();
      blip.set_Color((BlipColor) 3);
      blip.set_Scale(0.7f);
      blip.set_Name("Friend");
      EntityEventWrapper wrapper = new EntityEventWrapper((Entity) ped);
      wrapper.Died += (EntityEventWrapper.OnDeathEvent) ((sender, entity) =>
      {
        entity.get_CurrentBlip()?.Remove();
        wrapper.Dispose();
      });
      ped.PlayAmbientSpeech("GENERIC_HI", SpeechModifier.Standard);
    }

    public static void Recruit(this Ped ped, Ped leader, bool canBeTargetted)
    {
      ped.Recruit(leader, canBeTargetted, false, 100);
    }

    public static void Recruit(this Ped ped, Ped leader)
    {
      ped.Recruit(leader, true);
    }

    public static void SetCombatRange(this Ped ped, CombatRange range)
    {
      Function.Call((Hash) 4350590797670664571L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) range)
      });
    }

    public static void SetCombatMovement(this Ped ped, CombatMovement movement)
    {
      Function.Call((Hash) 5592521861259579479L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit((int) movement)
      });
    }

    public static void ClearFleeAttributes(this Ped ped)
    {
      Function.Call((Hash) 8116279360099375049L, new InputArgument[3]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      });
    }

    public static bool IsUsingAnyScenario(this Ped ped)
    {
      return (bool) Function.Call<bool>((Hash) 6317224474499895619L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static bool CanHearPlayer(this Ped ped, Player player)
    {
      return (bool) Function.Call<bool>((Hash) -966241770451121623L, new InputArgument[2]
      {
        InputArgument.op_Implicit(player.get_Handle()),
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static void SetHearingRange(this Ped ped, float hearingRange)
    {
      Function.Call((Hash) 3722497735840494396L, new InputArgument[2]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(hearingRange)
      });
    }

    public static bool IsCurrentWeaponSileced(this Ped ped)
    {
      return (bool) Function.Call<bool>((Hash) 7345588343449861831L, new InputArgument[1]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle())
      });
    }

    public static void Jump(this Ped ped)
    {
      Function.Call((Hash) 784761447855974321L, new InputArgument[4]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(true),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      });
    }

    public static void SetToRagdoll(this Ped ped, int time)
    {
      Function.Call((Hash) -5865380420870110134L, new InputArgument[7]
      {
        InputArgument.op_Implicit(((Entity) ped).get_Handle()),
        InputArgument.op_Implicit(time),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0),
        InputArgument.op_Implicit(0)
      });
    }
  }
}
