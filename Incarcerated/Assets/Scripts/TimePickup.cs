using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : MonoBehaviour {

	public LevelManager lm;        // Reference to the level manager
	public float timeToRemove = 2; // How much time to remove from the time taken

	// Remove time
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			lm.RemoveTime(timeToRemove);
			Destroy (transform.parent.gameObject);
		}
	}
}
