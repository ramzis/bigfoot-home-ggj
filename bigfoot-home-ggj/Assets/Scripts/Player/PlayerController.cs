using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField] float m_MoveSpeed = 5f;
    [SerializeField] float m_TurnSpeed = 10f;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    //CapsuleCollider m_Collider;
    Transform m_Cam;
    Vector3 m_CamForward;
    Vector3 m_Move;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Collider = GetComponent<CapsuleCollider>();
        if (Camera.main != null)
            m_Cam = Camera.main.transform;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        Move(m_Move);
    }

    void Move(Vector3 move)
    {
        if (move.normalized == Vector3.zero){
            m_Animator.SetBool("Idle", true);
            return;
        }
        m_Rigidbody.velocity = new Vector3(move.x * m_MoveSpeed, m_Rigidbody.velocity.y, move.z * m_MoveSpeed);
        transform.forward = Vector3.Lerp(transform.forward, move, m_TurnSpeed * Time.deltaTime);
        m_Animator.SetBool("Idle", false);
    }
}
