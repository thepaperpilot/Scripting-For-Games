using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy {

    public int damage = 25;   // How much damage this enemy does on contact with the player

    private float initialScale; // Store the initialScale, to help calculate what our target scale should be

    void Start()
    {
        base.Start();
        initialScale = transform.parent.localScale.y;
        sinkSpeed /= 2;
    }

    // Instead of sinking, we shrink
    protected override void Sink()
    {
        // Does a bunch of math to position and scale the object correctly so it animates to a target scale
        // dependent on how much health it has left, so that it approaches half size at 0 health, at which
        // point it quickly scales into nothingness and then dies
        float targetScale = health <= 0 ? 0 : initialScale * (health / (float)initialHealth / 2 + .5f);
        float newScale = Mathf.MoveTowards(transform.parent.localScale.x, targetScale, Time.deltaTime * sinkSpeed);
        transform.parent.localScale = new Vector3(newScale, newScale, newScale);
        // the 2.5 comes from the x5 scaling of the "enemies" layer, and the whole thing divided by 2 because origin is the center of the cube
        transform.parent.position = new Vector3(transform.parent.position.x, newScale * 2.5f, transform.parent.position.z);

        // This enemy should actually just disappear if it gets below 0 health
        if (health <= 0)
        {
            Destroy(transform.parent.gameObject, 1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Damage(damage);
        }
    }
}
