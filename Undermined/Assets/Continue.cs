using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

	public int level = 0;	// Level to continue to

	LevelManager lm;
	Button button;

	void Awake() {
		button = GetComponent<Button> ();
	}

	void Start() {
		lm = (LevelManager) GameObject.FindObjectOfType (typeof(LevelManager));
		button.onClick.AddListener(OnClick);
	}

	void OnClick() {
		lm.LoadLevel (level);
	}
}
