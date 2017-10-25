using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Tank : MonoBehaviour {

	public Vector3 cameraOffset = new Vector3(0, .5f, .5f);
	public GameObject camera;
	public float moveSpeed = 1;
	public float rotSpeed = 2;
	public GameObject bullet; // prefab
	public float cooldownDuration = 1;
	public float kickback = 1;

	private float move;
	private float rotate;
	private bool canFire = true;
	Rigidbody rb;
	MeshRenderer mr;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		mr = GetComponent<MeshRenderer> ();
	}

	void Update () {
		move = Input.GetAxis ("Vertical") * moveSpeed;
		rotate = Input.GetAxis ("Horizontal") * rotSpeed;
		camera.transform.position = transform.position + cameraOffset;

		if (Input.GetKeyDown (KeyCode.Space) && canFire) {
			StartCoroutine ("Cooldown");
			Instantiate (bullet, transform.position + new Vector3(0, .5f, 0) + transform.forward * .7f, transform.rotation);
			rb.AddForce (transform.forward * -1 * kickback);
		}
	}

	void FixedUpdate() {
		rb.MovePosition (transform.position + transform.forward * move);
		rb.MoveRotation (transform.rotation * Quaternion.Euler (0, rotate, 0));
	}

	IEnumerator Cooldown() {
		canFire = false;
		mr.material.color = Color.red;
		yield return new WaitForSeconds (cooldownDuration / 3f);
		mr.material.color = new Color(1, .5f, 0); // orange
		yield return new WaitForSeconds (cooldownDuration / 3f);
		mr.material.color = Color.yellow;
		yield return new WaitForSeconds (cooldownDuration / 3f);
		mr.material.color = Color.green;
		canFire = true;
	}
}
