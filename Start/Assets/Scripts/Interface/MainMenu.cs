using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button begin;        // Button telling us to begin the games
    public Button end;          // Button telling us to quit the game
    public Button credits;      // Button telling us to go to the credits scene
    public Button settings;     // Button telling us to go to the settings scene

	public void Start() {
        begin.onClick.AddListener(Begin);
        end.onClick.AddListener(End);
        credits.onClick.AddListener(Credits);
        settings.onClick.AddListener(Settings);
	}

	void Begin() {
        SceneManager.LoadScene("PlayerSelect", LoadSceneMode.Single);
	}

	void End() {
		Application.Quit ();
	}

    void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
