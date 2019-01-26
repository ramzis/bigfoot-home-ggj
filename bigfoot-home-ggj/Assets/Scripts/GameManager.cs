using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public List<GameObject> spawns;
	public List<GameObject> houseModels;

	void Start ()
	{
		spawns = new List<GameObject>()
		{
			new GameObject("Spawn")
		};
		spawnManager = new SpawnManager(houseModels, 100f);
		spawnManager.CreateHouses(spawns);
	}
}
