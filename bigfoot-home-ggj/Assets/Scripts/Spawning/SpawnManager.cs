using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class SpawnManager
{
    private SpawnSettings settings;
    private Dictionary<Spawn, House> houseSpawnMap;

    public SpawnManager(SpawnSettings spawnSettings)
	{
        settings = spawnSettings;
        houseSpawnMap = new Dictionary<Spawn, House>();
    }

	public House CreateHouse(Transform spawn)
	{
		Debug.Log("Creating house @ " + spawn);
		if(spawn != null)
		{
            GameObject house_go = new GameObject("House");
            house_go.transform.position = spawn.position;
			var house = house_go.AddComponent<House>();
			house.Init(settings);
            return house;
		}
		else
		{
			Debug.LogWarning("Tried to spawn at null spawn point " + spawn);
            return null;
		}
	}

    public List<GameObject> CreateObstacles(List<Transform> spawns)
    {
        throw new System.NotImplementedException();
    }

    public void PopulateSpawns()
    {
        var spawnContainer = GameObject.FindObjectOfType<SpawnContainer>();

        if (spawnContainer != null)
        {
            if (spawnContainer.SpawnList == null)
            {
                Debug.Log("Null spawn list");
            }
            else
            {
                houseSpawnMap.Clear();
                foreach (var spawn in spawnContainer.SpawnList)
                {
                    var house = CreateHouse(spawn.transform);
                    spawn.state = Spawn.State.OCCUPIED;
                    if (houseSpawnMap.ContainsKey(spawn))
                    {
                        houseSpawnMap[spawn] = house;
                    }
                    else
                    {
                        houseSpawnMap.Add(spawn, house);
                    }
                }
            }
        }
    }

    public void ResetHouseAtSpawn(Spawn spawn)
    {
        houseSpawnMap[spawn].Init(settings);
    }

    public Spawn GetEmptySpawn()
    {
        var emptySpawns = houseSpawnMap.Keys.Where(x => x.state == Spawn.State.EMPTY).ToList();
        if (emptySpawns.Count > 0)
        {
            return emptySpawns[Random.Range(0, emptySpawns.Count)];
        }
        else
        {
            return null;
        }
    }

    public bool GrowRandomHouse()
    {
        var unfinishedHouses = houseSpawnMap.Values.Where(x => x != null && x.state != settings.lastState).ToList();
        if (unfinishedHouses.Count > 0)
        {
            var house = unfinishedHouses[Random.Range(0, unfinishedHouses.Count)];
            house.NextState();
            return true;
        }
        else
        {
            Debug.Log("No houses to grow");
            return false;
        }
    }

    public Spawn SpawnFromHouse(House house)
    {
        Spawn spawn = null;
        foreach (var item in houseSpawnMap)
        {
            if (item.Value == house)
            {
                spawn = item.Key;
                break;
            }
        }
        return spawn;
    }

    public int CalculatePollution()
    {
        int sum = 0;
        foreach (var house in houseSpawnMap.Values)
        {
            if (house != null)
            {
                sum += house.GetPollution();
            }
        }
        return sum;
    }
}
