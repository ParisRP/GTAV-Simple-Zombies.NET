using GTA.Math;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZombiesMod.DataClasses
{
  public class SpawnBlocker : IList<Vector3>, ICollection<Vector3>, IEnumerable<Vector3>, IEnumerable
  {
    private readonly List<Vector3> _blockers = new List<Vector3>();

    public int Count
    {
      get
      {
        return this._blockers.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return ((ICollection<Vector3>) this._blockers).IsReadOnly;
      }
    }

    public IEnumerator<Vector3> GetEnumerator()
    {
      return (IEnumerator<Vector3>) this._blockers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this._blockers).GetEnumerator();
    }

    public void Add(Vector3 item)
    {
      this._blockers.Add(item);
    }

    public void Clear()
    {
      this._blockers.Clear();
    }

    public bool Contains(Vector3 item)
    {
      return this._blockers.Contains(item);
    }

    public void CopyTo(Vector3[] array, int arrayIndex)
    {
      this._blockers.CopyTo(array, arrayIndex);
    }

    public bool Remove(Vector3 item)
    {
      return this._blockers.Remove(item);
    }

    public int IndexOf(Vector3 item)
    {
      return this._blockers.IndexOf(item);
    }

    public void Insert(int index, Vector3 item)
    {
      this._blockers.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this._blockers.RemoveAt(index);
    }

    public Vector3 this[int index]
    {
      get
      {
        return this._blockers[index];
      }
      set
      {
        this._blockers[index] = value;
      }
    }

    public int FindIndex(Predicate<Vector3> match)
    {
      if (match == null)
        return -1;
      for (int index = 0; index < this.Count; ++index)
      {
        if (match(this[index]))
          return index;
      }
      return -1;
    }
  }
}
