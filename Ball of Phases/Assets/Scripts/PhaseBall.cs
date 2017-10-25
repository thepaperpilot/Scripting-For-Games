using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
	}
}
