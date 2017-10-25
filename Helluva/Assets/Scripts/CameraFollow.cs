using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject camera;
	public Vector3 offset = new Vector3(0, 5, -5);

	void Update () {
		camera.transform.position = transform.position + offset;
		camera.transform.LookAt (transform.position);
	}
}
