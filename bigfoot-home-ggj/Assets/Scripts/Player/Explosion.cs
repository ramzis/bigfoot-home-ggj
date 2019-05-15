using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public ParticleSystem explosion;
    public float range = 4f;
    public float damage = 100f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void explode()
    {
        explosion.Play();
        Collider[] enemies;
        int layerMask = 1 << 9;
        enemies = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach (var enemy in enemies)
        {
            House house = enemy.GetComponentInParent<House>();
            if (house)
            {
                //Debug.Log("ExplosionDamaging");
                house.TakeDamage(damage);
            }
        }
    }
}
