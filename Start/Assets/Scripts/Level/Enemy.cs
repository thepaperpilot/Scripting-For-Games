using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy : Unit {

	// Used for finding nearest stuff
	// Has to be a class because of the recursive bit
	class Node {
		public Node parent;
		public Tile tile;
	}

	public ActionManager.Direction direction = ActionManager.Direction.NULL;	// Direction we currently want to move
    public InvAction action = null;										            // Action we currently want to perform
    private Coroutine coroutine;                                                    // Our AI coroutine, if we're currently thinking

	// Starts running calculations on what actions to perform this beat
	public void Think(MonoBehaviour controller, Level level) {
		if (coroutine != null) controller.StopCoroutine (coroutine);
        coroutine = controller.StartCoroutine(CalculateMove(level));
	}

	// Returns the direction the enemy wants to move this beat
	IEnumerator CalculateMove(Level level)
    {
		// Calculate where we want to move to
		Node source = new Node ();
        source.tile = level.map[row, col];
		Node target = FindNearestCharacter(level, source);

        // If there are no valid targets, stop
        if (target == null)
            yield break;

		// If our target is next to us, attack it
		if (target.parent == source)
            action = actions[0];
        else
			action = null;
		
		// Find the first move to make to reach our target
		while (target.parent != source)
			target = target.parent;

		// Find direction to go to get to our target
		if (target.tile.row == row) {
            direction = target.tile.col < col ? ActionManager.Direction.DOWN : ActionManager.Direction.UP;
		} else {
            direction = target.tile.row < row ? ActionManager.Direction.LEFT : ActionManager.Direction.RIGHT;
		}

		// Finished calculations. Hopefully before the end of the beat, or else we're not doing anything
		yield break;
    }

	// BFS Implementation (my own work, based off a psuedocode BFS implementation)
	// I just don't want you thinking I'm copying code from somewhere else
	// If I did, I'd put it in a "lib" folder or something. Anything in the "Scripts"
	// folder is my own work
	Node FindNearestCharacter(Level level, Node source) {
		Queue<Node> open = new Queue<Node> ();
		HashSet<Tile> visited = new HashSet<Tile> ();

		open.Enqueue (source); 
		while (open.Count > 0) {
			Node parent = open.Dequeue ();
            Tile tile = parent.tile;

			// Check if this tile is a charater
			if (IsValidTarget(tile)) {
				return parent;
			}

			// Add adjacent nodes to our queue
			// For each direction make sure it's not out of bounds, its a passable tile, and we haven't visited it already
            Tile[] tiles = {
            /* up    */ parent.tile.row != 0 ? level.map[parent.tile.row - 1, parent.tile.col] : null,
            /* down  */ parent.tile.row != level.width - 1 ? level.map[parent.tile.row + 1, parent.tile.col] : null,
            /* left  */ parent.tile.col != 0 ? level.map[parent.tile.row, parent.tile.col - 1] : null,
            /* right */ parent.tile.col != level.height - 1 ? level.map[parent.tile.row, parent.tile.col + 1] : null
            };
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null && !visited.Contains(tiles[i]) && (tiles[i].IsPassable() || IsValidTarget(tiles[i])))
                    AddNode(open, parent, tiles[i]);
            }

			// Mark this node as visited
			visited.Add(tile);
		}

		return null;
	}

    // Utility function to determine node is a valid target
    bool IsValidTarget(Tile tile)
    {
        return tile.occupied != null && tile.occupied as Character != null;
    }

	// Utility function to enqueue a child node
	void AddNode(Queue<Node> queue, Node parent, Tile tile) {
		Node child = new Node ();
		child.parent = parent;
		child.tile = tile;
		queue.Enqueue (child);
	}
}
