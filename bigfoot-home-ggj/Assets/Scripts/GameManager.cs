using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public List<Spawn> spawns;
    public SpawnSettings spawnSettings;

    public bool isGameOver;
    public bool isWaveOver;
    public int currentWave = 0;
    public List<House> houses;

    private Transform spawnContainer;

    public void OnValidate()
    {
        Debug.Assert(spawnSettings != null, "Missing spawn settings. Create new Spawn Settings Asset");
        Debug.Assert(spawnSettings.waves.Count > 0, "No wave data available in Spawn Settings");
    }

	void Start ()
	{
        spawns = new List<Spawn>();
        houses = new List<House>();
        spawnManager = new SpawnManager(spawnSettings);
        spawnContainer = new GameObject("Spawn Container").transform;
        isGameOver = false;
        StartCoroutine(Play());
	}

    public IEnumerator Play()
    {
        Debug.Log("Game has started.");

        Spawn spawn;

        /// Create demo spawn points --- TO BE REMOVED
        for (int i = 0; i < 20; i++)
        {
            var spawn_t = CreateNewSpawnPoint();
            spawn = spawn_t.gameObject.AddComponent<Spawn>();
            spawns.Add(spawn);
            OffsetRandomlyWithinRangeFromOrigin(spawns);
        }
        ///

        currentWave = 0;

        while (!isGameOver)
        {
            isWaveOver = false;
            StartCoroutine(WaveDurationCounter(spawnSettings.waves[currentWave].duration));

            while (!isWaveOver)
            {
                GrowRandomHouse();
                spawn = GetEmptySpawn();
                if(spawn != null)
                {
                    houses.AddRange(CreateHouses(new List<Transform>() { spawn.transform }));
                    spawn.state = Spawn.State.OCCUPIED;
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
        return spawns.Find(x => x.state == Spawn.State.EMPTY);
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

    public List<House> CreateHouses(List<Transform> spawns)
    {
        List<House> houses = spawnManager.CreateHouses(spawns);
        foreach (var house in houses)
        {
            house.OnHouseDestroyed += OnHouseDestroyed;
        }
        return houses;
    }

    public void OnHouseDestroyed(House house)
    {
        Debug.LogFormat("A {0} was destroyed", house.state);
        house.OnHouseDestroyed -= OnHouseDestroyed;
        Destroy(house.gameObject);
    }

    public bool GrowRandomHouse()
    {
        var unfinishedHouses = houses.FindAll(x => x != null && x.state != spawnSettings.lastState);
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
