using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		Debug.Log (coll.gameObject);
		GameObject gameObject = coll.gameObject;
		gameObject.GetComponent<SphereCollider> ().enabled = false;
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		ParticleSystem ps = gameObject.GetComponent<ParticleSystem> ();
		ParticleSystem.EmissionModule emission = ps.emission;
		emission.enabled = false;
		// Note I used lateupdate in the dontgothroughthings script, so it happens at the right moment
		// in the rendering order. However, the particle system will spawn ~1 particle on the wrong side 
		// of whatever is triggering it, so we'll delete it
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
		int num = ps.GetParticles(particles);
		particles [num - 1].remainingLifetime = 0;
		ps.SetParticles (particles, num);
		Destroy (gameObject, 1);
	}
}
