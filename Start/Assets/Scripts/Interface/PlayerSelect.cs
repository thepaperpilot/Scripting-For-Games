using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {

    public Image[] panels;          // The 4 player panels 
    public Sprite unreadySprite;    // The panel background to use before a player is ready
    public Sprite readySprite;      // The panel background to use when a player is ready
    public Text readyText;          // The text displaying how many players are ready
    public GameObject transition;   // The game object that envelops the screen between scenes because I think it looks cool
    public GameObject keyboard;     // Prefab for a keyboard's actions controls
    public GameObject controller;   // Prefab for a controller's actions controls

    private int players = 0;        // How many players have joined
    private int ready = 0;          // How many players have readied up
    private List<string> playingControllers = new List<string>(4) {null,null,null,null};    // Ordered list of what input each player is using
    private List<string> readyControllers = new List<string>(4);                            // Unordered list of inputs that have readied up
    private string[] allControllers = {"kb","c1","c2","c3","c4"};                           // All possible inputs

	void Update () {
        // Iterate through each of our potential controllers
        for (int i = 0; i < allControllers.Length; i++) {
            // Join Game
            if (!playingControllers.Contains(allControllers[i]) && Input.GetButton("Fire1_" + allControllers[i]))
            {
                int player = AddPlayer(allControllers[i]);
                if (player > -1) {
                    panels[player].color = Color.white;
                    players++;
                    readyText.text = ready + "/" + players + " Players Ready";
                    Instantiate(i == 0 ? keyboard : controller, panels[player].gameObject.transform);
                }
            }

            // Leave Game
            if (playingControllers.Contains(allControllers[i]) && !readyControllers.Contains(allControllers[i]) && Input.GetButton("Fire2_" + allControllers[i]))
            {
                Image panel = panels[playingControllers.IndexOf(allControllers[i])];
                playingControllers[playingControllers.IndexOf(allControllers[i])] = null;
                Debug.Log(playingControllers.Count);
                panel.color = new Color(1, 1, 1, .5f);
                players--;
                readyText.text = ready + "/" + players + " Players Ready";
                if (ready > 0 && ready == players)
                {
                    StartGame();
                }
                // Destroy any controls panels
                Transform controls = panel.gameObject.transform.Find("KeyboardControls(Clone)");
                if (controls != null)
                    Destroy(controls.gameObject);
                controls = panel.gameObject.transform.Find("ControllerControls(Clone)");
                if (controls != null)
                    Destroy(controls.gameObject);
            }

            // Ready up
            if (playingControllers.Contains(allControllers[i]) && !readyControllers.Contains(allControllers[i]) && Input.GetButton("Start_" + allControllers[i]))
            {
                readyControllers.Add(allControllers[i]);    // Ready list doesn't need to be sorted
                panels[playingControllers.IndexOf(allControllers[i])].sprite = readySprite;
                ready++;
                readyText.text = ready + "/" + players + " Players Ready";
                if (ready == players)
                {
                    StartGame();
                }
            }

            // Unready
            if (playingControllers.Contains(allControllers[i]) && readyControllers.Contains(allControllers[i]) && Input.GetButton("Fire2_" + allControllers[i]))
            {
                readyControllers.Remove(allControllers[i]);
                panels[playingControllers.IndexOf(allControllers[i])].sprite = readySprite;
                ready--;
                readyText.text = ready + "/" + players + " Players Ready";
            }
        }
	}

    // Finds the next available player slot
    int AddPlayer(string controller)
    {
        int i = 0;
        while (i < playingControllers.Count && playingControllers[i] != null)
            i++;
        if (i < playingControllers.Count) playingControllers[i] = controller;
        return i < playingControllers.Count ? i : -1;
    }

    void StartGame()
    {
        // Wait WHAT?!?! C# has support for inline functions?
        // Wow its been holding out on me
        playingControllers.RemoveAll(x => x == null);
        // I mean seriously wasn't that incredible?

        TurnManager.controllers = playingControllers.ToArray();
        if (LevelManager.currLevel == 0)
            LevelManager.instance.Transition(transition, "Game");
        else SceneManager.LoadScene("LevelSelect");
    }
}
