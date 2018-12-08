using GTA;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using ZombiesMod.Static;

namespace ZombiesMod.PlayerManagement
{
  public class PlayerStats : Script
  {
    public static bool UseStats = true;
    private readonly float _statDamageInterval;
    private readonly float _hungerReductionMultiplier;
    private readonly float _thirstReductionMultiplier;
    private readonly float _sprintReductionMultiplier;
    private readonly float _statSustainLength;
    private readonly List<StatDisplayItem> _statDisplay;
    private float _hungerDamageTimer;
    private float _hungerSustainTimer;
    private float _thirstDamageTimer;
    private float _thirstSustainTimer;
    private bool _removedDisplay;

    public PlayerStats()
    {
      base.\u002Ector();
      PlayerInventory.FoodUsed += new PlayerInventory.OnUsedFoodEvent(this.PlayerInventoryOnFoodUsed);
      this._sprintReductionMultiplier = (float) this.get_Settings().GetValue<float>("stats", "sprint_reduction_multiplier", (M0) (double) this._sprintReductionMultiplier);
      this._hungerReductionMultiplier = (float) this.get_Settings().GetValue<float>("stats", "hunger_reduction_multiplier", (M0) (double) this._hungerReductionMultiplier);
      this._thirstReductionMultiplier = (float) this.get_Settings().GetValue<float>("stats", "thirst_reduction_multiplier", (M0) (double) this._thirstReductionMultiplier);
      this._statDamageInterval = (float) this.get_Settings().GetValue<float>("stats", "stat_damage_interaval", (M0) (double) this._statDamageInterval);
      this._statSustainLength = (float) this.get_Settings().GetValue<float>("stats", "stat_sustain_length", (M0) (double) this._statSustainLength);
      this.get_Settings().SetValue<bool>("stats", "use_stats", (M0) (PlayerStats.UseStats ? 1 : 0));
      this.get_Settings().SetValue<float>("stats", "sprint_reduction_multiplier", (M0) (double) this._sprintReductionMultiplier);
      this.get_Settings().SetValue<float>("stats", "hunger_reduction_multiplier", (M0) (double) this._hungerReductionMultiplier);
      this.get_Settings().SetValue<float>("stats", "thirst_reduction_multiplier", (M0) (double) this._thirstReductionMultiplier);
      this.get_Settings().SetValue<float>("stats", "stat_damage_interaval", (M0) (double) this._statDamageInterval);
      this.get_Settings().SetValue<float>("stats", "stat_sustain_length", (M0) (double) this._statSustainLength);
      this.get_Settings().Save();
      this._statDisplay = new List<StatDisplayItem>();
      foreach (Stat stat in new Stats().StatList)
      {
        StatDisplayItem statDisplayItem1 = new StatDisplayItem();
        statDisplayItem1.Stat = stat;
        BarTimerBar barTimerBar = new BarTimerBar(stat.Name.ToUpper());
        barTimerBar.set_ForegroundColor(Color.White);
        barTimerBar.set_BackgroundColor(Color.Gray);
        statDisplayItem1.Bar = barTimerBar;
        StatDisplayItem statDisplayItem2 = statDisplayItem1;
        this._statDisplay.Add(statDisplayItem2);
        MenuConrtoller.BarPool.Add((TimerBarBase) statDisplayItem2.Bar);
      }
      this.add_Tick(new EventHandler(this.OnTick));
      this.set_Interval(10);
    }

    private void PlayerInventoryOnFoodUsed(FoodInventoryItem item, FoodType foodType)
    {
      switch (foodType)
      {
        case FoodType.Water:
          this.UpdateStat((IFood) item, "Thirst", "Thirst ~g~sustained~s~.", 0.0f);
          break;
        case FoodType.Food:
          this.UpdateStat((IFood) item, "Hunger", "Hunger ~g~sustained~s~.", 0.0f);
          break;
        case FoodType.SpecialFood:
          this.UpdateStat((IFood) item, "Hunger", "Hunger ~g~sustained~s~.", 0.0f);
          this.UpdateStat((IFood) item, "Thirst", "Thirst ~g~sustained~s~.", 0.15f);
          break;
      }
    }

