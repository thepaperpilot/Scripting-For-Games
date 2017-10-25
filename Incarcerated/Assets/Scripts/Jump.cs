using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

	public float strength = 1000; // How much force to apply to the player

	// Make player jump
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * strength);
			Destroy (transform.parent.gameObject);
		}
	}
}
