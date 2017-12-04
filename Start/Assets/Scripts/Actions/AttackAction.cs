using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
	// Attacks an entity one tile away in a given direction
    public void Act(Level level, Unit unit, ActionManager.Direction dir, int tier)
    {
        Tile target = Tile.GetRelativeTile(level, level.map[unit.row, unit.col], dir, 1);
        if (target != null && target.occupied != null && target.occupied as Unit != null)
        {
            (target.occupied as Unit).Damage(tier * unit.strength);
            LevelManager.instance.PlayOneShot("marimba");
        }
    }

    public string GetTooltip(int tier)
    {
        string desc = "";
        switch (tier)
        {
            case 0:
                desc = "Unlocks the ability to perform a fast but weak melee attack";
                break;
            default:
                desc =  "Attacks become stronger but slower";
                break;
        }
        desc += "\nAction Cost: " + GetActionCost(tier) + "\t\tCooldown: " + GetCooldown(tier);
        return desc;
    }

    public int GetActionCost(int tier) { return tier + 1; }
    public string GetSprite(int tier) { return "sword"; }
    public int GetCooldown(int tier) { return tier - 1; }
    public int GetUpgradeCost(int tier) { return 4 + tier * 2; }
}
