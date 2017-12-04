using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit : Entity {

    public int maxHealth = 100;             // How much health this object can carry
    public int speed = 100;                 // How long between turns
    public int strength = 10;               // How much base physical damage they do
    public int actionPoints = 4;            // Actions per turn
    public InvAction[] actions;             // Our equipped actions

    [NonSerialized]
    public ActionManager.Direction facing = ActionManager.Direction.UP;   // Current direction we're facing

    private int health;                     // How much health this object currently has
    private Health healthComponent;         // Instance of Health, which contains the health bar stuff, on the gameobject

    // An inventory-version of action
    // Originally a struct, like a lot of things I've created,
    // but changed to a class because it needs to be nullable and 
    // cooldown gets set a lot. I keep on thinking "Hey, I want
    // a container of data, let's make a struct!" because I equate
    // structs to serializable javascript objects in my head
    [Serializable]
    public class InvAction
    {
        public string name;             // The name of the action this refers to
        public int tier;                // The tier of the action this refers to

        [NonSerialized]
        public int cooldown;            // How long until we can use this action again
    }

    public void Setup()
    {
        health = maxHealth;
        healthComponent = gameObject.GetComponent<Health>();
        healthComponent.Setup();
    }

    // Damages the unit
    public void Damage(int amount)
    {
        // Take damage
        health -= amount;

        // Update UI
		healthComponent.SetTargetScale((float) health / maxHealth);

        // Handle death
        if (health <= 0)
        {
            Level level = LevelManager.instance.level;
            level.map[this.row, this.col].occupied = null;
            level.units.Remove(this);
            LevelManager.instance.CheckGameOver();
            MonoBehaviour.Destroy(gameObject);
        }
    }

    // Heals the unit
    public void AddHealth(int amount)
    {
        // Add Health
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        // Update UI
		healthComponent.SetTargetScale((float) health / maxHealth);
    }
}
