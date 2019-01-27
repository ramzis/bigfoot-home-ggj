using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class House : MonoBehaviour
{
	public float health;
	public SpawnSettings.State state;
    public SpawnSettings settings;
	public Action<House> OnHouseDestroyed;

	private GameObject spawnedModel;

	public void Init(SpawnSettings settings)
	{
        this.settings = settings;
		state = settings.firstState;
		UpdateModelBasedOnState();
        UpdateHealthBasedOnState();
	}

	public void UpdateModelBasedOnState()
	{
		if (spawnedModel != null)
		{
			Destroy(spawnedModel);
		}

        if (state == SpawnSettings.State.EMPTY)
            return;

		if (state >= settings.firstState && state <= settings.lastState)
		{
			if (settings.models.Count > (int) state && state >= 0)
			{
				spawnedModel = Instantiate(settings.models[(int) state], gameObject.transform.position, Quaternion.identity, transform);
			}
			else
			{
				Debug.LogWarningFormat("Not enough models for state {0}", state);
			}
		}
	}

    public void UpdateHealthBasedOnState()
    {
        if (state == SpawnSettings.State.EMPTY)
        {
            health = 0;
            return;
        }

        if (state >= settings.firstState && state <= settings.lastState)
        {
            if (settings.startingHealth.Count > (int)state && state >= 0)
            {
                health = settings.startingHealth[(int)state];
            }
            else
            {
                Debug.LogWarningFormat("Not enough models for state {0}", state);
            }
        }
    }

	public void NextState()
	{
		int nextState = (int)state + 1;
		if(nextState > (int)settings.lastState || nextState < (int)SpawnSettings.State.EMPTY)
		{
			Debug.LogWarningFormat("Attempted to set state to invalid range {0}", nextState);
		}
		else
		{
			state = (SpawnSettings.State)nextState;
			UpdateModelBasedOnState();
            UpdateHealthBasedOnState();
        }
    }

	public void SetRubbleState()
	{
		state = SpawnSettings.State.RUBBLE;
		UpdateModelBasedOnState();
        health = 0;
	}

	public void TakeDamage(float damage)
	{
		if (health <= 0)
		{
            if(state != SpawnSettings.State.RUBBLE && state != SpawnSettings.State.EMPTY)
			    Debug.LogWarning("Trying to damage already destroyed house.");
		}
		else
		{
			health -= damage;
			if (health <= 0)
			{
                SetRubbleState();
				if(OnHouseDestroyed != null)
					OnHouseDestroyed.Invoke(this);
                StartCoroutine(Respawn());
			}
		}
	}

    public int GetPollution()
    {
        switch (state)
        {
            case SpawnSettings.State.EMPTY:
                return 0;
            case SpawnSettings.State.CONSTRUCTION:
                return 5;
            case SpawnSettings.State.HOUSE:
                return 10;
            case SpawnSettings.State.APARTMENT:
                return 15;
            case SpawnSettings.State.SKYSCRAPER:
                return 20;
            case SpawnSettings.State.RUBBLE:
                return 0;
            default:
                return 0;
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        state = SpawnSettings.State.EMPTY;
        UpdateModelBasedOnState();
        UpdateHealthBasedOnState();
    }
}
