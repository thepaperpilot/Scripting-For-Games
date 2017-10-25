using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

    public GameObject projectile;
    public float throwSpeed = 2000;
    public float volumeLow = .5f;
    public float volumeHigh = 1;
    public AudioClip shootSound;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            source.PlayOneShot(shootSound, Random.RandomRange(volumeLow, volumeHigh));
            GameObject throwThis = Instantiate(projectile, transform.position, transform.rotation);
            Rigidbody rb = throwThis.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(0, 0, throwSpeed));
        }
	}
}
