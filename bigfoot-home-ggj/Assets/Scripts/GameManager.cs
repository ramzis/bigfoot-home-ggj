using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public List<Spawn> spawns;
    public SpawnSettings spawnSettings;

    public bool isGameOver;
    public bool isWaveOver;
    public int currentWave = 0;
    public Dictionary<Spawn, House> houseSpawnMap;

    private Transform spawnContainer;

    public void OnValidate()
    {
        Debug.Assert(spawnSettings != null, "Missing spawn settings. Create new Spawn Settings Asset");
        Debug.Assert(spawnSettings.waves.Count > 0, "No wave data available in Spawn Settings");
    }

	void Start ()
	{
        spawns = new List<Spawn>();
        houseSpawnMap = new Dictionary<Spawn, House>();
        houseSpawnMap.Clear();
        spawnManager = new SpawnManager(spawnSettings);
        spawnContainer = new GameObject("Spawn Container").transform;
        isGameOver = false;
        StartCoroutine(Play());
	}

    public IEnumerator Play()
    {
        Debug.Log("Game has started.");

        Spawn spawn;

        var spawns_go = GameObject.Find("Spawns");
        if(spawns_go != null)
        {
            var spawnContainer = spawns_go.GetComponent<SpawnContainer>();
            if(spawnContainer != null)
            {
                if(spawnContainer.SpawnList == null)
                {
                    Debug.Log("Null spawn list");
                }
                else
                {
                    foreach(var s in spawnContainer.SpawnList)
                    {
                        houseSpawnMap.Add(s, null);
                    }
                }
            }
        }

        currentWave = 0;

        while (!isGameOver)
        {
            isWaveOver = false;
            StartCoroutine(WaveDurationCounter(spawnSettings.waves[currentWave].duration));

            while (!isWaveOver)
            {
                GrowRandomHouse();
                spawn = GetEmptySpawn();
                if (spawn != null)
                {
                    var house = CreateHouse(spawn.transform);
                    spawn.state = Spawn.State.OCCUPIED;
                    if(houseSpawnMap.ContainsKey(spawn))
                    {
                        houseSpawnMap[spawn] = house;
                    }
                    else
                    {
                        houseSpawnMap.Add(spawn, house);
                    }
                }
                else
                {
                    Debug.Log("No empty spawns found");
                }
                yield return new WaitForSeconds(spawnSettings.waves[currentWave].spawnDelay);
            }

            if(currentWave + 1 < spawnSettings.waves.Count)
            {
                currentWave++;
            }
        }

        isGameOver = true;

        Debug.Log("Game over!");

        yield return null;
    }

    public IEnumerator WaveDurationCounter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isWaveOver = true;
        yield return null;
    }

    public Spawn GetEmptySpawn()
    {
        return houseSpawnMap.Keys.FirstOrDefault(x => x.state == Spawn.State.EMPTY);

    }

    public Transform CreateNewSpawnPoint()
    {
        var spawnPoint = new GameObject("Spawn").transform;
        spawnPoint.parent = spawnContainer;
        return spawnPoint;
    }

    public void OffsetRandomlyWithinRangeFromOrigin(List<Spawn> spawns)
    {
        foreach(var spawn in spawns)
        {
            spawn.transform.position = new Vector3(
                Random.Range(-5f, 5f), 
                0,
                Random.Range(-5f, 5f));
        }
    }

    public House CreateHouse(Transform spawn)
    {
        List<House> houses = spawnManager.CreateHouses(new List<Transform>() { spawn });
        foreach (var house in houses)
        {
            house.OnHouseDestroyed += OnHouseDestroyed;
        }
        return houses[0];
    }

    public void OnHouseDestroyed(House house)
    {
        Debug.LogFormat("A {0} was destroyed", house.state);
        house.OnHouseDestroyed -= OnHouseDestroyed;

        Spawn spawn = null;
        foreach(var item in houseSpawnMap)
        {
            if(item.Value == house)
            {
                spawn = item.Key;
                break;
            }
        }

        if(spawn != null)
        {
            spawn.state = Spawn.State.EMPTY;
        }

        Destroy(house.gameObject);
    }

    public bool GrowRandomHouse()
    {
        var unfinishedHouses =  houseSpawnMap.Values.Where(x => x != null && x.state != spawnSettings.lastState).ToList();
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
}
