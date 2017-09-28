using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

	public int damagePerShot = 10;
	public float fireRate;
	public float fireRange;

	float timer;
	float shotDisplayTime;

	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;

	ParticleSystem gunParticles;
	LineRenderer gunLine; 
	AudioSource gunAudio;
	Light gunLight;
	GameObject player;
	PlayerHealth playerHealth;

	void Awake () {
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent<PlayerHealth> ();
	}

	// Use this for initialization
	void Start () {
		timer = 0.0f;
		shotDisplayTime = 0.15f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (player != null && playerHealth.alive) {
			if (timer >= fireRate && Input.GetKey (KeyCode.Space)) {
				Shoot ();
			}

			if (timer >= fireRate * shotDisplayTime) {
				DisableBullets ();
			}
		}
	}

	void DisableBullets () {
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	void Shoot () {
		timer = 0.0f;

		gunAudio.Play ();
		gunLight.enabled = true;
		gunParticles.Stop ();
		gunParticles.Play ();
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if(Physics.Raycast (shootRay, out shootHit, fireRange, shootableMask))
		{
			// Set the second position of the line renderer to the point the raycast hit.
			gunLine.SetPosition (1, shootHit.point);

			GameObject solidObject = shootHit.collider.gameObject;
			if (solidObject.tag.Equals ("Enemy")) {
				solidObject.GetComponent<EnemyHealth> ().TakeDamage (damagePerShot);
			}

		}
		// If the raycast didn't hit anything on the shootable layer...
		else
		{
			// ... set the second position of the line renderer to the fullest extent of the gun's range.
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * fireRange);
		}

	}
}
