using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShooter : MonoBehaviour {

	public Transform spawnPoint;
	public GameObject bulletPrefab;
	public GameObject impactPrefab;
	public int gunDamage = 10;

	private float shootDistance = 100;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Shoot ();
		}
	}

	void Shoot() {
		RaycastHit hit;
		GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
		bullet.GetComponent<Rigidbody> ().AddForce (spawnPoint.forward * 10000);
		if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, shootDistance)) {
			Debug.Log ("You shot at the " + hit.transform.name);
			GameObject impact = Instantiate(impactPrefab, hit.point + hit.normal * .01f, Quaternion.Euler(hit.normal));
			Health healthScript = hit.transform.GetComponent<Health> ();
			if (healthScript != null) {
				healthScript.TakeDamage (gunDamage);
			}
		}			
	}
}
