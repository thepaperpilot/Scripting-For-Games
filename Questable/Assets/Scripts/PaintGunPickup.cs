using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGunPickup : Pickup {

    // Add paint gun
    public override void DoPickup(Collider player)
    {
        // Also hide the meshes in our children (the ammo gauge)
        MeshRenderer[] components = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].enabled = false;
        }
        player.GetComponent<Player>().EnablePaintGun();
    }
}
