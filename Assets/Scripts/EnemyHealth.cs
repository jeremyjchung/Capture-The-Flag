using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

	public int health;
	public float sinkSpeed = 0.5f;
	public float timeBeforeSink = 2.25f;
	public AudioClip deathSound;
	public AudioClip hurtSound;

	float timer;
	Animator anim;
	NavMeshAgent agent;
	Rigidbody rb;
	SphereCollider sphereRadius;
	ParticleSystem fluffParticles;
	AudioSource audio;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		rb = GetComponent<Rigidbody> ();
		sphereRadius = GetComponent<SphereCollider> ();
		fluffParticles = GetComponent<ParticleSystem> ();
		audio = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		timer += Time.deltaTime;
		if (health <= 0 && timer > timeBeforeSink) {
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}

	public void TakeDamage (int damage) {
		fluffParticles.Stop ();
		fluffParticles.Play ();

		audio.Stop ();
		audio.clip = hurtSound;
		audio.Play ();

		health -= damage;
		sphereRadius.radius = 150;
		if (health <= 0) {
			Death ();
		}
	}

	public void Death () {
		anim.SetTrigger ("Dead");

		audio.Stop ();
		audio.clip = deathSound;
		audio.Play ();

		GetComponent<CapsuleCollider> ().enabled = false;
		rb.isKinematic = true;
		agent.enabled = false;
		timer = 0;
		Destroy (gameObject, 20.0f);
	}
}

