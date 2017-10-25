using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGems : MonoBehaviour {

    public float xRange = 20;
    public float yRange = 6;
    public int numObjects = 32;
    public GameObject[] objects;

	void Start () {
        Spawn();
	}

    void Spawn()
    {
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 spawnLoc = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange) - 1, -0.2f);
            int objectPick = Random.Range(0, objects.Length - 1);
            Instantiate(objects[objectPick], spawnLoc, Random.rotation);
        }
    }
}
