using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class Door : MonoBehaviour {

    // When a door gets hit, it flashes and makes a bzzz sound

    public float effectDuration = .25f; // How long to flash white

    private AudioSource buzz;           // Sound to play when buzzed
    private MeshRenderer renderer;      // Reference to our model, so we can change its color

    private float timer = 0;            // Used for finding out how long we've been flashing white
    private Color startingColor;        // Used for returning to our original color after being buzzed
    private Color whiteColor;           // Transparent version of Color.white (also slightly redder)

    void Start()
    {
        whiteColor = Color.white;
        whiteColor.a /= 5;
        whiteColor.r += 1f;
    }

    void Awake()
    {
        buzz = GetComponent<AudioSource>();
        renderer = GetComponent<MeshRenderer>();
        startingColor = renderer.material.color;
    }
    	
	void Update () {
        Flash();
	}

    // Update our material color, based on whether we've been hit recently
    void Flash()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            renderer.material.color = whiteColor;
        }
        else
        {
            renderer.material.color = startingColor;
        }
    }

    // Called when hit by a projectile
    public void Buzz()
    {
        if (timer < 0)
            buzz.Play();
        timer = effectDuration;
    }
}
