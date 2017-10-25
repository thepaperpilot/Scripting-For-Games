using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	public float strength = 10f;

	void OnTriggerEnter(Collider other) {
		// Not Working!
		Rigidbody rb = other.GetComponent<Rigidbody> ();
		// Not accurate for player
		// I should be using forces for player movement
		Debug.Log (rb.velocity);
		rb.AddForce (rb.velocity * -1 * strength);
	}
}
