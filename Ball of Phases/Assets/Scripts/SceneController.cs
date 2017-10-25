using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    private GameObject scene1;
    private GameObject scene2;

	// Use this for initialization
	void Start () {
        scene1 = GameObject.Find("Scenery");
        scene2 = GameObject.Find("Scenery-2");
        scene2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
        {
            scene1.SetActive(true);
            scene2.SetActive(false);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log(scene1);
            scene1.SetActive(false);
            scene2.SetActive(true);
        }


        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