    private void UpdateStat(IFood item, string name, string notify, float valueOverride = 0.0f)
    {
      StatDisplayItem statDisplayItem = this._statDisplay.Find((Predicate<StatDisplayItem>) (displayItem => displayItem.Stat.Name == name));
      statDisplayItem.Stat.Value += (double) valueOverride <= 0.0 ? item.RestorationAmount : valueOverride;
      statDisplayItem.Stat.Sustained = true;
      UI.Notify(notify, true);
      if ((double) statDisplayItem.Stat.Value <= (double) statDisplayItem.Stat.MaxVal)
        return;
      statDisplayItem.Stat.Value = statDisplayItem.Stat.MaxVal;
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
      if (Database.PlayerIsDead)
      {
        foreach (StatDisplayItem statDisplayItem in this._statDisplay)
          statDisplayItem.Stat.Value = statDisplayItem.Stat.MaxVal;
      }
      else if (!PlayerStats.UseStats)
      {
        if (this._removedDisplay)
          return;
        foreach (StatDisplayItem statDisplayItem in this._statDisplay)
          MenuConrtoller.BarPool.Remove((TimerBarBase) statDisplayItem.Bar);
        this._removedDisplay = true;
      }
      else
      {
        if (this._removedDisplay)
        {
          foreach (StatDisplayItem statDisplayItem in this._statDisplay)
            MenuConrtoller.BarPool.Add((TimerBarBase) statDisplayItem.Bar);
          this._removedDisplay = false;
        }
        int index = 0;
        for (int count = this._statDisplay.Count; index < count; ++index)
        {
          StatDisplayItem statDisplayItem = this._statDisplay[index];
          Stat stat = statDisplayItem.Stat;
          statDisplayItem.Bar.set_Percentage(stat.Value);
          this.HandleReductionStat(stat, "Hunger", "You're ~r~starving~s~!", this._hungerReductionMultiplier, ref this._hungerDamageTimer, ref this._hungerSustainTimer);
          this.HandleReductionStat(stat, "Thirst", "You're ~r~dehydrated~s~!", this._thirstReductionMultiplier, ref this._thirstDamageTimer, ref this._thirstSustainTimer);
          this.HandleStamina(stat);
        }
      }
    }

    private void HandleStamina(Stat stat)
    {
      if (stat.Name != "Stamina")
        return;
      if (stat.Sustained)
      {
        if (Database.PlayerIsSprinting)
        {
          if ((double) stat.Value > 0.0)
          {
            stat.Value -= Game.get_LastFrameTime() * this._sprintReductionMultiplier;
          }
          else
          {
            stat.Sustained = false;
            stat.Value = 0.0f;
          }
        }
        else if ((double) stat.Value < (double) stat.MaxVal)
          stat.Value += Game.get_LastFrameTime() * (this._sprintReductionMultiplier * 10f);
        else
          stat.Value = stat.MaxVal;
      }
      else
      {
        Game.DisableControlThisFrame(2, (Control) 21);
        stat.Value += Game.get_LastFrameTime() * this._sprintReductionMultiplier;
        if ((double) stat.Value >= (double) stat.MaxVal * 0.300000011920929)
          stat.Sustained = true;
      }
    }

    private void HandleReductionStat(Stat stat, string targetName, string notification, float reductionMultiplier, ref float damageTimer, ref float sustainTimer)
    {
      if (stat.Name != targetName)
        return;
      if (!stat.Sustained)
      {
        if ((double) stat.Value > 0.0)
        {
          stat.Value -= Game.get_LastFrameTime() * reductionMultiplier;
          damageTimer = this._statDamageInterval;
        }
        else
        {
          UI.Notify(notification);
          damageTimer += Game.get_LastFrameTime();
          if ((double) damageTimer >= (double) this._statDamageInterval)
          {
            PlayerStats.PlayerPed.ApplyDamage(Database.Random.Next(3, 15));
            damageTimer = 0.0f;
          }
          stat.Value = 0.0f;
        }
      }
      else
      {
        damageTimer = this._statDamageInterval;
        sustainTimer += Game.get_LastFrameTime();
        if ((double) sustainTimer < (double) this._statSustainLength)
          return;
        sustainTimer = 0.0f;
        stat.Sustained = false;
      }
    }
  }
}
