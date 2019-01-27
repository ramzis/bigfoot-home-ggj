using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField] float m_MoveSpeed = 7.5f;
    [SerializeField] float m_JumpPower = 10f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    bool m_Grounded;
    bool m_Jump;
    CapsuleCollider m_Collider;
    Transform m_Cam;
    Vector3 m_CamForward;
    Vector3 m_Move;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();
        if (Camera.main != null)
            m_Cam = Camera.main.transform;
    }

    void Update()
    {
        if (!m_Jump)
            m_Jump = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1,0,1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        Move(m_Move, m_Jump);
        m_Jump = false;
    }

    void Move(Vector3 move, bool jump)
    {
        
    }
}
