using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    public Transform origin;
    public float speed = 4f;
    public Gun gun;
    public GameManager gameManager;
    public GameObject popupPrefab;

    private Vector3 target;
    private GameObject childObject;
    private LineRenderer lineRenderer;
    private bool hitJewel;
    private bool retracting;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	void Update () {
        float step = speed * Time.deltaTime;
        if (retracting && hitJewel)
        {   
            // If rock, take longer to return
            step *= childObject.GetComponent<JewelValue>().jewelValue == 25 ? .6f : 1.25f;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, transform.position);
        if (transform.position == origin.position && retracting)
        {
            gun.CollectedObject();
            int jewelValue;
            if (hitJewel)
            {
                jewelValue = childObject.GetComponent<JewelValue>().jewelValue;
                gameManager.AddPoints(jewelValue);
                GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity) as GameObject;
                popup.GetComponentInChildren<TextMesh>().text = "" + jewelValue;
                Destroy(popup, 1);
                hitJewel = false;
            }
            Destroy(childObject);
            gameObject.SetActive(false);
        }
	}

    public void ClawTarget(Vector3 pos)
    {
        target = pos;
    }

    void OnTriggerEnter(Collider other)
    {
        retracting = true;
        target = origin.position;

        if (other.gameObject.CompareTag("Jewel"))
        {
            hitJewel = true;

            childObject = other.gameObject;
            other.transform.SetParent(this.transform);
        }
    }
}
