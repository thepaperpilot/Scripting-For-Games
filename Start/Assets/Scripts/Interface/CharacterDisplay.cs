using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour {

    public GameObject[] actions;        // The UI elements for this character's actions, each with an image and text component (potentially nested)
    public Text currency;               // The UI element showing this character's current gold count
	[HideInInspector]
    public Character character;         // The character this display is displaying the status of

    private TurnManager tm;             // Reference to the TurnManager, because that's where I decided to put the action sprites dictionary

    void Awake()
    {
        tm = GameObject.FindObjectOfType<TurnManager>();
    }

    public void Setup(Character character)
    {
        // Make this gameObject active
        gameObject.SetActive(true);

        // Bind ourselves together
        this.character = character;
        character.display = this;

        // Set up our actions
        for (int i = 0; i < character.actions.Length && i < actions.Length; i++)
        {
            Action action = ActionManager.actions[character.actions[i].name];
            int tier = character.actions[i].tier;
            character.actions[i].cooldown = 0;
            actions[i].GetComponentInChildren<Image>().sprite = tm.actionDict[action.GetSprite(tier)];
            actions[i].GetComponentInChildren<Text>().text = "";
			if (tier > 0) actions[i].SetActive(true);
        }

        // Set up our gold display
        currency.text = character.gold.ToString();
    }

    // Updates UI elements for our various actions
    public void UpdateActions()
    {
        for (int i = 0; i < character.actions.Length && i < actions.Length; i++)
        {
            character.actions[i].cooldown--;
            actions[i].GetComponentInChildren<Text>().text = character.actions[i].cooldown <= 0 ? "" : character.actions[i].cooldown.ToString();
        }
    }

    // Updates UI elements for our current amount of gold stored
    public void UpdateGold()
    {
        currency.text = character.gold.ToString();
    }
}
