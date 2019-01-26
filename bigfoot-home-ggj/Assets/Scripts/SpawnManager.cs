using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SpawnManager
{
	private float houseStartingHealth;
	private List<GameObject> houseModels;

	public SpawnManager(List<GameObject> houseModels, float houseStartingHealth)
	{
		this.houseStartingHealth = houseStartingHealth;
		this.houseModels = houseModels;
	}

	public List<House> CreateHouses(List<GameObject> spawns)
	{
		List<House> houses = new List<House>();
		foreach (GameObject spawn in spawns)
		{
			Debug.Log("Creating object @ " + spawn);
			//TODO pick whether it is house or other entity
			if(spawn != null)
			{
				var house = spawn.AddComponent<House>();
				house.Init(houseModels, houseStartingHealth);
				house.OnHouseDestroyed += OnHouseDestroyed;
				houses.Add(house);
			}
			else
			{
				Debug.LogWarning("Tried to interact with null spawn point " + spawn);
			}
		}

		return houses;
	}

	public void OnHouseDestroyed(GameObject house)
	{
		Debug.LogFormat("A house was destroyed");
	}
}
