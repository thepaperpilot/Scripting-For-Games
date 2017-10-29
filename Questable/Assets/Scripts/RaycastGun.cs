using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(DecalPool))]
public class RaycastGun : MonoBehaviour
{
    public int gunDamage = 10;          // How much damage this gun does
    public float reloadSpeed = .2f;     // How long between shots
    public Gradient bulletGradient;     // What color our bullet decals should be

    public Transform gunPoint;          // Position of where our gun fires

    private DecalPool bulletDecals;      // Pool used for creating bullet hole decals
    private LayerMask layerMask;        // What layers we can hit
    private LineRenderer lineRenderer;  // Used to make a line when we fire our gun
    private Light light;                // Muzzle flash for our gun
    private Player player;              // Reference to the player's attributes, for ammo count
    private AudioSource laser;          // Sound we play when we shoot

    private float shootDistance = 250;  // How far our bullets travel
    private float timer = 0;            // How long after we last shot
    private float effectDuration = .02f;// How long our gun effects last

    void Awake()
    {
        bulletDecals = GetComponent<DecalPool>();
        lineRenderer = GetComponent<LineRenderer>();
        light = GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        laser = GetComponents<AudioSource>()[0];
    }

    void Start()
    {
        layerMask = (1 << LayerMask.NameToLayer("Environment")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Door"));
    }

    void Update()
    {
        // Handle Input
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        // Reset gun effects after effectDuration
        timer += Time.deltaTime;
        if (timer >= effectDuration)
        {
            lineRenderer.enabled = false;
            light.enabled = false;
        }
    }

    // Fires our gun
    void Shoot()
    {
        // Make sure we're ready to fire
        if (timer < reloadSpeed)
            return;

        // Make sure we have ammo to use
        if (!player.UseAmmo())
            return;

        // Setup gun effects
        timer = 0;
        light.enabled = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gunPoint.position);
        laser.Play();

        // Shoot
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, shootDistance, layerMask))
        {
            // If we hit an enemy, shoot it
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            // If we hit a door, make it flash and bzzz
            Door door = hit.transform.GetComponent<Door>();
            if (enemy != null)
            {
                enemy.Damage(gunDamage);
            }
            else if (door != null)
            {
                door.Buzz();
            }

            // Draw our bullet line to it
            lineRenderer.SetPosition(1, hit.point);

            // Add bullet decal (unless we hit an entity or a door)
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment") && !hit.collider.CompareTag("Door"))
                bulletDecals.ParticleHit(hit.point, hit.normal, bulletGradient.Evaluate(Random.Range(0f, 1f)));
        }
        else
        {
            lineRenderer.SetPosition(1, gunPoint.position + gunPoint.forward * shootDistance);
        }
    }
}
