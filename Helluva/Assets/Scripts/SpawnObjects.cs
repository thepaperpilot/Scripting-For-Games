using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {

	public int numObjects = 10;
	public int range = 20;
	public GameObject[] prefabs;

	void Awake() {
		for (int i = 0; i < numObjects; i++) {
			Instantiate (prefabs [Random.Range (0, prefabs.Length)], new Vector3 (Random.Range (-range, range), 10, Random.Range (-range, range)), Random.rotation);
		}
	}
}
