using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement_Hunter : MonoBehaviour {

	public float chasingRange;

	GameObject player;
	NavMeshAgent agent;
	bool inRange;

	void Awake() {
		agent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update() {
		if (inRange) {
			agent.isStopped = false;
			agent.SetDestination (player.transform.position);
		} else {
			agent.isStopped = true;
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other);
		if (other.CompareTag ("Player")) {
			Debug.Log ("In Range");
			inRange = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("Player")) {
			Debug.Log ("Out Range");
			inRange = false;
		}
	}
}
