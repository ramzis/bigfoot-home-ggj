using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(UIManager), typeof(SoundManager))]
public class GameManager : MonoBehaviour
{
    private GameObject player;
    private SpawnManager spawnManager;
    private SoundManager soundManager;
    private UIManager uiManager;
    private Attack attack;
    private GameObject gameOverMenu;
//    public Animator deathAnimator;
    public Image deathImage;

    public bool isGameOver;
    public bool isWaveOver;
    public int currentWave = 0;
    public int pollution = 0;
    public int threshold = 200;
    public int ticks = 0;

    [Header("Options")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public SpawnSettings spawnSettings;
    public float attackDamage = 50f;

    public void OnValidate()
    {
        Debug.Assert(spawnSettings != null, "Missing spawn settings. Create new Spawn Settings Asset");
        Debug.Assert(spawnSettings.waves.Count > 0, "No wave data available in Spawn Settings");
    }

    void Start()
    {
        uiManager = GetComponent<UIManager>();
        soundManager = GetComponent<SoundManager>();
        spawnManager = new SpawnManager(spawnSettings);
        gameOverMenu = GameObject.Find("GameOver");
        gameOverMenu.SetActive(false);
        SpawnPlayer();
        spawnManager.PopulateSpawns();
        StartCoroutine(Play());
    }

    public GameObject SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        player.name = "Player";
        attack = player.AddComponent<Attack>();
        attack.damage = attackDamage;
        attack.OnAttack += OnPlayerAttack;
        return player;
    }

    public IEnumerator Play()
    {
        //Debug.Log("Game has started.");

        currentWave = 0;
        pollution = 0;
        ticks = 0;
        isGameOver = false;
        attack.gameObject.SetActive(true);
        // if (deathImage != null)
        //     deathImage.gameObject.SetActive(false);

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
                spawnManager.GrowRandomHouse();
                // Create new houses
                spawnManager.GrowNewHouse();
                // Random grunt chance
                PlayGruntRandomly();
                // Calculate pollution caused by houses
                pollution += spawnManager.CalculatePollution();
                // Update pollution UI display
                uiManager.pollutionDisplay.text = pollution.ToString();
                UpdateFog();
                // Check for Game Over condition
                if (pollution > threshold)
                {
                    isWaveOver = true;
                    GameOver();
                }
                ticks++;
                // Delay between spawning more houses
                yield return new WaitForSeconds(spawnSettings.waves[currentWave].spawnDelay);
            }

            if (isGameOver)
                break;

            // Go to next wave or stay at last one
            if (currentWave + 1 < spawnSettings.waves.Count)
            {
                currentWave++;
            }
        }

        yield return null;
    }

    public void GameOver()
    {
        //Debug.Log("Game over!");
        gameOverMenu.SetActive(true);
        // if(deathImage != null)
        // {
        //     deathImage.gameObject.SetActive(true);
        // }
        // deathAnimator.SetTrigger("zoom");
        isGameOver = true;
        soundManager.PlaySoundByName(soundManager.playerAudioSource, "bigfoot_crying1");
        attack.gameObject.SetActive(false);
        
    }

    public void PlayGruntRandomly()
    {
        if (Random.Range(0, 1f) > 0.75f)
            soundManager.PlaySoundByName(soundManager.playerAudioSource, "short_punchy_growl");
    }

    public IEnumerator WaveDurationCounter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isWaveOver = true;
        yield return null;
    }

    public void OnHouseDestroyed(House house)
    {
        Debug.LogFormat("A {0} was destroyed", house.state);
        if (house != null)
        {
            house.SetRubbleState();
            StartCoroutine(house.Respawn());
        }

        int cleaningValue = house.GetPollution();
        pollution -= (int)((cleaningValue * ticks) / 1.5f);

        soundManager.PlaySoundByName(soundManager.buildingAudioSource, "short_destroy_building");

    }

    public void OnPlayerAttack()
    {
        soundManager.PlaySoundByName(soundManager.playerAudioSource, "short_growl");
    }

    public void UpdateFog()
    {
        RenderSettings.fogDensity = pollution / 10000f;
    }
}
