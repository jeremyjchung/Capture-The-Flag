using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public float normalSpeed;
	public float excitedSpeed;
	public float stoppingDistance;
	public float damage;
	public float timeBetweenAttacks;

	float timer;
	Animator anim;
	SphereCollider sphereRadius;
	GameObject player;
	PlayerHealth playerHealth;
	NavMeshAgent agent;
	NavMeshHit hit;
	bool chase;
	bool dealDamage;

	void Awake () {
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		sphereRadius = GetComponent<SphereCollider> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.gameObject.GetComponent<PlayerHealth> ();
	}

	// Use this for initialization
	void Start () {
		timer = 0f;
		chase = false;
		dealDamage = false;

		agent.stoppingDistance = stoppingDistance;
		agent.speed = normalSpeed;
		anim.SetFloat ("Speed", agent.speed);

		SetRoamingDestination ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (player == null) {
			dealDamage = false;
			chase = false;
		}

		if (agent.isActiveAndEnabled) {
			if (chase) {
				SetChaseDestination ();
			} else {
				if (agent.remainingDistance <= 1.2 * stoppingDistance) {
					SetRoamingDestination ();
				}
			}
		}

		if (dealDamage && timer > timeBetweenAttacks) {
			DealDamage ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			chase = true;
			agent.speed = excitedSpeed;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			chase = false;
			agent.speed = normalSpeed;
		}
	}

	void SetChaseDestination () {
		agent.SetDestination (player.transform.position);
	}

	void SetRoamingDestination () {
		Vector3 movement = new Vector3 (
			Random.Range (-sphereRadius.radius, sphereRadius.radius), 
			0.0f, 
			Random.Range (-sphereRadius.radius, sphereRadius.radius));
		NavMesh.SamplePosition (transform.position + movement, out hit, sphereRadius.radius, NavMesh.AllAreas);
		agent.SetDestination (hit.position);

	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			dealDamage = true;
		}
	}

	void OnCollisionExit (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			dealDamage = false;
		}
	}

	void DealDamage () {
		timer = 0f;

		if (playerHealth.alive) {
			playerHealth.TakeDamage (damage);
		}
	}
}
