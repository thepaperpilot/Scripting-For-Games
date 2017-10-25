using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePickup : MonoBehaviour {

    public float pickupDuration = 10;         // How long the pickup lasts
    public GameObject player;                 // Reference to player, so we can change its speed
    public Renderer renderer;                 // Reference to player renderer, so we can change its material color
    public Rigidbody rb;                      // Reference to player's rigidbody, so we can change its mass

    private float pickupTime = 0;             // Current time pickup has been active
    private float pickupAnimTime = 0;         // How far pickup animation has completed
    private float pickupAnimDuration = 0.5f;  // How long the pickup animation takes

    private Vector3 playerSize;               // Original player size
    private float playerMass;                 // Original player mass

    public void Pickup()
    {
        if (pickupTime <= 0)
        {
            playerSize = player.transform.localScale;
            playerMass = rb.mass;
        }
        pickupTime = pickupDuration;
    }

    void Update()
    {
        // If we're active and animating
        if (pickupTime > pickupAnimDuration && pickupAnimTime < pickupAnimDuration)
        {
            // Find out how much time has passed
            float delta = Mathf.Min(pickupAnimDuration - pickupAnimTime, Time.deltaTime);

            // Apply the pickup effects
            player.transform.localScale = playerSize * (1 - pickupAnimTime / pickupAnimDuration / 2);
            rb.mass = playerMass * (.5f + .5f * (1 - pickupAnimTime / pickupAnimDuration));
            if (pickupAnimTime == 0)
                renderer.material.color = Color.cyan;
            
            // Add how long it took, to the total time taken
            pickupAnimTime += delta;
        }
        // If we're inactive and animating
        else if (pickupTime <= pickupAnimDuration && pickupAnimTime > 0)
        {
            // Find out how much time has passed
            float delta = Mathf.Min(pickupAnimTime, Time.deltaTime);

            // Apply the pickup effects
            player.transform.localScale = playerSize * (1 - pickupAnimTime / pickupAnimDuration / 2);
            rb.mass = playerMass * (.2f + .8f * pickupAnimTime / pickupAnimDuration);
            if (pickupAnimTime - delta == 0)
                renderer.material.color = Color.red;
            
            // Add how long it took, to the total time taken
            // (for animating to inactivity, we go in reverse)
            pickupAnimTime -= delta;
        }

        pickupTime -= Time.deltaTime;

        // Make the particles flash red increasingly fast as the animation goes on
        if (pickupTime > pickupDuration / 2)
        {
            Flash(1, .1f);
        }
        else if (pickupTime > pickupDuration / 4)
        {
            Flash(.5f, .1f);
        }
        else if (pickupTime > pickupDuration / 8)
        {
            Flash(.25f, .1f);
        }
        else if (pickupTime > 0)
        {
            Flash(.1f, .05f);
        }
    }

    void Flash(float frequency, float duration)
    {
        renderer.material.color = pickupTime % frequency > 0 && pickupTime % frequency < duration ? Color.red : Color.cyan;
    }
}
