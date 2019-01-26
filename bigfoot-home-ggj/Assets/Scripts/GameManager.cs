using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public List<Transform> spawns;
    public SpawnSettings spawnSettings;

    List<int> waves = new List<int>() { 3, 5, 8, 12, 20 };
    List<float> waveDelay = new List<float>() { 3, 2.5f, 2f, 1.5f, 1f };

    public bool isGameOver;
    public bool isWaveOver;
    public int currentWave = 0;
    public List<House> houses;

    private Transform spawnContainer;

    public void OnValidate()
    {
        Debug.Assert(spawnSettings != null, "Missing spawn settings. Create new Spawn Settings Asset");
    }

	void Start ()
	{
        spawns = new List<Transform>();
        houses = new List<House>();
        spawnManager = new SpawnManager(spawnSettings);
        spawnContainer = new GameObject("Spawn Container").transform;
        isGameOver = false;
        StartCoroutine(Play());
	}

    public IEnumerator Play()
    {
        Debug.Log("Game has started.");

        // Loop through waves
        while (!isGameOver && currentWave < waves.Count)
        {
            // Setup wave params
            List<Transform> newSpawns = new List<Transform>();
            if (spawns.Count < waves[currentWave])
            {
                var delta = waves[currentWave] - spawns.Count;
                Debug.Log("Delta is " + delta);
                for (int s = 0; s < delta; s++)
                    newSpawns.Add(CreateNewSpawnPoint());

                OffsetRandomlyWithinRangeFromOrigin(newSpawns);
                spawns.AddRange(newSpawns);
            }

            // Execute wave
            isWaveOver = false;
            while (!isWaveOver)
            {
                Debug.LogFormat("Wave {0} starting with {1} spawn points", currentWave, spawns.Count);
                houses.AddRange(CreateHouses(newSpawns));
                
                while(GrowRandomHouse())
                {
                    //yield return new WaitForSeconds(waveDelay[currentWave]);
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForEndOfFrame();
                isWaveOver = true;
                Debug.Log("Wave is over");
            }

            currentWave++;
        }

        isGameOver = true;

        Debug.Log("Game over!");

        yield return null;
    }

    public Transform CreateNewSpawnPoint()
    {
        var spawnPoint = new GameObject("Spawn").transform;
        spawnPoint.parent = spawnContainer;
        return spawnPoint;
    }

    public void OffsetRandomlyWithinRangeFromOrigin(List<Transform> transforms)
    {
        foreach(var transform in transforms)
        {
            transform.position = new Vector3(
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
