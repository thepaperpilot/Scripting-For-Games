using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSRaycasting : MonoBehaviour {

	public float distance = 10;
	public float duration = 10;
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			DebugRay ();
			ShootRay ();
		}
	}

	void DebugRay() {
		Vector3 endPoint = transform.forward * distance;
		Debug.DrawRay (transform.position, endPoint, Color.cyan, duration);
	}

	void ShootRay() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && hit.collider.gameObject.CompareTag("Enemy")) {
			Debug.Log("You hit the enemy!");
		}
	}
}
