using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ReturnButton : MonoBehaviour {

    private Button button;  // The button attached to this game object, which brings us to the main menu when clicked

    void Awake()
    {
        button = GetComponent<Button>();
    }

	void Start() {
        button.onClick.AddListener(Return);
	}
	
	void Return() {
        SceneManager.LoadScene("Main");
	}
}
