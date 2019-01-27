using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Settings", menuName = "Spawning/Settings", order = 1)]
public class SpawnSettings : ScriptableObject
{
    public List<GameObject> models;
    public List<float> startingHealth;
    public List<Wave> waves;

    public State firstState = State.CONSTRUCTION;
    public State lastState = State.SKYSCRAPER;

    public enum State
    {
        EMPTY = -1,
        CONSTRUCTION = 0,
        HOUSE = 1,
        APARTMENT = 2,
        SKYSCRAPER = 3,
        RUBBLE = 4
    }
}
