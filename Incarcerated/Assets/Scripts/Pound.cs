using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pound : MonoBehaviour {

    public float[] angles = new float[] { 90 }; // Array of angles to rotate (looped)

    private int index = 0;                      // Current position in array

    // This get's called by the moving enemy's animation at the end of each loop using Unity's animations event system
    // It tells the enemy to move forward, and then rotate itself according to the angles array,
    // so that we can write its path (e.g. {90} will just turn in a square, or { 90, 180 } will do a plus sign)
	public void Step () {
        transform.parent.transform.Translate(new Vector3(0, 0, transform.parent.transform.localScale.z));
        transform.parent.transform.Rotate(Vector3.up, angles[index]);
        index++;
        if (index == angles.Length) index = 0;
	}
}
