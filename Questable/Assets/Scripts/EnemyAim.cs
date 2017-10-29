using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour {

    public float rotSpeed = 5;      // Speed at which the enemy can rotate towards the player
    public float viewRange = 100;   // How far away the enemy can see the player

    protected Transform target;       // The player
    protected LayerMask layerMask;    // What layers the enemy can see

    protected void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // The following line uses bit shifting because I don't like the LayerMask objects in the Unity interface
        // You have to open the dropdown for every item you want to add :/
        // Besides, this is scripting for games. Not "Force yourself to use the unity interface" class
        layerMask = (1 << LayerMask.NameToLayer("Environment")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("InvisibleToEnemy")) | (1 << LayerMask.NameToLayer("Door"));
    }

    void Update()
    {
        RaycastHit hit;
        // Shoot a raycast at the player, and ensure it hits the player
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, viewRange, layerMask) && hit.collider.gameObject.CompareTag("Player"))
        {
            // Find how close we are to looking at the player
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.position - transform.position));
            // Originally I used Quaternion.Slerp for this function, and it doesn't work, so here's an explaination why:
            // The percentage value is always approximately the same (assuming deltaTime is always approximately the same)
            // but the distance between the current rotation and the target rotation is not- it gets smaller as we rotate towards it
            // This meant that it would slow down as it approached its target rotation, whereas the intended result was a constant speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotSpeed * Time.deltaTime);
            if (angle <= rotSpeed * Time.deltaTime)
            {
                LookingAtTarget();
            }
            else
            {
                CanSeeTarget();
            }
        }
        else
        {
            CannotSeeTarget();
        }
    }

    // Fired when we can see the target and have rotated to look at them
    protected virtual void LookingAtTarget()
    {

    }

    // Fired when we can see the target but are still rotating to look at them
    protected virtual void CanSeeTarget()
    {

    }

    // Fired when we cannot see the target
    protected virtual void CannotSeeTarget()
    {

    }
}
