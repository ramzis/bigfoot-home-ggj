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
    float range = 1f;
    Collider[] enemies;

    public Action OnAttack;

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            enemies = Physics.OverlapSphere(transform.position, range, layerMask);

            foreach (var enemy in enemies)
            {
                House house = enemy.GetComponentInParent<House>();
                if(house)
                {
                    Debug.Log("Damaging " + enemy.name);
                    house.TakeDamage(damage);
                }
            }

            if(OnAttack != null)
            {
                OnAttack.Invoke();
            }
        }
    }
}
