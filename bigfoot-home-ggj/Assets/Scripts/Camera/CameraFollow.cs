using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Camera playerCam;
    public Transform trans;
	// Use this for initialization
	void Start () {
        playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        trans = GameObject.FindWithTag("CamTransform").transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        playerCam.transform.LookAt(trans);
	}
}
