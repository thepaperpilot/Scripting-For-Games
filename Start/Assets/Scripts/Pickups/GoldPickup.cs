using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GoldPickup : Pickup 
{

    public int amount = 20; // How much gold to give the player

    // Add gold when picked up
    protected override void DoPickup(Entity entity)
    {
        if (entity as Character != null)
            (entity as Character).AddGold(amount);
    }
}
