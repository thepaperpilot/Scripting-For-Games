using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour {

	public float distance = 10f;
	public float effect = 100f;
	public bool drawRay = false;

	Rigidbody rb;
	MeshRenderer meshRenderer;
	bool launchedParachute = false;
	bool isGrounded = false;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
		meshRenderer = GetComponent<MeshRenderer> ();
	}

	void Update () {
		if (drawRay)
			Debug.DrawRay (transform.position, Vector3.down * distance, Color.cyan, .01f);
		RaycastHit hit;
		if (!launchedParachute && !isGrounded && Physics.Raycast(transform.position, Vector3.down, out hit, distance)) {
			rb.drag *= effect;
			launchedParachute = true;
			meshRenderer.material.color = Color.red;
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (launchedParachute && coll.gameObject.CompareTag ("Floor")) {
			rb.drag /= effect;
			launchedParachute = false;
			isGrounded = true;
			meshRenderer.material.color = Color.green;
		}
	}

	void OnCollisionEexit(Collision coll) {
		if (isGrounded && coll.gameObject.CompareTag ("Floor")) {
			isGrounded = false;
			meshRenderer.material.color = Color.blue;
		}
	}
}
