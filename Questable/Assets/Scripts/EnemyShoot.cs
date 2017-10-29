using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyShoot : EnemyAim {

    public float shootInterval = 1; // How long between shots
    public float bulletSpeed = 10;  // How fast bullets travel

    public GameObject bulletPrefab; // Prefab for bullets
    public Transform spawnPos;      // Where to spawn the bullets

    private float shootTimer = 0;   // Timer for shooting

    private AudioSource laser;      // Laser sound to make while shooting

    void Awake()
    {
        laser = GetComponent<AudioSource>();
    }

    // The EnemyAim class I wrote will automatically look towards the player, and fire one of the following
    // three functions every frame, depending on our current status in regards to looking at the player
    protected override void LookingAtTarget()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            // Shoot Bullet
            GameObject bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.Euler(transform.forward));
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            laser.Play();
            shootTimer -= shootInterval;
        }
    }

    protected override void CanSeeTarget()
    {
        shootTimer = 0;
    }

    // If we can't see the player, rotate in a circle
    protected override void CannotSeeTarget()
    {
        shootTimer = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, Time.time * 100, 0), rotSpeed * Time.deltaTime);
    }
}
