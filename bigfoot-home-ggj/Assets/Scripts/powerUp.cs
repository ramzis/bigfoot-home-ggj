using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour {
    public string power;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        if (this.GetComponent<Explosion>().explosion.isPlaying)
        {
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            if(this.GetComponent<Explosion>().explosion.isStopped)
            {
                Destroy(this.gameObject);
            }
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(power == "explode")
            {
                this.GetComponent<Explosion>().explode();
                //Destroy(this.gameObject);
            }
        }
    }
}
