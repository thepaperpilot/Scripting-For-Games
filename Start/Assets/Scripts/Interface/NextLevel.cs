using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class NextLevel : MonoBehaviour {

    public GameObject transition;   // The game object that envelops the screen between scenes because I think it looks cool
    
	private Button button;  		// The button attached to this game object, which brings us to the next level when clicked

    void Awake()
    {
        if (LevelManager.currLevel == LevelManager.maxLevels)
        	// If there isn't a next level, just remove the button
        	Destroy(gameObject);
        else button = GetComponent<Button>();
    }

	void Start() {
        button.onClick.AddListener(Continue);
	}
	
	void Continue() {
        LevelManager.instance.Transition(transition, "Game");
	}
}
