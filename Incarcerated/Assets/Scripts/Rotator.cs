using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public Vector3 rotation = new Vector3(15, 30, 45); // How much to rotate every second

    void Update()
    {
        // Better than animators, because you can use custom speeds for each axis without needing to make some hour long animation
        // (or however long it takes to get back to original state)
        transform.Rotate(rotation * Time.deltaTime);
    }
}
