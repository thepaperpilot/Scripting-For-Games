using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile : Feature {

    public Entity occupied = null;	// The entity currently on this tile
    public bool passable = true;	// Whether or not entities can walk onto this tile

	// Determines if we're passable, based off our own value, and,
	// if we're occupied, our entities' passable value
    public bool IsPassable()
    {
        return passable && (occupied == null || occupied.passable);
    }

	// Moves an entity onto this tile
    public void SetOccupant(Entity entity)
    {
        if (occupied != null)
            occupied.gameObject.SendMessage("PickupAndDestroy", entity, SendMessageOptions.DontRequireReceiver);
        occupied = entity;
        Vector3 localPos = entity.gameObject.transform.localPosition;
        entity.gameObject.transform.parent = gameObject.transform;
        entity.gameObject.transform.localPosition = localPos;
    }

    // Finds the tile relative to another tile
    public static Tile GetRelativeTile(Level level, Tile tile, ActionManager.Direction dir, int distance) {
        int row = tile.row;
        int col = tile.col;
        switch (dir)
        {
			// For whatever reason, UP and DOWN change our column, and LEFT and RIGHT our row
            case ActionManager.Direction.UP:
				col += distance;
                break;
            case ActionManager.Direction.DOWN:
				col -= distance;
                break;
            case ActionManager.Direction.LEFT:
				row -= distance;
                break;
            case ActionManager.Direction.RIGHT:
                row += distance;
                break;
        }
        if (row < 0 || row >= level.height)
            return null;
        if (col < 0 || col >= level.width)
            return null;
        return level.map[row, col];
    }

    // Makes us bounce or not, affecting all children
    public void UpdateBounce(bool bounce)
    {
        foreach (Animator anim in gameObject.GetComponentsInChildren<Animator>())
            anim.enabled = bounce;
    }
}
