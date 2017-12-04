using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entity : Feature {

    public bool passable = false; // Whether or not other entities can pass into this one. Usually only true for pickups

	// Determines if the entity can move in a given direction, dependent on the current state of the board
    public bool CanMove(Level level, ActionManager.Direction dir)
    {
        if (dir == ActionManager.Direction.LEFT && (row == 0 || !level.map[row - 1, col].IsPassable()))
            return false;
        if (dir == ActionManager.Direction.RIGHT && (row == level.width - 1 || !level.map[row + 1, col].IsPassable()))
            return false;
        if (dir == ActionManager.Direction.DOWN && (col == 0 || !level.map[row, col - 1].IsPassable()))
            return false;
        if (dir == ActionManager.Direction.UP && (col == level.height - 1 || !level.map[row, col + 1].IsPassable()))
            return false;

        return true;
    }
    
    // Moves entity to a new location
    public void Move(Tile[,] map, int row, int col)
    {
        // Remove ourselves from the old location
        map[this.row, this.col].occupied = null;
        map[this.row, this.col].UpdateBounce(true);

        // Add ourselves to the new one
        map[row, col].UpdateBounce(false);
        map[row, col].SetOccupant(this);

        // Update our position
        this.row = row;
        this.col = col;
    }
}
