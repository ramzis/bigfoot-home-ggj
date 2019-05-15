using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    Ray ray;                                   
    RaycastHit hit;                            
    int layerMask = 1 << 9;
    float range = 5.5f;
    Collider[] enemies;
    Animator m_Animator;

    public Action OnAttack;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("AnimateAttack");
        }
    }
    
    void ProcessAttack()
    {
        // Debug.Log("Processing attack");
        enemies = Physics.OverlapSphere(transform.position, range, layerMask);

        foreach (var enemy in enemies)
        {
            House house = enemy.GetComponentInParent<House>();
            if(house)
            {
                // Debug.Log("Damaging " + enemy.name);
                house.TakeDamage(damage);
            }
        }

        if(OnAttack != null)
        {
            OnAttack.Invoke();
        }
    }

    IEnumerator AnimateAttack()
    {
        m_Animator.SetBool("Kicking", true);
        yield return new WaitForSeconds(0.15f);
        ProcessAttack();
        yield return new WaitForSeconds(0.18f);
        m_Animator.SetBool("Kicking", false);
    }
}
