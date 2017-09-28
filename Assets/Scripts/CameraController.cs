using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject player;
	Transform playerTransform;

	public float offset;
	public float height;
	public float turnOffset;

	const float halfTurn = 180f;
	const float fullTurn = 360f;
	Vector3 heightVector;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = player.transform;
	}

	// Use this for initialization
	void Start () {
		heightVector = new Vector3 (0, height, 0);
		transform.rotation = Quaternion.LookRotation ( 
			new Vector3 (
				10.0f,
				playerTransform.rotation.y,
				0.0f
			)
		);
	}

	void FixedUpdate () {
		if (player != null) {
			Turn ();
			Follow ();
		}
	}

	void Follow () {
		transform.position = playerTransform.position - playerTransform.forward * offset + heightVector;
	}

	void Turn () {
		if (Quaternion.Angle (transform.rotation, playerTransform.rotation) > turnOffset) {
			transform.rotation = Quaternion.Slerp (
				transform.rotation, playerTransform.rotation, Time.deltaTime * GameController.rotationSensitivity
			);
		}
	}
}
