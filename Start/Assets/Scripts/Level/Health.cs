using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject healthBar;            // GameObject that gets scaled to represent health

    private float initialScale;             // How large our healthbar is at startup
    private float targetScale;              // health / maxHealth, used for animation purposes

    public void Setup()
    {
        targetScale = initialScale = healthBar.transform.localScale.y;
    }

    public void SetTargetScale(float scale)
    {
        targetScale = scale * initialScale;
    }

    // Updates the health bar on screen
    void Update()
    {
        // Interpolate to the target scale
        float newScale = Mathf.MoveTowards(healthBar.transform.localScale.y, targetScale, Time.deltaTime);
        healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x, newScale, healthBar.transform.localScale.z);
    }
}
