using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UIManager), typeof(SoundManager))]
public class GameManager : MonoBehaviour
{
    private GameObject player;
    private SpawnManager spawnManager;
    private SoundManager soundManager;
    private UIManager uiManager;
    private Attack attack;

    private Dictionary<Spawn, House> houseSpawnMap;

    public bool isGameOver;
    public bool isWaveOver;
    public int currentWave = 0;
    public int pollution = 0;
    public int threshold = 200;
    public int ticks = 0;

    [Header("Options")]
    public GameObject playerPrefab;
    public SpawnSettings spawnSettings;
    public float attackDamage = 50f;

    public void OnValidate()
    {
        Debug.Assert(spawnSettings != null, "Missing spawn settings. Create new Spawn Settings Asset");
        Debug.Assert(spawnSettings.waves.Count > 0, "No wave data available in Spawn Settings");
    }

	void Start ()
	{
        uiManager = GetComponent<UIManager>();
        soundManager = GetComponent<SoundManager>();
        houseSpawnMap = new Dictionary<Spawn, House>();
        spawnManager = new SpawnManager(spawnSettings);
        SpawnPlayer();
        UpdateSpawns();
        StartCoroutine(Play());
	}

    public void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, new Vector3(0,0.75f,0f), Quaternion.identity);
        player.name = "Player";
        attack = player.AddComponent<Attack>();
        attack.damage = attackDamage;
        attack.OnAttack += OnPlayerAttack;
    }

    public void UpdateSpawns()
    {
        var spawnContainer = FindObjectOfType<SpawnContainer>();

        if (spawnContainer != null)
        {
            if (spawnContainer.SpawnList == null)
            {
                Debug.Log("Null spawn list");
            }
            else
            {
                houseSpawnMap.Clear();
                foreach (var s in spawnContainer.SpawnList)
                {
                    houseSpawnMap.Add(s, null);
                }
            }
        }
    }

    public IEnumerator Play()
    {
        Debug.Log("Game has started.");

        Spawn spawn;
        House house;

        currentWave = 0;
        pollution = 0;
        ticks = 0;
        isGameOver = false;
        attack.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        soundManager.PlaySoundByName(soundManager.playerAudioSource, "short_punchy_growl");

        while (!isGameOver)
        {
            isWaveOver = false;
            // Delay between moving to next wave
            StartCoroutine(WaveDurationCounter(spawnSettings.waves[currentWave].duration));

            while (!isWaveOver)
            {
                // Update existing houses
                GrowRandomHouse();
                // Create new houses
                spawn = GetEmptySpawn();
                if (spawn != null)
                {
                    house = CreateHouse(spawn.transform);
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
                // Calculate pollution caused by houses
                pollution += CalculatePollution();
                uiManager.pollutionDisplay.text = pollution.ToString();
                if (pollution > threshold)
                {
                    GameOver();
                }
                ticks++;
                // Delay between spawning more houses
                yield return new WaitForSeconds(spawnSettings.waves[currentWave].spawnDelay);
            }
            // Go to next wave or stay at last one
            if(currentWave + 1 < spawnSettings.waves.Count)
            {
                currentWave++;
            }
        }

        GameOver();

        Debug.Log("Game over!");

        yield return null;
    }

    public void GameOver()
    {
        isGameOver = true;
        soundManager.PlaySoundByName(soundManager.playerAudioSource, "bigfoot_crying1");
        attack.gameObject.SetActive(false);
    }

    public IEnumerator WaveDurationCounter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isWaveOver = true;
        yield return null;
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

    public House CreateHouse(Transform spawn)
    {
        House house = spawnManager.CreateHouse(spawn);
        house.OnHouseDestroyed += OnHouseDestroyed;
        return house;
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

        int cleaningValue = house.GetPollution();
        pollution -= (int)((cleaningValue * ticks) / 1.5f);

        Destroy(house.gameObject);

        soundManager.PlaySoundByName(soundManager.buildingAudioSource, "short_destroy_building");

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

    public int CalculatePollution()
    {
        int sum = 0;
        foreach(var house in houseSpawnMap.Values)
        {
            if(house != null)
            {
                sum += house.GetPollution();
            }
        }
        return sum;
    }

    public void OnPlayerAttack()
    {
        soundManager.PlaySoundByName(soundManager.playerAudioSource, "short_growl");   
    }
}
