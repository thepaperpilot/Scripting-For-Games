using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

	public CharacterStore[] characterStore;        		// Each of the shops (UI components) for our characters (in same order as characters)
	// This should be the same as the one on the TurnManager. I should've probably found a way to not have to duplicate all that
	[HideInInspector]
	public Dictionary<string, Sprite> actionDict;      	// Dictionary of action sprites

	[SerializeField]
	private SpriteDef[] actionSprites;  				// Used to create a dictionary of action sprites
    private Text text;                                  // The text component where we show the tooltip

	[Serializable]
	struct SpriteDef
	{
		public string name;
		public Sprite sprite;
	}

    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

	void Start () {
		actionDict = new Dictionary<string, Sprite>();
		for (int i = 0; i < actionSprites.Length; i++)
		{
			actionDict.Add(actionSprites[i].name, actionSprites[i].sprite);
		}

        // Grant extra characters for completing levels
        while (LevelManager.instance.activeCharacters.Count <= LevelManager.currLevel)
        	LevelManager.instance.activeCharacters.Add(LevelManager.instance.characters[LevelManager.instance.activeCharacters.Count]);

        // Save everything
        PlayerPrefs.SetString("characters", JsonUtility.ToJson(LevelManager.instance.activeCharacters));
        PlayerPrefs.SetInt("level", LevelManager.currLevel);

		// I didn't use prefabs for the unit displays, because each one is different (color)
		// and on the game screen their gold displays are in different locations
		// and these ones need characterStore, but the ones in the game scene
		// need characterDisplay
		// But that does mean it's harder to edit these, because you'd need to
		// mirror the changes across all 8 displays!
		for (int i = 0; i < LevelManager.instance.activeCharacters.Count; i++)
		{
			characterStore[i].Setup(this, LevelManager.instance.activeCharacters[i]);
		}

        // Clear our editor-only tooltip ;)
        SetTooltip("");
    }

    public void SetTooltip(string text)
    {
        this.text.text = text;   // Same
    }
}
