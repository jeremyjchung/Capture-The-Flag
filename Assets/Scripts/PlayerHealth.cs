using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthBar {
	public float warningThreshold = 50f;
	public float dangerThreshold = 20f;
	public Slider slider;
	public Image colorImage;
}

public class PlayerHealth : MonoBehaviour {

	public float maxHealth = 100f;
	public float healingTimer = 5f;
	public float healingRate = 0.01f;
	public bool alive = true;

	public HealthBar healthBar;

	public float flashSpeed = 10.0f;
	public Image damageImage;
	public Color flashColor;

	public AudioClip hurtSound;
	public AudioClip deathSound;

	[HideInInspector] 
	public float health;

	bool hit;
	float timer;
	Animator anim;
	AudioSource audio;

	void Awake () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		timer = 0;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(hit) {
			damageImage.color = flashColor;
		}
		else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		if (health < maxHealth && timer >= healingTimer) {
			Heal ();
		}

		hit = false;

		if (health < healthBar.dangerThreshold) {
			healthBar.colorImage.color = Color.red;
		} else if (health < healthBar.warningThreshold) {
			healthBar.colorImage.color = Color.yellow;
		} else {
			healthBar.colorImage.color = Color.cyan;
		}

		healthBar.slider.value = health;
	}

	void Heal () {
		health += healingRate;
	}

	void Death () {
		anim.SetTrigger ("Dead");
		alive = false;

		audio.Stop ();
		audio.clip = deathSound;
		audio.Play ();

		GetComponent<Rigidbody> ().isKinematic = true;
		Destroy (gameObject, 10.0f);
	}

	public void TakeDamage (float damage) {
		timer = 0;
		health -= damage;
		hit = true;

		audio.Stop ();
		audio.clip = hurtSound;
		audio.Play ();

		if (health <= 0) {
			Death ();
		}
	}
		
}
