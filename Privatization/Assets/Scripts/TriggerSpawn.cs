using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn : MonoBehaviour {

	public GameObject objectToSpawn;
	public Transform[] spawnLocations;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (spawnLocations.Length == 0) {
				Debug.Log ("spawnLocations empty on object " + gameObject);
			} else {
				SpawnObjectRandomLocation ();
			}
		}
	}

	void SpawnObjectRandomLocation() {
		Transform tf = spawnLocations [Random.Range (0, spawnLocations.Length)];
		Instantiate (objectToSpawn, tf.position, tf.rotation);
	}
}
