using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FastObjectTrigger))]
public class BulletImpact : MonoBehaviour {

    public int damage = 5;      // How much damage this bullet does to the player

    private ParticleSystem ps;  // The particle system that trails behind the bullets

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // We hit something!
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<Player>().Damage(damage);
            Die();
        }
        else if (coll.gameObject.CompareTag("Door"))
        {
            coll.gameObject.GetComponent<Door>().Buzz();
            Die();
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            // Destroy self
            Die();
        }
    }

    void Die()
    {
        // Prevent this object from rendering, colliding, or moving
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<FastObjectTrigger>().enabled = false;

        // Particles emitted in the last frame need to be deleted
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.enabled = false;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int num = ps.GetParticles(particles);
        for (int i = num - 1; i >= 0 && num - i <= emission.rateOverTime.constant * Time.deltaTime; i--)
            particles[i].remainingLifetime = 0;
        ps.SetParticles(particles, num);

        // Finally, destroy ourselves (after all particles finish)
        Destroy(gameObject, ps.main.startLifetime.constant);
    }
}
