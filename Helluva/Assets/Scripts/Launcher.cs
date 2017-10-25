using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

	public string[] tags = new string[]{"Player"}; // Will only launch objects with these tags
	public float speed = 1f;

	void OnTriggerStay(Collider other) {
		for (int i = 0; i < tags.Length; i++) {
			if (other.gameObject.CompareTag (tags [i])) {
				other.gameObject.transform.Translate (0, speed * Time.deltaTime, 0);
				break;
			}
		}
	}
}
