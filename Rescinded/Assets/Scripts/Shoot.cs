using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject bulletPrefab;

	private float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 1) {
			timer -= 1;
			GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.Euler(transform.forward));
			bullet.GetComponent<Rigidbody> ().AddForce (transform.forward * 10000);
		}
	}
}
