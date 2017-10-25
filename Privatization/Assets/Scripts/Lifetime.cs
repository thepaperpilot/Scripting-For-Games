using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {

	public float duration = 2f;


	void Start () {
		Destroy (gameObject, duration);
	}
}
