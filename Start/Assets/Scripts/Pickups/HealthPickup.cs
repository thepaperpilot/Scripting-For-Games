using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{

    public int amount = 20; // How much health to give the player

    // Add health when picked up
    protected override void DoPickup(Entity entity)
    {
        if (entity as Unit != null)
            (entity as Unit).AddHealth(amount);
    }
}
