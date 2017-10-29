using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyStep : MonoBehaviour {

    // This plays a sound each time the enemy finishes its animation

    private AudioSource pound;                  // Sound to play when he hits the ground

    void Awake()
    {
        pound = GetComponent<AudioSource>();
    }

    // Called at the end of each animation loop
    public void Step()
    {
        transform.parent.Translate(new Vector3(0, 0, transform.parent.lossyScale.z));
        pound.Play();
    }
}
