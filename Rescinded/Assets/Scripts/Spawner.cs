using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] prefabs;
	public Transform[] locations;
	public float[] probabilities;

	void Update() {
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown(KeyCode.G)) {
			float sum = 0;
			for (int i = 0; i < probabilities.Length; i++)
				sum += probabilities[i];
			float random = Random.Range (0f, sum);
			sum = 0;
			GameObject prefab = null;
			for (int i = 0; i < probabilities.Length; i++) {
				if (random >= sum && random < sum + probabilities [i]) {
					prefab = prefabs[i];
					break;
				}
				sum += probabilities [i];
			}
			if (prefab != null) {
				Transform location = locations [Random.Range (0, locations.Length)];
				GameObject instance = Instantiate (prefab, location.position, location.rotation);
				Destroy (instance, 2);
			}
		}
	}
}
