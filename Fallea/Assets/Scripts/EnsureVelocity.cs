using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsureVelocity : MonoBehaviour {

    public float constantVelocity = 6f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity = constantVelocity * rb.velocity.normalized;
	}
}
