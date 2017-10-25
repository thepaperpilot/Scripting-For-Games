using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class, when attached to an object, exposes a "transparent" value that can be dynamically set
// and will automatically animate to a set alpha level, depending on whether transparent is true or not
// We use it to make walls between the player and camera fade out
public class Ghostable : MonoBehaviour {

    public float animDuration = 1;     // How long it takes to become transparent or opague
    public float targetAlpha = .1f;    // How transparent to become

    private Material mat;              // Reference to this object's material, so we can change its alpha
    private float animTime = 0;        // Our current progress in the transparency animation
    private bool transparent = false;  // Whether or not we are transparent

    void Start () {
        mat = GetComponent<Renderer>().material;
    }
	
	void Update () {
        // If we're transparent and animating to our targer alpha level
		if (transparent && animTime < animDuration) {
            // Find out how much time has passed
            float delta = Mathf.Min(animDuration - animTime, Time.deltaTime);
            // Change the transparency the amount it needs to, given the amount of passed time
            mat.color -= new Color(0, 0, 0, (1 - targetAlpha) * delta / animDuration);
            // Add how long it took, to the total time taken
            animTime += delta;
        }
        // If we're not transparent and animating back to full opagueness
        else if (!transparent && animTime > 0)
        {
            // Find out how much time has passed
            float delta = Mathf.Min(animTime, Time.deltaTime);
            // Change the transparency the amount it needs to, given the amount of passed time
            mat.color += new Color(0, 0, 0, (1 - targetAlpha) * delta / animDuration);
            // Add how long it took, to the total time taken
            // (for animating back to non-transparent, we go in reverse)
            animTime -= delta;
        }
	}

    public bool IsComplete() {
        // Find out if we're currently animating
        return (animTime <= 0 && !transparent) || (animTime >= animDuration && transparent);
    }

    public void SetTransparent(bool val)
    {
        // Change whether or not we should be transparent
        transparent = val;
    }
}
