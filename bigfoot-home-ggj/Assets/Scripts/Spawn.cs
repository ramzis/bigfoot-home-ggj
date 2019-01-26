using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public enum State{occupied,destroyed,clear};
    public State state;
    public int id = -1;
}