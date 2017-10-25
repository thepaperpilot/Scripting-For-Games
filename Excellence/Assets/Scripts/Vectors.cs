using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Vectors : MonoBehaviour {

	public Text text;
	public GameObject ball;
	public GameObject cube;
	public Light spotlight;
	public float speed = 16f;
	public float friction = 5f;

	Vector3 velocity = new Vector3(0, 0, 0);
	GameObject selected;

	// Use this for initialization
	void Start () {	
		selected = ball;
	}
	
	// Update is called once per frame
	void Update () {
		// Select Object
		if (Input.GetKeyDown (KeyCode.C)) {
			selected = cube;
			velocity.Set (0, 0, 0);
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			selected = ball;
			velocity.Set (0, 0, 0);
		}

		// Display object position
		text.text = (selected == cube ? "Cube" : "Ball") + "\n" + selected.transform.position.ToString();

		// Friction
		velocity *= 1 / (1 + (Time.deltaTime * friction));
		if (velocity.magnitude < 0.01) velocity.Set(0, 0, 0);

		// Update Velocity from Input
		velocity.x += Input.GetAxis("Horizontal") * speed;
		velocity.z += Input.GetAxis("Vertical") * speed;

		// Apply velocity
		selected.transform.Translate(velocity * Time.deltaTime);
		if (selected.transform.position.y < 0) {
			Vector3 position = selected.transform.position;
			selected.transform.position = new Vector3(position.x, 2, position.z);
		}

		// Make spotlight move
		Quaternion rotation = spotlight.transform.rotation;
		spotlight.transform.rotation = new Quaternion (rotation.x, Mathf.Sin (Time.time) / 4f, rotation.z, rotation.w);

		// Reset Position and Velocity if Space is pressed
		if (Input.GetKeyDown (KeyCode.Space)) {
			selected.transform.position = new Vector3(0, 2f, 0);
			velocity.Set (0, 0, 0);
		}

		// Set rotation using a lot of keys
		if (Input.GetKey (KeyCode.U)) {
			cube.transform.Rotate (-10f, 0, 0);
		}
		if (Input.GetKey (KeyCode.I)) {
			cube.transform.Rotate (10f, 0, 0);
		}
		if (Input.GetKey (KeyCode.J)) {
			cube.transform.Rotate (0, -10f, 0);
		}
		if (Input.GetKey (KeyCode.K)) {
			cube.transform.Rotate (0, 10f, 0);
		}
		if (Input.GetKey (KeyCode.N)) {
			cube.transform.Rotate (0, 0, -10f);
		}
		if (Input.GetKey (KeyCode.M)) {
			cube.transform.Rotate (0, 0, 10f);
		}

		// Exit the game by pressing escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Switch Scenes
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene (0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene (1);
		}		
	}
}
