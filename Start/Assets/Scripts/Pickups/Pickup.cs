using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Pickup : MonoBehaviour
{

    public ParticleSystem ps;           // Particle System surrounding our pickup

    private AudioSource pickupSound;    // Sound we play when we're picked up

    void Awake()
    {
        pickupSound = GetComponent<AudioSource>();
    }

    // When we're picked up, play a sound destroy ourselves
    void PickupAndDestroy(Entity entity)
    {
        DoPickup(entity);
        pickupSound.Play();
        // Destroy only after the audio and particles are finished
        transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        if (ps != null)
        {
            ps.Stop();
            Destroy(gameObject, Mathf.Max(pickupSound.clip.length, ps.main.startLifetime.constant));
        }
        else
        {
            Destroy(gameObject, pickupSound.clip.length);
        }
    }

    // Do whatever it is we do when we're picked up
    protected abstract void DoPickup(Entity entity);
}
