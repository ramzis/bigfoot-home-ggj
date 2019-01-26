using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		if (state >= settings.firstState && state <= settings.lastState)
		{
			if (settings.models.Count > (int) state)
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
        if (state >= settings.firstState && state <= settings.lastState)
        {
            if (settings.startingHealth.Count > (int)state)
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

	public void SetEmptyState()
	{
		state = SpawnSettings.State.EMPTY;
		UpdateModelBasedOnState();
        health = 0;
	}

	public void TakeDamage(float damage)
	{
		if (health <= 0)
		{
			Debug.LogWarning("Trying to damage already destroyed house.");
		}
		else
		{
			health -= damage;
			if (health <= 0)
			{
				if(OnHouseDestroyed != null)
					OnHouseDestroyed.Invoke(this);
			}
		}
	}
}
