using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;          // Speed the player moves at
    public GameObject camera;    // The camera, used so force can get added in the direction we're looking
    public LevelManager lm;      // Level Manager, for ending the level when we hit the win screen
    public ParticleSystem death; // Particle system to play after dying

    private Rigidbody rb;        // Used to add force to the player

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        // Read inputs from player
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Find the angle of the camera looking
        Vector3 cameraOffset = (camera.transform.position - transform.position);
        cameraOffset = new Vector3(cameraOffset.x, 0, cameraOffset.z).normalized;
        float angle = Mathf.Atan2(cameraOffset.z, cameraOffset.x);
        // Find the angle of the player's input, and add it to the angle of the camera looking
        angle += Mathf.Atan2(moveHorizontal, - moveVertical);
        // Calculate vector with magnitude of player's input, in direction of angle (with y rotation being 0)
        float magnitude = new Vector3(moveHorizontal, 0.0f, moveVertical).magnitude;
        Vector3 movement = new Vector3(Mathf.Cos(angle) * magnitude, 0.0f, Mathf.Sin(angle) * magnitude);

        // Apply force in the direction we want
        rb.AddForce(movement * speed);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            // Delete the pickup (using its parent, since we're just hitting the box part)
            other.transform.parent.gameObject.SetActive(false);
            // Apply pickup effect
            switch (other.transform.parent.name)
            {
                case "Speed": // Speed powerup
                    GetComponent<SpeedPickup>().Pickup();
                    break;
                case "Size": // Size powerup
                    GetComponent<SizePickup>().Pickup();
                    break;
            }
        }
        // Also detect if we hit the finish pad, in which case active the "you win" sequence
        else if (other.gameObject.CompareTag("finish"))
        {
            lm.FinishLevel();
        }
        // Also detect if we hit a spike, in which case active the "you died" sequence
        else if (other.gameObject.CompareTag("spike"))
        {
            lm.Die();
            Destroy(Instantiate(death, transform.position, Quaternion.identity), 0.6f);
            Destroy(gameObject);
            camera.GetComponent<FollowCamera>().ChangeDistance(5);
        }
    }

    // Used so player speed can be animated via the speed powerup
    public void ChangeSpeed(float val)
    {
        speed = val;
    }
}
