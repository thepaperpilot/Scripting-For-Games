using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSun : MonoBehaviour
{
    private Light light;

	// Use this for initialization
    void Start()
    {
        light = GetComponent<Light>();		
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
        {
            light.enabled = !light.enabled;
        }
	}
}
