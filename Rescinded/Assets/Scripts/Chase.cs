using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour {

	public Transform target;

	NavMeshAgent agent;

	void Awake() {
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update() {
		agent.SetDestination (target.position);
	}
}
