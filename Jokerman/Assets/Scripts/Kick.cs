using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour {

	public GameObject ballPrefab;
	public GameObject kicker;
	public bool autoKick = false;

	private bool kickBall = false;

	void Start() {
		if (autoKick) {
			StartCoroutine ("AutoKick");
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			kickBall = true;
		}
	}

	void FixedUpdate() {
		if (kickBall) {
			kickBall = false;
			GameObject ball = Instantiate (ballPrefab, kicker.transform.position + Vector3.up, Quaternion.identity);
			Rigidbody rb = ball.GetComponent<Rigidbody> ();
			rb.AddForce (-1000, 500, 0);
			rb.AddTorque (Random.Range (-1000f, 1000f), Random.Range (-1000f, 1000f), Random.Range (-1000f, 1000f));
		}
	}

	IEnumerator AutoKick() {
		while (true) {
			kickBall = true;
			yield return new WaitForSeconds(Random.Range(2, 5));
		}
	}
}
