using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField] float m_MoveSpeed = 7.5f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    bool m_Grounded;
    CapsuleCollider m_Collider;

	// Update is called once per frame
	void Update () {
		
	}
}
