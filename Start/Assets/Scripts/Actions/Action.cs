using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Action {

    // Interfaces don't support defining variables for child classes to set, so I have to
    // create getters for each of those values. A bit verbose for my tastes. I prefer
    // javascript because its better at handling these types of situations without
    // creating lots of small files

    // Perform this action upon the world
    void Act(Level level, Unit unit, ActionManager.Direction dir, int tier);

    // Get number of actionPoints required to perform this action
    int GetActionCost(int tier);

    // Get string identifier for the icon to use for this action
    string GetSprite(int tier);

    // Get how many beats need to pass before 
    int GetCooldown(int tier);

    // Get how much gold is needed to upgrade this action to the next tier
    int GetUpgradeCost(int tier);

    // Get the text to display when hovering over the upgrade in the shop
    string GetTooltip(int tier);
}
