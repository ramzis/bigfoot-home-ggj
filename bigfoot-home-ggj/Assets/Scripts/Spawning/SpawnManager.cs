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

	public House CreateHouse(Transform spawn)
	{
		Debug.Log("Creating object @ " + spawn);
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
}
