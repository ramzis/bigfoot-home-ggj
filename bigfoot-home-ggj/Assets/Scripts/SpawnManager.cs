using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SpawnManager
{
    private SpawnSettings settings;

	public SpawnManager(SpawnSettings spawnSettings)
	{
        settings = spawnSettings;
	}

	public List<House> CreateHouses(List<Transform> spawns)
	{
		List<House> houses = new List<House>();
		foreach (Transform spawn in spawns)
		{
			Debug.Log("Creating object @ " + spawn);
			if(spawn != null)
			{
                GameObject house_go = new GameObject("House");
                house_go.transform.position = spawn.position;
				var house = house_go.AddComponent<House>();
				house.Init(settings);
				houses.Add(house);
			}
			else
			{
				Debug.LogWarning("Tried to interact with null spawn point " + spawn);
			}
		}
		return houses;
	}

    public List<GameObject> CreateObstacles(List<Transform> spawns)
    {
        throw new System.NotImplementedException();
    }
}
