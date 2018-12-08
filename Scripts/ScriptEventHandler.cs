using GTA;
using System;
using System.Collections.Generic;

namespace ZombiesMod.Scripts
{
  public class ScriptEventHandler : Script
  {
    private readonly List<EventHandler> _wrapperEventHandlers;
    private readonly List<EventHandler> _scriptEventHandlers;
    private int _index;

    public ScriptEventHandler()
    {
      base.\u002Ector();
      ScriptEventHandler.Instance = this;
      this._wrapperEventHandlers = new List<EventHandler>();
      this._scriptEventHandlers = new List<EventHandler>();
      this.add_Tick(new EventHandler(this.OnTick));
    }

    public static ScriptEventHandler Instance { get; private set; }

    public void RegisterScript(EventHandler eventHandler)
    {
      this._scriptEventHandlers.Add(eventHandler);
    }

    public void UnregisterScript(EventHandler eventHandler)
    {
      this._scriptEventHandlers.Remove(eventHandler);
    }

    public void RegisterWrapper(EventHandler eventHandler)
    {
      this._wrapperEventHandlers.Add(eventHandler);
    }

    public void UnregisterWrapper(EventHandler eventHandler)
    {
      this._wrapperEventHandlers.Remove(eventHandler);
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      this.UpdateWrappers(sender, eventArgs);
      this.UpdateScripts(sender, eventArgs);
    }

    private void UpdateScripts(object sender, EventArgs eventArgs)
    {
      for (int index = this._scriptEventHandlers.Count - 1; index >= 0; --index)
      {
        EventHandler scriptEventHandler = this._scriptEventHandlers[index];
        if (scriptEventHandler != null)
          scriptEventHandler(sender, eventArgs);
      }
    }

    private void UpdateWrappers(object sender, EventArgs eventArgs)
    {
      for (int index = this._index; index < this._index + 5 && index < this._wrapperEventHandlers.Count; ++index)
      {
        EventHandler wrapperEventHandler = this._wrapperEventHandlers[index];
        if (wrapperEventHandler != null)
          wrapperEventHandler(sender, eventArgs);
      }
      this._index += 5;
      if (this._index < this._wrapperEventHandlers.Count)
        return;
      this._index = 0;
    }
  }
}
