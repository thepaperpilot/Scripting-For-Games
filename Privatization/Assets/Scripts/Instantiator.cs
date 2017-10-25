using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour {

	public GameObject objectToInstantiate;
	public Transform spawnLoc;
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			GameObject gObject = Instantiate (objectToInstantiate, spawnLoc.position, spawnLoc.rotation);
			gObject.GetComponent<Rigidbody> ().AddForce (Vector3.up);
		}
	}
}
