using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPad : MonoBehaviour {

	public int healAmount = 10;

	private Player player;

	void Awake() {
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
	}

	void OnTriggerEnter(Collider other) {
		player.Heal (healAmount);
	}
}
