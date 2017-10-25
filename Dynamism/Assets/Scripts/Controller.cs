using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public Camera camera;           // Reference to camera in the world
    public ParticleSystem emitter;  // Reference to particle emitter in the world
    public Light light;             // Reference to light in the world
    public MeshRenderer cube;       // Reference to cube in the world
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            light.enabled = !light.enabled;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            light.intensity -= 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            light.intensity += 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Color color = GetRandomColor();
            light.color = color;
            emitter.startColor = color;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            light.type = LightType.Point;
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera.fieldOfView -= 0.2f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera.fieldOfView += 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            cube.material.color = GetRandomColor();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1));
    }
}
