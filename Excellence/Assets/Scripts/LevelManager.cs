using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Material materialToChange;
	public Light lightToChange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			ChangeLight();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			ChangeMaterial ();
		}

		// Exit the game by pressing escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Switch Scenes
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene (1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SceneManager.LoadScene (2);
		}
	}

	void ChangeLight() {
		lightToChange.type = lightToChange.type == LightType.Point ? LightType.Spot : LightType.Point;
	}

	void ChangeMaterial() {
		materialToChange.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
	}
}
