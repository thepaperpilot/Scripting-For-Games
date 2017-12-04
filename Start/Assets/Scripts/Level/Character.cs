using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : Unit {

    public float gold;                  // How much gold this character has

    [NonSerialized]
    public CharacterDisplay display;    // The UI controller that shows off all of this character's information

    public void AddGold(int amount)
    {
        // Add gold
        gold += amount;

        // Update UI
        display.UpdateGold();
    }
}
