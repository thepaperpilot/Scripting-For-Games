using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup {

    public int amount = 20; // How much health to give the player

    // Add health when picked up
    public override void DoPickup(Collider player)
    {
        player.GetComponent<Player>().AddHealth(amount);
    }
}
