using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class PaintGun : MonoBehaviour {

    // There's a lot of particle systems going on in the paint gun system, and they each have several parameters that need to be set to specific values
    // If I had more time, I'd just set them programmatically, and add notes to the component that it overrides some particleSystem values

    // This is probably the worst case of me putting multiple things in one class. This handles not only shooting paint, but also handling the player
    // crouching in paint and controlling the camera's position and tint while doing so. It's fixable, but due to time constraints, unviably so. 

    // Also, I wanted to do stuff like reduce player speed while crouching, but the built-in FPS controller makes it very hard to effect it from other classes-
    // The rigidbody needs to be set to kinematic, so you can't use forces, and all the fields and methods in the controller itself are set to private!

    public int maxPaint = 100;                              // How much paint we can store at once

    public GameObject paintGauge;                           // Object who's scale gets set to indicate how much paint we have remaining
    public Gradient particleColorGradient;                  // Gradient to choose paint gun's color from (so each game has a different color)
    // Note: the decalPool has its own particle system on it, holding all the paint particles in the world that the player can interact with (sink into and use to climb walls)
    // So whatever gameobject the decalpool is on, needs a Paint component as well
    public DecalPool decalPool;                             // Pool for storing all our paint decals. The paint that appears on collisions with the environment
    public ParticleSystem splatterParticles;                // Particle system for paint splatters. The particles that spawn on collisions with the environment - not interactable
    public GameObject paintPrefab;                          // Paint prefab to spawn (with the trigger)
    public MeshRenderer tint;                               // Mesh with transparent material, used to tint the screen

    private ParticleSystem ps;                              // Particle system for the "paint bullet" particles. The particles that come out of the gun
    private AudioSource shooting;                           // Audio to play while shooting
    private GameObject player;                      // Player object, so we can make it invisible to enemies when crouching in paint
    private Transform playerTransform;                      // Player's transform, used to handle climbing
    private CharacterController charController;             // Character controller, used to handle crouching

    private int paint;                                      // How much paint the player has remaining
    private float maxGauge;                                 // The size of the paint gauge at max paint
    private float targetGauge;                              // The current size of the paint gauge to animate towards
    private List<ParticleCollisionEvent> collisionEvents;   // List for passing to the GetCollisionEvents function to obtain particle collision events
    private bool shootingPaint = false;                     // Whether or not we're shooting paint. Used to separate paint emission frequency from the frame rate
    private bool isCrouching;                               // Whether or not we're crouching. We can't shoot paint while crouching
    private Color color;                                    // Color of the paint we shoot. Chosen at random at start, from our public color gradient
    private bool inPaint;                                   // Whether or not we're currently in paint
    private bool canClimb;                                  // Whether or not we're currently in vertical paint, which lets us climb up it
    private Color targetTint;                               // What color we're currently tinting the screen - our paint color, when crouching in paint

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        shooting = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        charController = player.GetComponent<CharacterController>();
    }

	void Start () {
        paint = maxPaint;
        targetGauge = maxGauge = paintGauge.transform.localScale.y;
        collisionEvents = new List<ParticleCollisionEvent>();
        color = particleColorGradient.Evaluate(Random.Range(0f, 1f));
	}

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(ps, other, collisionEvents);

        // When our gun particles collide with stuff, go through all the collision events
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            // Sometimes colliderComponent is null, idk why but in that event, we don't want to make paint
            if (collisionEvents[i].colliderComponent == null)
                continue;

            // If we hit a door, just buzz it without creating paint
            if (collisionEvents[i].colliderComponent.CompareTag("Door"))
            {
                collisionEvents[i].colliderComponent.GetComponent<Door>().Buzz();
                continue;
            }         
     
            // If we hit a WallPickup, dislodge it (without creating paint)
            WallPickup pickup = collisionEvents[i].colliderComponent.GetComponent<WallPickup>();
            if (pickup != null)
            {
                pickup.Dislodge(collisionEvents[i].normal);
                continue;
            }

            // If we didn't hit the environment, we don't want to make any paint
            if (collisionEvents[i].colliderComponent.gameObject.layer != LayerMask.NameToLayer("Environment"))
                continue;
  
            // Create paint where we hit
            decalPool.ParticleHit(collisionEvents[i].intersection, collisionEvents[i].normal, color);
            Object.Instantiate(paintPrefab, collisionEvents[i].intersection, Quaternion.Euler(collisionEvents[i].normal));

            // Create some splatter particles, just to look cool I guess
            // (tbh I was having trouble getting this particlesystem to look right, but figured I'd leave
            // the code in since that part was fine and that's what this whole project is about)
            EmitAtLocation(collisionEvents[i]);
        }
    }

    // Creates splatter particles
    void EmitAtLocation(ParticleCollisionEvent collisionEvent)
    {
        // Add splatter particles at the location of our collision
        splatterParticles.transform.position = collisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation(collisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = color;
        splatterParticles.Emit(1);
    }
	
	void Update () {
        // Handle Input paint
        shootingPaint = Input.GetButton("Fire2");

        // Update Gauge
        float scaleY = Mathf.MoveTowards(paintGauge.transform.localScale.y, targetGauge, Time.deltaTime);
        paintGauge.transform.localScale = new Vector3(paintGauge.transform.localScale.x, scaleY, paintGauge.transform.localScale.z);

        // Handle crouching
        UpdateCrouching();
	}

    // Since paint splatter is used as a gameplay mechanic, I don't want its emission rate tied to FPS
    void FixedUpdate()
    {
        if (!isCrouching && shootingPaint && paint > 0)
        {
            ParticleSystem.MainModule psMain = ps.main;
            psMain.startColor = color;
            ps.Emit(1);
            paint--;
            targetGauge = maxGauge * paint / maxPaint;
            if (!shooting.isPlaying) shooting.Play();
        }
        else shooting.Pause();
    }

    // Handle if we're crouching and all the effects that happen if we are
    void UpdateCrouching()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        player.layer = LayerMask.NameToLayer("Player");
        if (isCrouching)
        {
            // Crouch
            charController.center = new Vector3(0, charController.height / 2f, 0);
            targetTint = inPaint ? color : Color.black;
            if (inPaint)
            {
                // Restore paint
                paint += 2;
                if (paint > maxPaint) paint = maxPaint;
                targetGauge = maxGauge * paint / maxPaint;
                // Hide Player
                player.layer = LayerMask.NameToLayer("Default");
            }
            if (canClimb)
            {
                // Climb paint
                playerTransform.Translate(Vector3.up * Time.deltaTime * 50);
                if (playerTransform.position.y >= 100) playerTransform.position = new Vector3(playerTransform.position.x, 100, playerTransform.position.z);
            }
        }
        else
        {
            charController.center = new Vector3();
            targetTint = Color.black;
        }
        // Since this is lerping, it'll just be constantly approaching the targetTint
        // There isn't a Color.MoveTowards function, though, so this'll have to do
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetTint, Time.deltaTime * 4);
    }

    // Called to update whether or not we're currently in paint
    public void SetInPaint(bool inPaint, bool canClimb)
    {
        this.inPaint = inPaint;
        this.canClimb = canClimb;
    }
}
