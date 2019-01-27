using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;


public class SpawnManager
{
    private SpawnSettings settings;
    // private Dictionary<Spawn, House> houseSpawnMap;

    private List<House> houses;
    private Random rnd = new Random();

    public SpawnManager(SpawnSettings spawnSettings)
	{
        settings = spawnSettings;
        //houseSpawnMap = new Dictionary<Spawn, House>();
        houses = new List<House>();
    }

	public House CreateHouse(Transform spawn)
	{
		// Debug.Log("Creating house @ " + spawn);
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

    public void CreateObstacles(Transform spawn)
    {
        if (spawn != null)
        {
            GameObject obstacle_rand = settings.obstacles[0];
            GameObject obstacle_go = new GameObject("Obstacle");
            obstacle_go.transform.position = spawn.position;
            //var obstacle = obstacle_go.AddComponent<GameObject>();
        }
        else
        {
            Debug.LogWarning("Tried to spawn at null spawn point " + spawn);
        }
    }

    public void PopulateSpawns()
    {
        var spawnContainer = GameObject.FindObjectOfType<SpawnContainer>();
        House house;

        if (spawnContainer != null)
        {
            if (spawnContainer.SpawnList == null)
            {
                Debug.Log("Null house spawn list");
            }
            else
            {
                foreach (var spawn in spawnContainer.SpawnList)
                {
                    house = CreateHouse(spawn.transform);
                    houses.Add(house);
                }
            }
        }
    }

    public void ResetHouse(House house)
    {
        if(house != null)
        {
            house.state = SpawnSettings.State.EMPTY;
            house.UpdateHealthBasedOnState();
            house.UpdateModelBasedOnState();
        }
    }

    public House GetEmptyHouse()
    {
        var emptyHouses = houses.FindAll(x => x.state == SpawnSettings.State.EMPTY);

        if (emptyHouses.Count > 0)
        {
            return emptyHouses[Random.Range(0, emptyHouses.Count)];
        }
        else
        {
            Debug.Log("No empty houses");
            return null;
        }
    }

    public House GetRandomRubbleHouse()
    {
        var emptyHouses = houses.Where(x => x.state == SpawnSettings.State.RUBBLE).ToList();
        if (emptyHouses.Count > 0)
        {
            return emptyHouses[Random.Range(0, emptyHouses.Count)];
        }
        else
        {
            return null;
        }
    }

    public House GetNonEmptyHouse()
    {
        var nonEmptyHouses = houses.Where(x => x.state != SpawnSettings.State.EMPTY && x.state != SpawnSettings.State.RUBBLE).ToList();
        if (nonEmptyHouses.Count > 0)
        {
            return nonEmptyHouses[Random.Range(0, nonEmptyHouses.Count)];
        }
        else
        {
            return null;
        }
    }

    public bool GrowRandomHouse()
    {
        var house = GetNonEmptyHouse();
        if (house != null)
        {
            house.NextState();
            return true;
        }
        else
        {
            Debug.Log("No houses to grow");
            return false;
        }
    }

    public bool GrowNewHouse()
    {
        var house = GetEmptyHouse();
        if (house != null)
        {
            Debug.Log("Grew new house");
            house.state = settings.firstState;
            house.UpdateHealthBasedOnState();
            house.UpdateModelBasedOnState();
            return true;
        }
        else
        {
            Debug.Log("No spots for new houses to grow");
            return false;
        }
    }

    public bool ResetRandomDestroyedHouse()
    {
        var house = GetRandomRubbleHouse();
        if(house != null)
        {
            house.ResetHouse();
            Debug.Log("Reset some rubble");
            return true;
        }
        else
        {
            Debug.Log("No rubble found to reset");
            return false;
        }
    }

    public int CalculatePollution()
    {
        int sum = 0;
        foreach (var house in houses)
        {
            if (house != null)
            {
                sum += house.GetPollution();
            }
        }
        return sum;
    }

    public int GetActiveHouseCount()
    {
        return houses.FindAll(x => x.state != SpawnSettings.State.RUBBLE && x.state != SpawnSettings.State.EMPTY).Count;
    }
}
