using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent requires rigidbody
// Technically rotator and collider should be required by the pickup, but since that class doesn't use them I
// decided to support them not having them. However, since I use them here, I'll make them required here
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Pickup))]
public class WallPickup : MonoBehaviour {

    public float dislodgeForce;         // How much force to apply to pickup when dislodged

    private Rotator rotator;            // Rotator component, to disable it when in the wall
    private BoxCollider collider;       // Collider component, to make it larger and not a trigger when in the wall
    private Pickup pickup;              // Pickup component, to disable it and its particle system when in the wall
    private Rigidbody parentRb;         // Parent's rigidbody, to add force when dislodged

    void Awake()
    {
        rotator = GetComponent<Rotator>();
        collider = GetComponent<BoxCollider>();
        pickup = GetComponent<Pickup>();
        parentRb = transform.parent.GetComponent<Rigidbody>();
    }

    void Start () {
        rotator.enabled = false;
        collider.isTrigger = false;
        collider.size *= 2;
        ParticleSystem.EmissionModule emission = pickup.ps.emission;
        emission.enabled = false;
        pickup.enabled = false;
        // Doing this in code because the intent is to be able to just add this to a pickup and it'll do the rest
        gameObject.layer = LayerMask.NameToLayer("Door");   // I noticed these behave similarly to doors, so I did this instead of creating a new layer
        transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
	}

    // Called when we get hit by the paintGun
    public void Dislodge(Vector3 normal)
    {
        rotator.enabled = true;
        collider.isTrigger = true;
        collider.size /= 2;
        pickup.enabled = true;
        ParticleSystem.EmissionModule emission = pickup.ps.emission;
        emission.enabled = true;
        parentRb.isKinematic = false;
        parentRb.AddForce(normal * dislodgeForce);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
