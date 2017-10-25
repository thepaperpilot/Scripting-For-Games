using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour {

	public Rigidbody trigger;

	void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody == trigger)
			GetComponent<MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		else
			other.gameObject.SetActive (false);
	}
}
