using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int initialHealth = 100;     // How much health the enemy starts with
    public float effectDuration = 0.1f; // How long to change body color when hit

    public MeshRenderer body;           // Used to change body color when hit

    protected float sinkSpeed = 5;        // How quickly the enemy sinks into the ground as it takes damage
    protected int health;                 // How much health the enemy currently has

    private Vector3 start;              // Where the enemy starts out
    private Vector3 end;                // Where the enemy sinks down to, at 0 health
    private Vector3 target;             // Where the enemy is currently sinking to
    private float timer = 0;            // Timer used for changing body color when hit
    private Color startingColor;        // Used for returning back to normal body color

    protected void Start()
    {
        health = initialHealth;
        start = target = transform.position;
        end = transform.position + Vector3.down * 6 * transform.localScale.y;
        startingColor = body.material.color;
    }

    void Update()
    {
        Sink();
        Flash();
    }

    // We need to access this in a child, so it's protected
    // I'm spoiled by javascript (which is "prototype-oriented")
    // where all functions are variables and can be removed,
    // added, replaced, etc. at will, without any keywords at all
    // (no protected, no public, no static, etc.)
    protected virtual void Sink()
    {
        // Move towards our current position (determined by current health)
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * sinkSpeed);
    }

    void Flash()
    {
        // Set our body color based on when we were last hit
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            body.material.color = Color.white;
        }
        else
        {
            body.material.color = startingColor;
        }
    }

    public void Damage(int amount)
    {
        // Take Damage
        health -= amount;

        // Update position (representing health)
        target = Vector3.Lerp(end, start, health / (float) initialHealth);

        // Make the body flash white
        timer = effectDuration;
        startingColor = body.material.color;
    }
}
