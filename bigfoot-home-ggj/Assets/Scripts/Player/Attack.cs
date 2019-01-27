using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    Ray ray;                                   
    RaycastHit hit;                            
    int layerMask = 1 << 9;
    float range = 1f;
    Collider[] enemies;

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
                    Debug.Log("Damaging");
                    house.TakeDamage(100f);
                }
            }
        }
    }
}
