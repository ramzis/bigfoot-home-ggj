﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Settings", menuName = "Spawning/Settings", order = 1)]
public class SpawnSettings : ScriptableObject
{
    public List<GameObject> models;
    public List<GameObject> rubbleModels;
    public GameObject explosion;
    public List<float> startingHealth;
    public List<Wave> waves;
    public List<GameObject> obstacles;

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
    public enum Obstacle
    {
        EMPTY = -1,
        LEAVES = 0
    }
}
