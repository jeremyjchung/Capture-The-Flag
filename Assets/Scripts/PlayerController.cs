using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
	Animator anim;
	Vector3 velocity;
	int shootableMask;
	float camRayLength;
	PlayerHealth health;

	public int speedFactor;

	void Awake () {
		health = GetComponent<PlayerHealth> ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		camRayLength = 100.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (health.health > 0) {
			float forwards = Input.GetAxisRaw ("Vertical");
			float horizontal = Input.GetAxisRaw ("Horizontal");

			Move (forwards, horizontal);
			Turn ();
			Animate (forwards, horizontal);
		}
	}

	void Animate (float forwards, float horizontal) {
		anim.SetFloat ("Speed", Mathf.Abs (forwards) + Mathf.Abs (horizontal));
	}

	void Move (float forwards, float horizontal) { 
		Vector3 movement = new Vector3 ();

		if (forwards != 0) {
			movement += transform.forward * forwards;
		}
		if (horizontal != 0) {
			movement += transform.right * horizontal;
		}

		velocity = movement.normalized * speedFactor * Time.deltaTime;
		rb.MovePosition (transform.position + velocity);
	}

	void Turn () {
		
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		Vector3 playerToMouse = camRay.GetPoint(camRayLength) - transform.position;
		playerToMouse.y = 0.0f;

		Quaternion rotateTo = Quaternion.LookRotation (playerToMouse);
		rb.MoveRotation (rotateTo);

	}
}
