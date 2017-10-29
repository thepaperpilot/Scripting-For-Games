using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPound : MonoBehaviour {

    // Note: This class needs EnemyStep to be placed on the body of the enemy
    // Also, it used to be a subclass of EnemyAim before I realized we're supposed to use NavMeshAgent
    // and I basically needed to rewrite this entire class, because before I was using my own AI

    public float rotSpeed = 100;    // Speed at which the enemy can rotate towards the player
    public float viewRange = 100;   // How far away the enemy can see the player
    
    private Transform target;                  // The player
    private int layerMask;                     // The layers our enemy can see
    private NavMeshAgent agent;                // The AI that makes us move towards the enemy
    private bool isPatrolling;                 // Whether or not we're currently patrolling

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = (1 << LayerMask.NameToLayer("Environment")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("InvisibleToEnemy")) | (1 << LayerMask.NameToLayer("Door"));
    }

    // Originally I wanted it to only pound while not rotating, but that would've
    // been really hard to do with NavMeshAgent, so due to time constraints,
    // it could not be done 

    void FixedUpdate()
    {
        RaycastHit hit;
        // Shoot a raycast at the player, and ensure it hits the player
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, viewRange, layerMask) && hit.collider.gameObject.CompareTag("Player"))
        {
            CanSeeTarget();
        }
        else
        {
            CannotSeeTarget();
        }
    }

    // Called every frame the player is visible to the enemy
    void CanSeeTarget()
    {
        agent.SetDestination(target.position);
        StopCoroutine("Patrol");
        isPatrolling = false;
    }

    // If we can't see the player, patrol
    void CannotSeeTarget()
    {
        if (!isPatrolling)
            StartCoroutine("Patrol");
        isPatrolling = true;
    }

    // Moves to a new location it can reach
    IEnumerator Patrol()
    {
        while (true)
        {
            // Choose direction to patrol
            Vector3 randDirection = Random.insideUnitSphere * 100;
            randDirection += transform.position;

            // Find a position to navigate to, in that direction
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, 100, -1);

            // Pound in that direction
            agent.SetDestination(navHit.position);
            yield return new WaitForSeconds(2);
        }
    }
}
