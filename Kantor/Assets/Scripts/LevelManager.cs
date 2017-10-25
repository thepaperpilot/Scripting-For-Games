using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject winObject;
	public float displayDuration = 1;
	public GameObject bullet; // Prefab

	float timer = 0; // Used for timer version

	void Start() {
		StartCoroutine ("SpawnBullet");
	}

	// Prof spent a lot of time optimizing this, even though its already "fast enough"
	// Donald Knuth told me not to prematurely optimize, and besides, typing on these
	// keyboard is annoying enough as it is, I'm not going to write *extra* code
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				// Coroutine Version
				StopCoroutine ("DisplayText");
				StartCoroutine ("DisplayText");
			} else {
				// Timer Version
				timer = displayDuration;
				winObject.SetActive (true);
			}
		}
		// Stuff for timer version
		if (timer > 0) {
			timer -= Time.deltaTime;
			if (timer == 0) {
				winObject.SetActive (false);
			}
		} else if (timer < 0) {
			timer = 0;
			winObject.SetActive (false);
		}
	}

	// Stuff for coroutine version
	// Wow, look how much less code it is, and it doesn't add an extra variable, and the logic is independent from Update()!
	// In terms of code smell, this is waaaaay better. Prof proposed an alternative to calling "stop coroutine" by adding
	// another boolean for isDisplayActive, and adding a conditional for it, but that makes it smellier. 
	IEnumerator DisplayText() {
		winObject.SetActive (true);
		yield return new WaitForSeconds (displayDuration);
		winObject.SetActive (false);
	}

	IEnumerator SpawnBullet() {
		while (true) {
			Instantiate (bullet, new Vector3 (0, 0, 0), Random.rotation);
			yield return new WaitForSeconds (Random.Range (1, 3));
		}
	}
}
