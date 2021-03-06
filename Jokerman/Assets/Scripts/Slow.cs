using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour {

	public float strength = 2;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Rigidbody> ().drag += strength;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Rigidbody> ().drag -= strength;
		}
	}
}
