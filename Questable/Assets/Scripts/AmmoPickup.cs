using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup {

    public int amount = 5;  // How much ammo to give the player

    // Add ammo when picked up
    public override void DoPickup(Collider player)
    {
        player.GetComponent<Player>().AddAmmo(amount);
    }
}
