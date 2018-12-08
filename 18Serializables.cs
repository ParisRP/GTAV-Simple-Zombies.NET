using System;
using System.Collections;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class PedCollection : IList<PedData>, ICollection<PedData>, IEnumerable<PedData>, IEnumerable
  {
    private readonly List<PedData> _peds;

    public event PedCollection.ListChangedEvent ListChanged;

    public PedCollection()
    {
      this._peds = new List<PedData>();
    }

    public int Count
    {
      get
      {
        return this._peds.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return ((ICollection<PedData>) this._peds).IsReadOnly;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public IEnumerator<PedData> GetEnumerator()
    {
      return (IEnumerator<PedData>) this._peds.GetEnumerator();
    }

    public void Add(PedData item)
    {
      this._peds.Add(item);
      // ISSUE: reference to a compiler-generated field
      PedCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this);
    }

    public void Clear()
    {
      this._peds.Clear();
      // ISSUE: reference to a compiler-generated field
      PedCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this);
    }

    public bool Contains(PedData item)
    {
      return this._peds.Contains(item);
    }

    public void CopyTo(PedData[] array, int arrayIndex)
    {
      this._peds.CopyTo(array, arrayIndex);
    }

    public bool Remove(PedData item)
    {
      bool flag = this._peds.Remove(item);
      // ISSUE: reference to a compiler-generated field
      PedCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged != null)
        listChanged(this);
      return flag;
    }

    public int IndexOf(PedData item)
    {
      return this._peds.IndexOf(item);
    }

    public void Insert(int index, PedData item)
    {
      this._peds.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this._peds.RemoveAt(index);
    }

    public PedData this[int index]
    {
      get
      {
        return this._peds[index];
      }
      set
      {
        this._peds[index] = value;
      }
    }

    public delegate void ListChangedEvent(PedCollection sender);
  }
}
