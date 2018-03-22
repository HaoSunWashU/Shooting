using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float delay = 3f;
	public float radius = 5f;
	public float explosionForce = 700f;
	public ParticleSystem explosionEffect;
	public ParticleSystem fireEffect;

	bool hasExploded = false;

	float countdown;

	// Use this for initialization
	void Start () {
		countdown = delay;
	}
	
	// Update is called once per frame
	void Update () {
//		countdown -= Time.deltaTime;
//
//		if (countdown <= 0f && !hasExploded) {
//			hasExploded = true;
		//			Explode();
//		}

		if (Input.GetKeyDown (KeyCode.G)) {
			Explode ();
		}
	}

	void Explode(){
		//show effect
		ParticleSystem particle1 = Instantiate (explosionEffect, gameObject.transform.position, Quaternion.Euler(-90,0,0));
		ParticleSystem particle2 = Instantiate (fireEffect, gameObject.transform.position, Quaternion.Euler(-90,0,0));


		Collider[] colliders = Physics.OverlapSphere (transform.position, radius);

		foreach (Collider nearbyObject in colliders) {
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();
			if(rb != null){
				rb.AddExplosionForce(explosionForce, transform.position, radius);
			}
		
		}
		//get nearby objects
			//add force
			//Damage


		Destroy (gameObject);

	}
}
