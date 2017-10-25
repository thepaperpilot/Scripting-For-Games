using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    public AudioClip[] footsteps;
    public float footstepFrequency = 1 / 60f;

    private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
    private AudioSource source;
    private float deltaTime = 0;

    void Awake()
    {
        controller = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        source = GetComponent<AudioSource>();
    }

	void Update () {
        if (controller.Velocity.magnitude != 0 && controller.Grounded && footsteps.Length != 0)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > footstepFrequency)
            {
                deltaTime -= footstepFrequency;
                source.PlayOneShot(footsteps[Random.Range(0, footsteps.Length - 1)], 1f);
            }
        }
	}
}
