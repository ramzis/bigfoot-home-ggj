using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public enum State
    {
        EMPTY,
        OCCUPIED,
        DESTROYED
    };

    public State state;
    public int id = -1;
}