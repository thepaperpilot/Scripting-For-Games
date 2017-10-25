using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

	public static void CheckSceneManager() {
		// Quit game using Escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Switch scenes using number keys
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		}
	}

	void Update() {
		CheckSceneManager ();
	}
}
