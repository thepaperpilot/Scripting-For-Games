using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public int startingHealth = 10;
	public int maxHealth = 20;
	public Text healthText;
	public ParticleSystem healParticles;

	private int health;

	void Awake() {
		health = startingHealth;
		UpdateHealthGUI ();
	}

	public void Heal(int amount) {
		if (health < maxHealth) {
			health = Mathf.Min(maxHealth, health + amount);
			UpdateHealthGUI ();
			healParticles.transform.position = transform.position;
			healParticles.Play ();
			Debug.Log ("New Health: " + health);
		} else
			Debug.Log ("Already at maximum health.");
	}

	void UpdateHealthGUI() {
		healthText.text = health + "/" + maxHealth;
	}
}
