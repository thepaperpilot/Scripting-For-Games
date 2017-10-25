using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int currentHealth = 100;

	public void TakeDamage(int amount) {
		currentHealth -= amount;
		if (currentHealth < 0)
			currentHealth = 0;
		Debug.Log (currentHealth);
	}
}
