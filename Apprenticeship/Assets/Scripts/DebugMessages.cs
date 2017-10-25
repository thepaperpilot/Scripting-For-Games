using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMessages : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Hello!");
        //ChangePlayerColor();
        Debug.Log("I've returned!");
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("I'm Still Here!");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayerColor();
            //Debug.Log("Spacebar done got pressed");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    void ChangePlayerColor() {
        Debug.Log("Called Arbitrary Function!");
    }
}
