using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour {

	public float strength = 2; // How much to slow the player by

	// Start slowing player down
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Rigidbody>().drag += strength;
		}
	}

	// Stop slowing player down
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Rigidbody>().drag -= strength;
		}
	}
}
