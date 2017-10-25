using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    int playerLives = 3; // Player's current number of lives
    int maxHealth = 100; // Player's maximum health
    int damage = 5; // Damage dealt to player at start
    int numEnemyAttacks = 3; // Number of times the enemy attack the player

    float health = 100; // Player's current health
    float baseDamage = 100f; // Base damage player deals
    float damageMultiplier = 1.75f; // Bonus damage multiplier
    float enemyDamage = 2f; // Base damage dealt by current enemy

    bool isAlive = true; // Whether or not the player is currently alive
    bool isInvincible = false; // Whether or not the player is currently invincible

    // These will be public because restrings are fun!
    // (public variables are accessible from the editor, and do not need default values in here)
    public string playerName = "Garfield the Deals Warlock"; // The player's name
    public string monsterName = "a gerblin"; // The name of the monster the player is currently fighting

    public string[] spellBook = new string[] { "Fireball", "Heal" }; // Array of spells known by the player
    public int currentSpell = 0; // Index of the spell currently being cast

    public Light myLight;
    public MeshRenderer sphere;

    public Color great = Color.cyan;
    public Color okay = Color.green;
    public Color bad = Color.yellow;
    public Color imminent = Color.red;
    public Color dead = Color.black;

    // For the console
    public Text logTextArea;
    Queue<string> scrollback = new Queue<string>();

    void Start () {
        Log("Cold open on our player, " + playerName + ", just appearing in the world...");
        Log("initial player lives: " + playerLives + ", initial player health: " + health);
        Log("");

        /*
        Log("Oh no! The player is attacking himself...");
        float totalDamageDealt = Hit(damage);
        Log(totalDamageDealt + " total damage dealt!");
        Log("new player health: " + health);
        Log("");
         */

        Log("Oh no! A wild " + monsterName + " appears!");
        Log("What will " + playerName + " do!?...");
        Cast(playerName, spellBook[currentSpell], monsterName);
	}
	
	void Update () {
        // Use escape to exit the game
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Handle death
        if (health <= 0)
        {
            Die();
        }

        // Get attacked by enemy, if space was pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnemyAttacks();
        }

        // Show all spells in spell book when Q is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowSpells();
        }

        // Change spells using arrow keys
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSpell(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSpell(1);
        }

        // Toggle light using W
        if (Input.GetKeyDown(KeyCode.W))
        {
            myLight.enabled = !myLight.enabled;
        }

        // Increase and Decrease light range using E and R
        if (Input.GetKey(KeyCode.E))
        {
            myLight.range -= .2f;
        }
        if (Input.GetKey(KeyCode.R))
        {
            myLight.range += .2f;
        }
    }

    void Die()
    {
        playerLives--;
        /*
        health = maxHealth;
        Log(playerName + " has died!");
        // TODO audiovisual feedback

        if (playerLives <= 0)
        {
            isAlive = false;
            Log(playerName + "'s game is over!");
            // TODO game over screen or something
        }
        */
    }

    float Hit(float damage)
    {
        if (isInvincible)
        {
            Log("The attack fails to hit " + playerName + "!");
            return 0f;
        }
        health -= damage;
        CheckStatus();
        return damage;
        // TODO audiovisual feedback
    }

    float GetDamageDealt()
    {
        return baseDamage * damageMultiplier;
    }

    void Cast(string caster, string spellName, string target)
    {
        Log(caster + " casts " + spellName + " at " + target + "!");
        Log("");
    }

    void CheckStatus()
    {
        if (health == maxHealth)
        {
            Log(playerName + " looks great!");
            sphere.material.color = great;
        } else if (health > 50)
        {
            Log(playerName + " is okay.");
            sphere.material.color = okay;
        } else if (health > 20)
        {
            Log(playerName + " has looked better.");
            sphere.material.color = bad;
        } else if (health > 0)
        {
            Log("Death calls for " + playerName + ".");
            sphere.material.color = imminent;
        } else
        {
            Log(playerName + " is deceased.");
            sphere.material.color = dead;
        }
    }

    void EnemyAttacks()
    {
        Log(monsterName + " attacks " + playerName + " " + numEnemyAttacks + " times!");
        for (int i = 0; i < numEnemyAttacks; i++)
        {
            float damage = Hit(enemyDamage);
            Log("Attack " + (i + 1) + " hits " + playerName + " for " + damage + " damage.");
        }
        Log("");
    }

    void ShowSpells()
    {
        Log("Current Spells:");
        foreach(string spell in spellBook)
        {
            Log((spell == spellBook[currentSpell] ? "*" : " ") + spell);
        }
        Log("");
    }

    void ChangeSpell(int direction)
    {
        currentSpell += direction;
        if (currentSpell == -1) currentSpell = spellBook.Length - 1;
        else if (currentSpell == spellBook.Length) currentSpell = 0;
        Log("Changed active spell to " + spellBook[currentSpell]);
        Log("");
    }

    void Log(string line)
    {
        scrollback.Enqueue(line);
        logTextArea.text = string.Join("\n", scrollback.ToArray());
    }
}
