using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public GameObject parent; // Parent for prefabs, e.g. Canvas for UI elements
	public GameObject winObject; // Prefab

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Ball")) {
			// Create win object, and destroy it one second later
			GameObject gameObject = Instantiate(winObject, parent.transform);
			Destroy (gameObject, 1);
		}
	}
}
