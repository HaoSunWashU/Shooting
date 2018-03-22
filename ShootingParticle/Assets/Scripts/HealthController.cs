using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	[SerializeField] private float health = 100f;

	public ParticleSystem damageFire;

	public void ApplyDamage(float damage){
		health -= damage;

		if (health <= 0) {
			Destroy (gameObject);

		}
	
	}
}
