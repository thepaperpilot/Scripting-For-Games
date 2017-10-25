using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    public LevelManager lm;             // Reference to level manager, to start the level

    private Animator animation;         // Animation component, so we can start and stop it

    void Start()
    {
        // Set stuff up, don't autostart the animation
        GetComponent<Text>().fontSize = Screen.height / 2;
        animation = GetComponent<Animator>();
        animation.enabled = false; ;
    }

    public void StartCountdown()
    {
        // Start the animation, and a coroutine
        animation.enabled = true;
    }

    public void StartLevel()
    {
        lm.StartLevel();
    }
}
