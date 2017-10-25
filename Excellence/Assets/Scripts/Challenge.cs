using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Challenge : MonoBehaviour {

	public Light myLight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Decreases light range by pressing Q
		// Also, by holding down shift it will constantly decrease range
		if (Input.GetKeyDown (KeyCode.Q) || ShiftModifier(KeyCode.Q)) {
			myLight.range -= 1;
		}

		// Increases light range by pressing E
		// Also, by holding down shift it will constantly increase range
		if (Input.GetKeyDown (KeyCode.E) || ShiftModifier(KeyCode.E)) {
			myLight.range += 1;
		}

		// Changes light color to a random color
		if (Input.GetKeyDown (KeyCode.Space)) {
			myLight.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		}

		// Exit the game by pressing escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Switch Scenes
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene (0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SceneManager.LoadScene (2);
		}
	}

	bool ShiftModifier (KeyCode key) {
		return (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && Input.GetKey (key);
	}
}
