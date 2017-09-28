using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static float rotationSensitivity = 5.0f;
	public GameObject hellephant;
	public GameObject zombunny;
	public Transform[] spawnPositions;
	public int zombunnyPercentageSplit;
	public float spawnOffset;

	private float timer;
	private GameObject player;
	private PlayerHealth playerHealth;
	private static bool gameOver;

	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.gameObject.GetComponent<PlayerHealth> ();	
	}
	// Use this for initialization
	void Start () {
		timer = 0.0f;
		SpawnEnemies ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (!gameOver && timer >= spawnOffset) {
			timer = 0;
			SpawnEnemies ();
		}
	}

	void SpawnEnemies () {

		for (int i = 0; i < spawnPositions.Length; i++) {
			int val = Random.Range (1, 100);
			if (val <= zombunnyPercentageSplit) {
				Instantiate (zombunny, spawnPositions [i]);
			} else {
				Instantiate (hellephant, spawnPositions [i]);
			}
		}
	}

	public static void GameOver() {
		gameOver = true;


	}
}
