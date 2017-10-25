using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject target;            // The object the camera is following
    public float rotationSpeed = 5;      // The sensitivity of mouse aim
    public float distance = 1;           // How far away should the camera be from the target

    Vector3 offset;                      // Used to ensure camera offset is maintained, even when the target is moving
    List<Ghostable> transparent = new List<Ghostable>();           // List of objects we made transparent

    void Start()
    {
        // Determine initial offset. 
        // This will, most relevantly, determine the y offset (which won't change through mouse aim)
        offset = transform.position - target.transform.position;
    }
	
	void Update () {
        UpdateHidden();        
	}

    void LateUpdate()
    {
        UpdateFollow();
    }

    // Make the camera follow the player, and be aimed with the mouse
    void UpdateFollow()
    {
        // If the player exists, use that
        // Otherwise, just use our last position
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        position -= offset;
        if (target != null)
        {
            position = target.transform.position;
        }

        // The player may have moved, so ensure we're using the same offset as before
        transform.position = position + offset;

        // Since this is player input, we do NOT multiply speed by deltaTime
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;

        // change the angle of the camera around the target
        // this won't change the y offset of the camera
        transform.RotateAround(position, Vector3.up, horizontal);

        // look at the target
        transform.LookAt(position);

        // ensure we're the same distance away from the object
        offset = (transform.position - position).normalized * distance;
        transform.position = position + offset;
    }

    // Make any objects between the camera and the player be hidden (transparent)
    void UpdateHidden()
    {
        // Remove objects that finished de-ghosting
        foreach (Ghostable ghost in transparent.ToArray()) {
            ghost.SetTransparent(false); 
            if (ghost.IsComplete())
                transparent.Remove(ghost);
        }

        // Find objects between camera and player
        RaycastHit[] objects;
        objects = Physics.RaycastAll(transform.position, transform.forward, distance);
        foreach (RaycastHit hit in objects)
        {
            Ghostable ghost = hit.collider.GetComponent<Ghostable>();
            if (ghost == null)
                continue;
            // For each found object, make it transparent and add it to our list
            ghost.SetTransparent(true);
            if (!transparent.Contains(ghost))
                transparent.Add(ghost);
        }
    }

    // Used so camera distance can be animated via the speed powerup
    public void ChangeDistance(float val)
    {
        distance = val;
    }
}
