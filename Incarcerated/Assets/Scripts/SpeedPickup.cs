using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour {

    public float pickupDuration = 10;         // How long the pickup lasts
    public float pickupEffect = 1;            // How strong the pickup effect is
    public GameObject player;                 // Reference to player, so we can change its speed
    public FollowCamera camera;               // Reference to camera, so we can change its distance

    private float pickupTime = 0;             // Current time pickup has been active
    private float pickupAnimTime = 0;         // How far pickup animation has completed
    private float pickupAnimDuration = 0.5f;  // How long the pickup animation takes

	private TrailRenderer tr;
    private float playerSpeed;                // Original player speed
    private float cameraDistance;             // Original camera distance

    public void Pickup()
    {
		tr = player.GetComponent<TrailRenderer> ();
        pickupTime = pickupDuration;
        playerSpeed = player.GetComponent<Player>().speed;
        cameraDistance = camera.distance;
    }

    void Update()
    {
        // If we're active and animating
        if (pickupTime > pickupAnimDuration && pickupAnimTime < pickupAnimDuration)
        {
            // Find out how much time has passed
            float delta = Mathf.Min(pickupAnimDuration - pickupAnimTime, Time.deltaTime);

            // Apply the pickup effects
            player.GetComponent<Player>().ChangeSpeed(playerSpeed * (1 + pickupEffect * pickupAnimTime / pickupAnimDuration));
            camera.ChangeDistance(cameraDistance * (1 + pickupEffect * pickupAnimTime / pickupAnimDuration));
			if (pickupAnimTime == 0)
				tr.enabled = true;
            
            // Add how long it took, to the total time taken
            pickupAnimTime += delta;
        }
        // If we're inactive and animating
        else if (pickupTime <= pickupAnimDuration && pickupAnimTime > 0)
        {
            // Find out how much time has passed
            float delta = Mathf.Min(pickupAnimTime, Time.deltaTime);

            // Apply the pickup effects
            player.GetComponent<Player>().ChangeSpeed(playerSpeed * (1 + pickupEffect * pickupAnimTime / pickupAnimDuration));
            camera.ChangeDistance(cameraDistance * (1 + pickupEffect * pickupAnimTime / pickupAnimDuration));
			if (pickupAnimTime - delta == 0)
				tr.enabled = false;
            
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
		tr.startColor = pickupTime % frequency > 0 && pickupTime % frequency < duration ? Color.red : Color.cyan;
    }
}
