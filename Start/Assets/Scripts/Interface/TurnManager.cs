using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {

    // Constants
    const int TURN_ORDER_QUANTITY = 6;	                // How many turns ahead to display turn order for

    public static string[] controllers;                 // Array of strings to append to input buttons to refer to that input mechanism

    public GameObject portrait;                         // Reference to our prefab we use for each character portrait
    public CharacterDisplay[] characterDisplays;        // Each of the Action Managers (UI components) for our characters (in same order as characters)
    public Sprite[] controllerSprites;                  // Sprites for each player
    [HideInInspector]
    public Dictionary<string, Sprite> actionDict;       // Dictionary of action sprites

    [SerializeField]
    private SpriteDef[] portraits;                      // Used to create a dictionary of portrait sprites
    [SerializeField]
    private SpriteDef[] actionSprites;                  // Used to create a dictionary of action sprites
    private List<Unit> order;		                    // The next TURN_ORDER_QUANTITY turns
    private Dictionary<string, Sprite> portraitDict;    // Dictionary of portrait sprites
    private GameObject[] portraitObjects;               // The list of portraits' game objects
    private Image[] controllerImages;                   // Indicators of which player's turn is up
    private int beats = 0;                              // Number of beats that have occured in this level
    private int player = 0;                             // Current player controlling stuff

    [Serializable]
    struct SpriteDef
    {
        public string name;
        public Sprite sprite;
    }

    void Start()
    {
        // Construct sprite dictionaries
        portraitDict = new Dictionary<string, Sprite>();
        for (int i = 0; i < portraits.Length; i++)
        {
            portraitDict.Add(portraits[i].name, portraits[i].sprite);
        }
        actionDict = new Dictionary<string, Sprite>();
        for (int i = 0; i < actionSprites.Length; i++)
        {
            actionDict.Add(actionSprites[i].name, actionSprites[i].sprite);
        }

        // Set up UI
        portraitObjects = new GameObject[TURN_ORDER_QUANTITY];
        // Make the first portrait large and custom
        // I don't know how to get a child gameObject, but fortunately in this situation the following works
        portraitObjects[0] = Instantiate(portrait, transform).GetComponentInChildren<Image>().gameObject;
        portraitObjects[0].transform.localPosition = new Vector3(-375, 135, 0);
        portraitObjects[0].transform.localScale *= 1.5f;
        controllerImages = new Image[TURN_ORDER_QUANTITY];
        GameObject gameObject = new GameObject();
        controllerImages[0] = gameObject.AddComponent<Image>();
        gameObject.transform.SetParent(transform);
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = new Vector3(-375, 45, 0);
        // The other portraits are small and bland
        for (int i = 1; i < TURN_ORDER_QUANTITY; i++)
        {
            portraitObjects[i] = Instantiate(portrait, transform).GetComponentInChildren<Image>().gameObject;
            portraitObjects[i].transform.localPosition = new Vector3(-200 + (i - 1) * 600 / (TURN_ORDER_QUANTITY - 2), 90, 0);
            gameObject = new GameObject();
            controllerImages[i] = gameObject.AddComponent<Image>();
            gameObject.transform.SetParent(transform);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = new Vector3(-200 + (i - 1) * 600 / (TURN_ORDER_QUANTITY - 2), 45, 0);
        }
    }

    public void SetupDisplay(List<Character> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characterDisplays[i].Setup(characters[i]);
        }
    }

    // Sets the new currently active unit and regenerates the next TURN_ORDER_QUANTITY turn orders
    public Unit NextTurn(List<Unit> units, List<Character> characters)
    {
        if (order != null && order[0] as Enemy != null)
            player = (player + 1) % controllers.Length;
        order = new List<Unit>(TURN_ORDER_QUANTITY);
        int turns = beats;
        while (order.Count < TURN_ORDER_QUANTITY)
        {
            int bestTurns = units[0].speed - (turns % units[0].speed);
            List<Unit> bestUnit = new List<Unit>();
            bestUnit.Add(units[0]);
            for (int i = 1; i < units.Count; i++)
            {
                int currTurns = units[i].speed - (turns % units[i].speed);
                if (currTurns < bestTurns)
                {
                    bestUnit = new List<Unit>();
                    bestUnit.Add(units[i]);
                    bestTurns = currTurns;
                }
                else if (currTurns == bestTurns)
                {
                    bestUnit.Add(units[i]);
                }
            }
            order.AddRange(bestUnit);
            turns += bestTurns;
        }

        // Update Action Cooldowns
        if (units[0] as Character != null) {
            int index = characters.IndexOf((Character) units[0]);
            if (index != -1)
                characterDisplays[index].UpdateActions();
        }

        // Update UI
        int tempPlayer = player;
        for (int i = 0; i < TURN_ORDER_QUANTITY; i++)
        {
            Image image = portraitObjects[i].GetComponent<Image>();
            image.sprite = portraitDict[order[i].type];
            image.color = Color.white;
            if (order[i] as Character != null) {
                controllerImages[i].color = Color.white;
                controllerImages[i].sprite = controllerSprites[tempPlayer];
                tempPlayer = (tempPlayer + 1) % controllers.Length;
            }
            else
            {
                controllerImages[i].color = Color.clear;
            }
        }

        // Set up new turn
        Unit unit = order[0];
        beats += unit.speed - (beats % unit.speed);
        return unit;
    }

    // Gets the controller for the current player, or null if its an enemy's turn
    public string GetController()
    {
        return order == null ? null : order[0] as Enemy == null ? controllers[player] : null;
    }
}
