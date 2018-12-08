using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using ZombiesMod.Static;

namespace ZombiesMod.Scripts
{
  public class AnimalSpawner : Script, ISpawner
  {
    public const int MinAnimalsPerSpawn = 3;
    public const int MaxAnimalsPerSpawn = 10;
    public const int RespawnDistance = 200;
    private readonly PedHash[] _possibleAnimals;
    private readonly List<Blip> _spawnBlips;
    private Dictionary<Blip, List<Ped>> _spawnMap;

    public AnimalSpawner()
    {
      // ISSUE: unable to decompile the method.
    }

    public static AnimalSpawner Instance { get; private set; }

    public bool Spawn { get; set; }

    private void OnAborted(object sender, EventArgs e)
    {
      this.Clear();
    }

    private void OnTick(object sender, EventArgs e)
    {
      if (this.Spawn)
      {
        this.CreateBlips();
        int index1 = 0;
        for (int count = this._spawnBlips.Count; index1 < count; ++index1)
        {
          Blip spawnBlip = this._spawnBlips[index1];
          if (!this._spawnMap.ContainsKey(spawnBlip))
          {
            List<Ped> animals = this.CreateAnimals(spawnBlip);
            this._spawnMap.Add(spawnBlip, animals);
          }
          else
          {
            List<Ped> spawn = this._spawnMap[spawnBlip];
            for (int index2 = spawn.Count - 1; index2 >= 0; --index2)
            {
              Ped ped = spawn[index2];
              if (Entity.op_Equality((Entity) ped, (Entity) null))
                spawn.Remove((Ped) null);
              else if (((Entity) ped).get_IsDead() || !((Entity) ped).Exists())
              {
                ((Entity) ped).MarkAsNoLongerNeeded();
                spawn.Remove(ped);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        this._spawnMap = (Dictionary<Blip, List<Ped>>) Enumerable.ToDictionary<KeyValuePair<Blip, List<Ped>>, Blip, List<Ped>>(Enumerable.Where<KeyValuePair<Blip, List<Ped>>>((IEnumerable<M0>) this._spawnMap, (Func<M0, bool>) (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_0 ?? (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_0 = new Func<KeyValuePair<Blip, List<Ped>>, bool>((object) AnimalSpawner.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003COnTick\u003Eb__16_0))))), (Func<M0, M1>) (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_1 ?? (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_1 = new Func<KeyValuePair<Blip, List<Ped>>, Blip>((object) AnimalSpawner.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003COnTick\u003Eb__16_1)))), (Func<M0, M2>) (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_2 ?? (AnimalSpawner.\u003C\u003Ec.\u003C\u003E9__16_2 = new Func<KeyValuePair<Blip, List<Ped>>, List<Ped>>((object) AnimalSpawner.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003COnTick\u003Eb__16_2)))));
      }
      else
        this.Clear();
    }

    private List<Ped> CreateAnimals(Blip blip)
    {
      List<Ped> pedList = new List<Ped>();
      int num = Database.Random.Next(3, 10);
      PedHash possibleAnimal = (PedHash) (int) (uint) this._possibleAnimals[Database.Random.Next(this._possibleAnimals.Length)];
      for (int index = 0; index < num; ++index)
      {
        Model model = Model.op_Implicit(possibleAnimal);
        Vector3 position = blip.get_Position();
        Vector3 vector3 = ((Vector3) ref position).Around(5f);
        Ped ped = World.CreatePed(model, vector3);
        if (!Entity.op_Equality((Entity) ped, (Entity) null))
        {
          pedList.Add(ped);
          ped.get_Task().WanderAround();
          ((Entity) ped).set_IsPersistent(true);
          Relationships.SetRelationshipBothWays((Relationship) 5, Relationships.PlayerRelationship, ped.get_RelationshipGroup());
        }
      }
      return pedList;
    }

    private void CreateBlips()
    {
      if (this._spawnBlips.Count >= Database.AnimalSpawns.Length)
        return;
      int index = 0;
      for (int length = Database.AnimalSpawns.Length; index < length; ++index)
      {
        Blip blip = World.CreateBlip(Database.AnimalSpawns[index]);
        blip.set_Sprite((BlipSprite) 141);
        blip.set_Name("Animals");
        this._spawnBlips.Add(blip);
      }
    }

    private void Clear()
    {
      if (this._spawnBlips.Count > 0)
      {
        using (List<Blip>.Enumerator enumerator1 = this._spawnBlips.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            Blip current = enumerator1.Current;
            if (this._spawnMap.ContainsKey(current))
            {
              using (List<Ped>.Enumerator enumerator2 = this._spawnMap[current].GetEnumerator())
              {
                while (enumerator2.MoveNext())
                  ((Entity) enumerator2.Current).Delete();
              }
            }
            current.Remove();
          }
        }
      }
      this._spawnMap.Clear();
    }
  }
}
