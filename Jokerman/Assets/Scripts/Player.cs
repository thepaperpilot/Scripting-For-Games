using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 250;
	public GameObject camera;
	public Vector3 cameraOffset = new Vector3(0, 5, -10);

	private Rigidbody rb;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (horizontal, 0, vertical);
		rb.AddForce (movement * speed);
	}

	void LateUpdate() {
		camera.transform.position = transform.position + cameraOffset;
		camera.transform.LookAt (transform.position);
	}
}
