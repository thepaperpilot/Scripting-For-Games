using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	
	public GameObject muzzle;
	public int speed = 1;
	public int rotSpeed = 1;

	private Rigidbody rb;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		UpdateTank ();
	}

	void UpdateTank() {
		// Move it
		transform.Translate (new Vector3 (0, 0, Input.GetAxis ("Vertical") * speed * Time.deltaTime));
		// Rotate it
		transform.Rotate (Vector3.up, Input.GetAxis ("Horizontal") * rotSpeed * Time.deltaTime);
		// If space is held, turn on muzzle flash
		muzzle.SetActive (Input.GetKey (KeyCode.Space));
	}
}
