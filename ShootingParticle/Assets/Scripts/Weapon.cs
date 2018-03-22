using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	private Animator anim;
	private AudioSource _AudioSource;


	public float range = 100f;				//Max range of bullet
	public int bulletsPerMag = 30;	
	public int bulletsLeft = 200;   		//total bullets left
	public int currentBullets;				//The current bulltets in our magazine
	public float hitForce = 500f;	
	public Transform shootPoint;			//gun shooting point. The point from which the bullet leaves the muzzle
	public ParticleSystem muzzleFlash;
	public AudioClip shootSound;

	public GameObject hitParticles;
	public GameObject bulletImpact;

	public float fireRate = 0.5f;
	private bool isReloading;

	public float damage = 20f;

	float fireTimer;						//Time counter for the delay

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		currentBullets = bulletsPerMag;

		_AudioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("Fire1")) {
			if (currentBullets > 0)
				Fire ();
			else if(bulletsLeft > 0)
				DoReload ();
//				Reload ();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			if(currentBullets < bulletsPerMag && bulletsLeft > 0)
			DoReload ();
		}

		if (fireTimer < fireRate) {
			fireTimer += Time.deltaTime;
		}
	}

	void FixedUpdate(){
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);  //first layer in animator

		isReloading = info.IsName ("Reload");
		if (info.IsName ("Fire"))
			anim.SetBool ("Fire", false);

	}

	private void Fire(){
	
		if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
			return; //cannot shot

		RaycastHit hit;

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, range)) {
			Debug.Log (hit.transform.name + "found!");

			GameObject hitParticleEffect = Instantiate (hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

			// Get a reference to a health script attached to the collider we hit
			if(hit.transform.GetComponent<HealthController>()){
				hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
				// Check if the object we hit has a rigidbody attached
				if (hit.rigidbody != null)
				{
					// Add force to the rigidbody we hit, in the direction from which it was hit
					hit.rigidbody.AddForce (-hit.normal * hitForce);
				}
			}


			Destroy (hitParticleEffect, 2f);
			Destroy (bulletHole, 3f);


		}

		anim.CrossFadeInFixedTime ("Fire", 0.05f);  //play the fire animation
		muzzleFlash.Play();							//play the muzzle flash
		PlayShootSound ();							//play the shooting sound

//		anim.SetBool ("Fire", true);
		currentBullets--;

		fireTimer = 0.0f; //Reset fire timer
	}

	public void Reload(){
		if (bulletsLeft <= 0) {
			return;
		}

		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;
	}

	private void DoReload(){
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);  //first layer in animator

		if (isReloading)
			return; // if already in the reload state, do nothing
		anim.CrossFadeInFixedTime ("Reload", 0.01f);
	
	}

	private void PlayShootSound(){
		_AudioSource.PlayOneShot (shootSound);
//		_AudioSource.clip = shootSound;
//		_AudioSource.Play ();
	}

}
