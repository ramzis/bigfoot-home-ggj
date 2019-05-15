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

    public enum Type
    {
        HOUSE,
        OBSTACLE
    }

    public State state;
    public Type type;
    public int id = -1;

}
