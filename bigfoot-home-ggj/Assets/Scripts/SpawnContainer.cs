using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainer : MonoBehaviour
{
    public List<Spawn> SpawnList = new List<Spawn>();

    void Start()
    {
        //Debug.Log("Number of spawn points: " + SpawnList.Count);
    }
}
