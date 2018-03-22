using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	[SerializeField] private float health = 100f;

	public ParticleSystem damageFire;

	public void ApplyDamage(float damage){
		health -= damage;

		if (health <= 0) {
			ParticleSystem particle = Instantiate (damageFire, gameObject.transform.position, Quaternion.Euler(-90,0,0));
			particle.Play ();
			Destroy (gameObject);

		}
	
	}
}
