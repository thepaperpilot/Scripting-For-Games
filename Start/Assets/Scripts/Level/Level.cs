using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    // Serialized values
    public int width;               // How wide the map is
    public int height;              // How tall the map is
    public float timeScale;         // Set timescale to match the audio
    public string beat;             // Name of the beat sound to play on this level
    public Tile[] features;         // List of non-empty map tiles
    public Tile[] startingPositions;// List of tiles (just using row and col) for where to start our characters
    public Entity[] entities;       // List of entities on the map (pickups)
    public Enemy[] enemies;         // List of enemies on the map

    // Other variables
    [NonSerialized]
    public Unit unit;               // The currently active unit
    [NonSerialized]
    public Tile[,] map;             // The map on screen, composed of a bunch of prefabs making up "tiles"
    [NonSerialized]
    public List<Unit> units;		// All the units on the board
	[NonSerialized]
	public int actionPoints;        // Actions remaining in this turn

    private LevelManager lm;		// Reference to our level manager
    private TurnManager tm;         // Reference to our turn manager

    public void Setup()
    {
        lm = LevelManager.instance;
        tm = GameObject.FindObjectOfType<TurnManager>();
        map = new Tile[width, height];

        // Add features to map
        for (int i = 0; i < features.Length; i++)
        {
            map[features[i].row, features[i].col] = features[i];
            features[i].CreateGameObject(lm, true);
        }

        // Fill rest of map with empty tiles
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == null)
                {
                    map[i, j] = new Tile()
                    {
                        row = i,
                        col = j,
                        type = "empty",
                        passable = true
                    };
                    map[i, j].CreateGameObject(lm, true);
                }
            }
        }

        // Position our characters
        for (int i = 0; i < lm.activeCharacters.Count && i < startingPositions.Length; i++)
        {
            lm.activeCharacters[i].row = startingPositions[i].row;
            lm.activeCharacters[i].col = startingPositions[i].col;
        }

        // Add our entities
        for (int i = 0; i < entities.Length; i++)
        {
            Entity entity = entities[i];
            entity.CreateGameObject(lm, false);
            MeshRenderer[] renderers = entity.gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].enabled = false;
            }
            ParticleSystem[] particles = entity.gameObject.GetComponentsInChildren<ParticleSystem>();
            for (int j = 0; j < particles.Length; j++)
            {
                particles[j].gameObject.SetActive(false);
            }
        }
        units = new List<Unit>(enemies.Length + lm.activeCharacters.Count);
        for (int i = 0; i < enemies.Length + lm.activeCharacters.Count; i++)
        {
            Unit entity = i < enemies.Length ? enemies[i] as Unit : lm.activeCharacters[i - enemies.Length] as Unit;
            entity.CreateGameObject(lm, false);
            MeshRenderer[] renderers = entity.gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].enabled = false;
            }
            entity.Setup();
            units.Add(entity);
        }
        tm.SetupDisplay(lm.activeCharacters);
        unit = tm.NextTurn(units, lm.activeCharacters);
        actionPoints = unit.actionPoints + 1;

        // Position level in center of scene
        lm.transform.position = new Vector3(width * lm.tileSize / -2f, 0, width * lm.tileSize / -2f);
    }

    // Called when the first beat ends, to help synchronize everything's animations
    public void FirstBeat()
    {
        for (int i = 0; i < entities.Length; i++)
        {
            MeshRenderer[] renderers = entities[i].gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].enabled = true;
            }
            ParticleSystem[] particles = entities[i].gameObject.GetComponentsInChildren<ParticleSystem>(true);
            for (int j = 0; j < particles.Length; j++)
            {
                particles[j].gameObject.SetActive(true);
            }
            entities[i].Move(map, entities[i].row, entities[i].col);
        }
        for (int i = 0; i < units.Count; i++)
        {
            MeshRenderer[] renderers = units[i].gameObject.GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].enabled = true;
            }
            units[i].Move(map, units[i].row, units[i].col);
        }
		GameObject.FindObjectOfType<ActionManager>().EndBeat();
    }

    // Called every beat, from the ActionController
    public Unit Beat(ActionManager.Direction dir, Unit.InvAction action)
    {
        // Move current character
        if (dir != ActionManager.Direction.NULL)
        {
            bool canMove = unit.CanMove(this, dir);
            int row = unit.row;
            int col = unit.col;
            switch (dir)
            {
                case ActionManager.Direction.UP:
                    if (canMove) col++;
                    unit.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case ActionManager.Direction.RIGHT:
                    if (canMove) row++;
                    unit.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case ActionManager.Direction.DOWN:
                    if (canMove) col--;
                    unit.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case ActionManager.Direction.LEFT:
                    if (canMove) row--;
                    unit.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
            unit.facing = dir;
            if (canMove && action == null) unit.Move(map, row, col);
        }

        // Turn Handling
        if (action != null)
        {
            Action act = ActionManager.actions[action.name];
            if (act.GetActionCost(action.tier) <= actionPoints && action.cooldown <= 0)
            {
                act.Act(this, unit, dir, action.tier);
                actionPoints -= act.GetActionCost(action.tier);
                action.cooldown = act.GetCooldown(action.tier);
                if (unit as Character != null)
                    (unit as Character).display.UpdateActions();
            }
            else actionPoints--;
        }
        else
        {
            actionPoints--;
        }
        if (actionPoints <= 0)
        {
            unit = tm.NextTurn(units, lm.activeCharacters);
            actionPoints = unit.actionPoints;
        }

        // Give the ActionController the new character, so they can add the controller to them
        return unit;
    }
}
