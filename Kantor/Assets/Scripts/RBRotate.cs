using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RBRotate : MonoBehaviour {

	public float turnSpeed = 1;

	Rigidbody rb;
	private float yTurn;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		yTurn = Input.GetAxis ("Horizontal") * turnSpeed;
	}

	void FixedUpdate() {
		rb.MoveRotation (rb.rotation * Quaternion.Euler (0, yTurn, 0));
	}
}
