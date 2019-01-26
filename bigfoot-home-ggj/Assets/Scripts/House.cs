using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public enum State
	{
		EMPTY = -1,
		CONSTRUCTION = 0,
		HOUSE = 1,
		APARTMENT = 2,
		SKYSCRAPER = 3
	}

	public float health;
	public State state;
	public Action<GameObject> OnHouseDestroyed;
	private List<GameObject> models;
	private State firstState = State.CONSTRUCTION;
	private State lastState = State.SKYSCRAPER;
	private GameObject spawnedModel;

	public void Init(List<GameObject> models, float startingHealth)
	{
		state = firstState;
		this.models = models;
		this.health = startingHealth;
		UpdateModelBasedOnState();
	}

	public void UpdateModelBasedOnState()
	{
		if (spawnedModel != null)
		{
			Destroy(spawnedModel);
		}

		if (state >= firstState && state <= lastState)
		{
			if (models.Count > (int) state)
			{
				spawnedModel = Instantiate(models[(int) state]);
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
		if(nextState > (int)lastState || nextState < (int)State.EMPTY)
		{
			Debug.LogWarningFormat("Attempted to set state to invalid range {0}", nextState);
		}
		else
		{
			state = (State)nextState;
			UpdateModelBasedOnState();
		}
	}

	public void SetEmptyState()
	{
		state = State.EMPTY;
		UpdateModelBasedOnState();
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
					OnHouseDestroyed.Invoke(gameObject);
			}
		}
	}
}
