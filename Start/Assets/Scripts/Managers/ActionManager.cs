using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionManager : MonoBehaviour
{
    public static Dictionary<string, Action> actions = new Dictionary<string, Action>
    {
        {"melee", new AttackAction()},
        {"heal", new HealAction()}
    };

    public enum Direction
    {
        UP, RIGHT, DOWN, LEFT, NULL
    }

	public GameObject missedBeat;                   // Red X that appears when we miss a beat or are too early
	public MeshRenderer timerCyclinder;			    // The cylinder representing time left, so we can disable it when we miss a beat
    public TextMesh actionPoints;                   // Number that appears on controller for number of actions remaining

    private LevelManager lm;					    // Reference to our level manager
    private TurnManager tm;					        // Reference to our turn manager
    private string controller;                      // The current controller
    private Direction dir = Direction.NULL;		    // Direction our unit wants to move
    private Direction facing = Direction.UP;        // Direction our unit is facing
    private bool onBeat = false;				    // Whether or not inputs would currently be on beat
    private Unit character = null;		    	    // The currently active unit
    private Unit.InvAction action = null;           // The currently chosen action
    private Camera camera;                          // The camera in the scene

	void Start () {
        lm = LevelManager.instance;
        tm = GameObject.FindObjectOfType<TurnManager>();
        camera = GameObject.FindObjectOfType<Camera>();
	}
	
	void Update () {
        if (character == null || character as Character == null || controller == null)
            return;

        if (onBeat)
        {
            // On beat
            HandleInput();
        }
        else
        {
            // Off beat
            if (Input.GetAxisRaw("Horizontal_" + controller) != 0 || Input.GetAxisRaw("Vertical_" + controller) != 0 && !missedBeat.activeInHierarchy)
            {
                missedBeat.SetActive(true);
				timerCyclinder.enabled = false;
                lm.SkipBeat();
                HandleInput();
            }
        }
	}

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal_" + controller);
        float v = Input.GetAxisRaw("Vertical_" + controller);
        // Only one movement direction allowed per beat
        if (h == 1 && v == 0)
        {
            // Turn right
            dir = (Direction)(((int)facing + 1) % 4);
        }
        else if (h == -1 && v == 0)
        {
            // Turn left
            dir = (Direction)(((int)facing + 3) % 4);
        }
        else if (v == 1 && h == 0)
        {
            dir = facing;
        }
        else if (v == -1 && h == 0)
        {
            // Turn around
            dir = (Direction)(((int)facing + 2) % 4);
        }

        if (dir != Direction.NULL)
        {
            if (Input.GetButton("Fire1_" + controller) && character.actions.Length > 0)
                action = character.actions[0];
            if (Input.GetButton("Fire2_" + controller) && character.actions.Length > 1)
                action = character.actions[1];
            if (Input.GetButton("Fire3_" + controller) && character.actions.Length > 2)
                action = character.actions[2];
            if (Input.GetButton("Fire4_" + controller) && character.actions.Length > 3)
                action = character.actions[3];
        }
    }

    // Called when the first EndBeat would happen, to synchronize everything
    public void FirstBeat()
    {
        lm.level.FirstBeat();
    }

    // Called when input is now valid to be considered on beat
    public void StartBeat()
    {
        onBeat = true;
        controller = tm.GetController();
    }

    // Called when input is no longer valid to be considered on beat
    public void EndBeat()
    {
        onBeat = false;
    }

    // Called upon a new beat
    public void Beat()
    {
        // Find our local position
        Vector3 localPos = transform.localPosition;

        // If we're an enemy, get this beat's actions
        if (character as Enemy != null)
        {
            dir = (character as Enemy).direction;
            action = (character as Enemy).action;
        }
        else 
        {
            // Fake no input if its a player's turn and we missed this beat
            if (missedBeat.activeInHierarchy && dir != Direction.NULL)
            {
                switch (dir)
                {
                    case ActionManager.Direction.UP:
                        character.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case ActionManager.Direction.RIGHT:
                        character.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case ActionManager.Direction.DOWN:
                        character.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case ActionManager.Direction.LEFT:
                        character.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                        break;
                }
                character.facing = dir;
                dir = Direction.NULL;
            }
        }

        // Find our new character (while updating their position and such)
        character = lm.level.Beat(dir, action);
        facing = character.facing;
        dir = Direction.NULL;
        action = null;

		// If the new character is an enemy, tell it to start calculating its next move
		if (character as Enemy != null)
			(character as Enemy).Think (this, lm.level);

        // Apply our local position to our new parent (current character)
        transform.parent = character.gameObject.transform;
        transform.localPosition = localPos;
        switch (facing)
        {
            case Direction.UP:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.RIGHT:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.DOWN:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.LEFT:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }

        // Reset beat
        missedBeat.SetActive(false);
		timerCyclinder.enabled = true;
		actionPoints.text = lm.level.actionPoints.ToString();
        actionPoints.transform.LookAt(camera.transform);
        actionPoints.transform.Rotate(0, 180, 0);
        lm.ResumeBeat();
    }
}
