using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Feature {

	// Represents something on the game board

    public int row;								// The row of the board this feature is positioned on
	public int col;								// The column of the board this feature is positioned on
	public string type;							// The key of this feature's game object in our prefab dictionary

    public GameObject gameObject;				// The game object that goes on the actual board

    // Sets up our game object based on the position and type and stuff
    public void CreateGameObject(LevelManager lm, bool setLocal)
    {
        gameObject = MonoBehaviour.Instantiate(lm.tilePrefabs[type], lm.transform);
        if (setLocal)
            gameObject.transform.localPosition = new Vector3(row * lm.tileSize, 0, col * lm.tileSize);
    }
}
