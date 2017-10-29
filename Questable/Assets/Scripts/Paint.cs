using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Paint : MonoBehaviour {

    // I tried for SO LONG to do this using triggers/colliders on the paint decals,
    // and only after many hours of trying to figure out why it was not working,
    // I discovered that you need the particle system to be playing in order for
    // collision and trigger events to fire. Since I couldn't have the particle system
    // playing, or else it wouldn't work for decals, I instead had to do this.
    // Ugh.

    private static int numPaint = 0;    // How many paint particles we're in
    private static int numLadders = 0;  // How many vertical paint particles we're in

    private PaintGun paintGun;          // Reference to our paintGun, so we can tell it if we're in paint or not

    private bool isLadder;              // Whether or not this paint particle is vertical or not

    void Awake()
    {
        paintGun = GameObject.FindObjectOfType<PaintGun>().GetComponent<PaintGun>();
    }

    void Start()
    {
        // Rotation gets set to vector of the normal of our collision, which will never affect the appearance of our circular particle, but will tell
        // us if we're on a horizontal or vertical surface. If we're on the latter, be a ladder
        isLadder = transform.rotation.eulerAngles.y == 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numPaint++;
            if (isLadder) numLadders++;
            paintGun.SetInPaint(numPaint > 0, numLadders > 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numPaint--;
            if (isLadder) numLadders--;
            paintGun.SetInPaint(numPaint > 0, numLadders > 0);
        }
    }
}
