using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject claw;
    public GameObject origin;
    public bool isShooting;
    public Animator minerAnimator;
    public Claw clawScript;
	
	void Update () {
        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            LaunchClaw();
        }
	}

    void LaunchClaw()
    {
        isShooting = true;
        minerAnimator.speed = 0;
        RaycastHit hit;
        Vector3 down = origin.transform.TransformDirection(Vector3.up);

        if (Physics.Raycast(origin.transform.position, down, out hit, 1000))
        {
            claw.SetActive(true);
            clawScript.ClawTarget(hit.point);
        }
    }

    public void CollectedObject()
    {
        isShooting = false;
        minerAnimator.speed = 1f;
    }
}
