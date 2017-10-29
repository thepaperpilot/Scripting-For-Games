using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FastObjectTrigger : MonoBehaviour {

    // This exists because my bullets were going really fast
    // (because that makes it more fun)
    // and it wouldn't collide with some triggers I had set up,
    // even when I set it to continuous collision detection.

    // SO, I wrote this, basically using raycasts to detect
    // any triggers we passed between the last frame and
    // now, and sending a message to the object.

    // It's important that it uses LateUpdate, because I was
    // using this to remove object after triggering something,
    // and if it was in FixedUpdate (or Update), then it would
    // still render the object that frame, and it would appear
    // on the wrong side of the trigger object. 
    
    // Full disclosure: I used a public script on the Unity wiki
    // called DontGoThroughThings for reference. I did, however,
    // build a custom version using conventions learned in class
    // and making specific changes for my needs, like using
    // LateUpdate. You can see the original script here:
    // http://wiki.unity3d.com/index.php?title=DontGoThroughThings

    // Do note this doesn't send OnTriggerExit events. That wasn't
    // needed for my project, and it would've complicated this,
    // so I left it out. 

    private float sqrMinimumSpeed;  // The minimum speed we need to be going in order to activate this trigger detection, squared (because sqrt is expensive)
    private Vector3 prevPos;        // The position we were at last frame, used for our current speed as well as to raycast to
    private LayerMask layerMask;    // Layers to check for triggers
    private Rigidbody rb;           // The rigidbody of the fast object
    private Collider coll;          // The collider of the fast object

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    void Start()
    {
        sqrMinimumSpeed = Mathf.Min(Mathf.Min(coll.bounds.extents.x, coll.bounds.extents.y), coll.bounds.extents.z);
        prevPos = rb.position;
        layerMask = (1 << LayerMask.NameToLayer("Environment")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Door"));
	}
	
	void LateUpdate () {
        // Calculate how much we moved last frame
        Vector3 deltaMovement = rb.position - prevPos;
        // Check if we moved fast enough to justify using raycast to activate triggers
        if (deltaMovement.sqrMagnitude > sqrMinimumSpeed)
        {
            RaycastHit hit;

            // Perform raycast from our current position to the position we were at last frame
            if (Physics.Raycast(prevPos, deltaMovement, out hit, deltaMovement.magnitude, layerMask.value))
            {
                // Make sure we hit something with a collider
                if (!hit.collider)
                    return;

                // Active the OnTriggerEnter function on our object
                SendMessage("OnTriggerEnter", hit.collider);

                // Active OnTriggerEnter on the hit object, as well
                //if (hit.collider.isTrigger)
                    //hit.collider.SendMessage("OnTriggerEnter", coll);
            }
        }
	}
}
