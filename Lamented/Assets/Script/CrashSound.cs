using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSound : MonoBehaviour {

    public AudioClip crashSoft;
    public AudioClip crashHard;

    private AudioSource source;
    private float lowPitch = 1.25f;
    private float highPitch = .75f;
    private float velToVol = .1f; // Velocity to Volume
    private float velocityClipSplit = 10;

    void Awake () {
        source = GetComponent<AudioSource>();
	}

    void OnCollisionEnter(Collision coll)
    {
        source.pitch = Random.Range(lowPitch, highPitch);
        float hitVol = coll.relativeVelocity.magnitude * velToVol;
        if (coll.relativeVelocity.magnitude > velocityClipSplit)
            source.PlayOneShot(crashHard, hitVol);
        else
            source.PlayOneShot(crashSoft, hitVol);
    }
}
