using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePoint : MonoBehaviour {

    private Light light;
    private ParticleSystem particles;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        light.enabled = particles.enableEmission = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
	}
}
