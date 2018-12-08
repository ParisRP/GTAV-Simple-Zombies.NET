using GTA;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class Map : ICollection<MapProp>, IEnumerable<MapProp>, IEnumerable
  {
    public List<MapProp> Props;

    public event Map.OnListChangedEvent ListChanged;

    public Map()
    {
      this.Props = new List<MapProp>();
      this.IsReadOnly = false;
    }

    public int Count
    {
      get
      {
        return this.Props.Count;
      }
    }

    public bool IsReadOnly { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public IEnumerator<MapProp> GetEnumerator()
    {
      return (IEnumerator<MapProp>) this.Props.GetEnumerator();
    }

    public void Add(MapProp item)
    {
      this.Props.Add(item);
      // ISSUE: reference to a compiler-generated field
      Map.OnListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this.Props.Count);
    }

    public void Clear()
    {
      while (this.Props.Count > 0)
      {
        MapProp prop = this.Props[0];
        prop.Delete();
        this.Props.Remove(prop);
      }
    }

    public bool Contains(MapProp item)
    {
      return this.Props.Contains(item);
    }

    public void CopyTo(MapProp[] array, int arrayIndex)
    {
      this.Props.CopyTo(array, arrayIndex);
    }

    public bool Remove(MapProp item)
    {
      if (!this.Props.Remove(item))
        return false;
      // ISSUE: reference to a compiler-generated field
      Map.OnListChangedEvent listChanged = this.ListChanged;
      if (listChanged != null)
        listChanged(this.Props.Count);
      return true;
    }

    public bool Contains(Prop prop)
    {
      return this.Props.Find((Predicate<MapProp>) (m => m.Handle == ((Entity) prop).get_Handle())) != null;
    }

    public void NotifyListChanged()
    {
      // ISSUE: reference to a compiler-generated field
      Map.OnListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this.Count);
    }

    public delegate void OnListChangedEvent(int count);
  }
}
