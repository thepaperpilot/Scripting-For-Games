using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameObject brickParticle;

    void OnCollisionEnter()
    {
        Instantiate(brickParticle, transform.position, Quaternion.identity);
        // Destroy parent's parent
        Destroy(transform.parent.transform.parent.gameObject);
        GM.instance.DestroyBrick();
    }
}
