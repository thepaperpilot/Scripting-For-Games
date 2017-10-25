using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    static bool restarted = false;  // Whether or not we've restarted the level

    public Text timerLabel;         // Reference to the UI element showing how long we've taken
    public Text winLabel;           // Reference to the UI element that we show when we've won
    public Text loseLabel;          // Reference to the UI element that we show when we've died
    public Text timePopupPrefab;    // Reference to the UI element we create when we pick up a time pickup
    public GameObject levelLabels;  // Reference to the object with all the UI elements we only show during the level overview
    public Rigidbody player;        // Reference to the player's rigidbody, for physics manipulations
    public GameObject canvas;       // Reference to the UI canvas, for adding time pickup popups to
    public Countdown countdown;     // Reference to the countdown we display at the beginning of the level
    public GameObject overview;     // Reference to the camera we use to show the level overview

    private float time;             // How long the player has taken thus far
    private bool started;           // Whether or not we're currently counting the time taken

	void Start () {
		// Hide the cursor
        Cursor.visible = false;
        // Reset the time scale (we lower it when we win or lose)
        Time.timeScale = 1f;
	}
	
	void Update () {
		HandleInput ();
		UpdateTimer ();
	}

	void HandleInput() {
		// Handle inputs dealing with the application and scene
		// Begin the level
		if (Input.GetKeyDown(KeyCode.Return) || restarted)
		{
			Begin();
		}
		// Quit
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// "Quitting" in a level means quitting the level
			// Quitting outside of one means quitting the game
			if (overview.activeInHierarchy)
			{
				Application.Quit();
			}
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
		// Restart the level
		if (Input.GetKeyDown(KeyCode.Backspace) && !overview.activeInHierarchy)
		{
			restarted = true;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		// Select next level
		if (overview.activeInHierarchy && Input.GetKeyDown(KeyCode.LeftArrow) && SceneManager.GetActiveScene().buildIndex > 0)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
		// Select previous level
		if (overview.activeInHierarchy && Input.GetKeyDown(KeyCode.RightArrow) && SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	void UpdateTimer() {
		if (started)
		{
			time += Time.deltaTime;
			float minutes = time / 60;
			float seconds = time % 60;
			float fraction = (time * 100) % 100;
			timerLabel.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
		}
	}

    public void StartLevel()
    {
        // Start timer and unlock the player's movement
        started = true;
        player.constraints = RigidbodyConstraints.None;
    }

    public void FinishLevel()
    {
        // Show the "you win" message, stop the timer, slow down the game, and set timer to auto-quit
        float minutes = time / 60;
        float seconds = time % 60;
        winLabel.text = "You Win!\nYour time:\n" + string.Format("{0:0} Minutes, {1:00} Seconds", minutes, seconds);
        started = false;
        Time.timeScale = .25f;
        StartCoroutine("End");
    }

    public void Die()
    {
        // Show the "you died" message, stop the timer, slow down the game, and set timer to auto-quit
        loseLabel.text = "You Died";
        started = false;
        Time.timeScale = .25f;
        StartCoroutine("End");
    }

    // Time pickup, remove the time and add a popup for how many seconds got removed
    public void RemoveTime(float seconds)
    {
        time -= seconds;
        Text text = Object.Instantiate(timePopupPrefab);
        text.transform.SetParent(canvas.transform);
        text.text = "-" + seconds + " Seconds";
        Destroy(text, 1);
    }

    // Switch from overview mode to game mode, and start the countdown
    void Begin()
    {
        countdown.StartCountdown();
        overview.SetActive(false);
        restarted = false;
        levelLabels.SetActive(false);
    }

    // Wait a second, then quit the level
    IEnumerator End()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
