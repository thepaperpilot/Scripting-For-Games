using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterStore : MonoBehaviour {

	public GameObject[] actions;        				// The UI elements for this character's actions, each with an image and text component (potentially nested)
	public Text currency;               				// The UI element showing this character's current gold count

	private Character character;         				// The character this display is displaying the status of
    private LevelSelect ls;                             // The thing controlling us o.o

	public void Setup(LevelSelect ls, Character character)
	{
        this.ls = ls;

		// Make this gameObject active
		gameObject.SetActive(true);

		// Bind ourselves together
		this.character = character;
		//character.display = this;

		// Set up our actions
		for (int i = 0; i < character.actions.Length && i < actions.Length; i++)
		{
			Action action = ActionManager.actions[character.actions[i].name];
			int tier = character.actions[i].tier;
			Image image = actions [i].GetComponentInChildren<Image> ();
			image.sprite = ls.actionDict[action.GetSprite(tier)];
			image.color = tier == 0 ? new Color (1, 1, 1, .5f) : Color.white;
			Text text = actions [i].GetComponentInChildren<Text> ();
			text.text = action.GetUpgradeCost(tier).ToString();
			text.color = action.GetUpgradeCost(tier) > character.gold ? new Color (1, 1, 1, .5f) : Color.white;
			actions[i].SetActive(true);
            Button button = actions[i].GetComponentInChildren<Button>();
            Buy buy = new Buy() {
            	character = character,
            	i = i
            };
            button.onClick.AddListener(delegate { buy.BuyAction(this); });

            // Adding PointerEnter and PointerExit is more complicated
            // Create a trigger for pointer entering
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener(delegate { ls.SetTooltip(action.GetTooltip(tier)); });

            // Create a trigger for pointer exiting
            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener(delegate { ls.SetTooltip(""); });
        
            // Create a trigger handler on the button, and add our triggers
            // Don't use actions[i] because the button may be nested
            // We want it specifically on the button
            button.gameObject.AddComponent<EventTrigger>();
            button.GetComponent<EventTrigger>().triggers.Add(pointerEnter);
            button.GetComponent<EventTrigger>().triggers.Add(pointerExit);
        }

		// Set up our gold display
		currency.text = character.gold.ToString();
	}

	public void UpdateGold()
	{
		currency.text = character.gold.ToString();
	}

	// Created this wrapper because when I used a delegate, i would be the wrong value
	// and I have deadlines to meet, and can't look into how delegates, loops, and variable scopes
	// work together
	class Buy {
		
		public Character character;
		public int i;

		public void BuyAction(CharacterStore store) {
			Action action = ActionManager.actions [character.actions[i].name];
			if (character.gold >= action.GetUpgradeCost (character.actions[i].tier)) {
				character.gold -= action.GetUpgradeCost (character.actions[i].tier);
				// Enable this action
				if (character.actions[i].tier == 0) {
					Image image = store.actions [i].GetComponentInChildren<Image> ();
					image.color = Color.white;
				}
				character.actions[i].tier++;
				Text text = store.actions [i].GetComponentInChildren<Text> ();
				text.text = action.GetUpgradeCost(character.actions[i].tier).ToString();
				text.color = action.GetUpgradeCost(character.actions[i].tier) > character.gold ? new Color (1, 1, 1, .5f) : Color.white;
				store.UpdateGold ();
				store.ls.SetTooltip(action.GetTooltip(character.actions[i].tier));
			}
		}
	}	
}
