using System;
using System.Collections;
using System.Collections.Generic;

namespace ZombiesMod
{
  [Serializable]
  public class VehicleCollection : IList<VehicleData>, ICollection<VehicleData>, IEnumerable<VehicleData>, IEnumerable
  {
    private readonly List<VehicleData> _vehicles;

    public event VehicleCollection.ListChangedEvent ListChanged;

    public VehicleCollection()
    {
      this._vehicles = new List<VehicleData>();
    }

    public int Count
    {
      get
      {
        return this._vehicles.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return ((ICollection<VehicleData>) this._vehicles).IsReadOnly;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public IEnumerator<VehicleData> GetEnumerator()
    {
      return (IEnumerator<VehicleData>) this._vehicles.GetEnumerator();
    }

    public void Add(VehicleData item)
    {
      this._vehicles.Add(item);
      // ISSUE: reference to a compiler-generated field
      VehicleCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this);
    }

    public void Clear()
    {
      this._vehicles.Clear();
      // ISSUE: reference to a compiler-generated field
      VehicleCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged == null)
        return;
      listChanged(this);
    }

    public bool Contains(VehicleData item)
    {
      return this._vehicles.Contains(item);
    }

    public void CopyTo(VehicleData[] array, int arrayIndex)
    {
      this._vehicles.CopyTo(array, arrayIndex);
    }

    public bool Remove(VehicleData item)
    {
      bool flag = this._vehicles.Remove(item);
      // ISSUE: reference to a compiler-generated field
      VehicleCollection.ListChangedEvent listChanged = this.ListChanged;
      if (listChanged != null)
        listChanged(this);
      return flag;
    }

    public int IndexOf(VehicleData item)
    {
      return this._vehicles.IndexOf(item);
    }

    public void Insert(int index, VehicleData item)
    {
      this._vehicles.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this._vehicles.RemoveAt(index);
    }

    public VehicleData this[int index]
    {
      get
      {
        return this._vehicles[index];
      }
      set
      {
        this._vehicles[index] = value;
      }
    }

    public delegate void ListChangedEvent(VehicleCollection sender);
  }
}
