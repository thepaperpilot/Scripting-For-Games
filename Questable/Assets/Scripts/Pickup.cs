using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {

    public ParticleSystem ps;           // Particle System surrounding our pickup

    private AudioSource pickupSound;    // Sound we play when we're picked up

    void Awake()
    {
        pickupSound = GetComponent<AudioSource>();
    }

    // When we're picked up, play a sound destroy ourselves
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DoPickup(other);
            pickupSound.Play();
            // Destroy only after the audio and particles are finished
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            ps.Stop();
            Destroy(transform.parent.gameObject, Mathf.Max(pickupSound.clip.length, ps.main.startLifetime.constant));
        }
    }

    // Do whatever it is we do when we're picked up
    public abstract void DoPickup(Collider player);
}
