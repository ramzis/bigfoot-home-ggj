using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainer : MonoBehaviour {
    public List<GameObject> SpawnList = new List<GameObject>();

    void Start(){
        //Debug.Log("Number of spawn points: " + SpawnList.Count);
    }
}
