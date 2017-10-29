using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Player : MonoBehaviour {

    public int maxHealth = 100;             // How much health our player can carry
    public int maxAmmo = 15;                // How much ammo we can carry
    public float effectDuration = .2f;      // How long to apply post processing effects when the player is hurt

    public GameObject healthUI;             // A gameobject we set its xscale to, to represent current health
    public TextMesh ammoUI;                 // TextMesh that sits on our gun telling us how much ammo we have
    public PostProcessingProfile profile;   // Post Processing file for when the player is hurt
    public GameObject paintGun;             // Our paint gun, used to enable it after picking it up

    private int health;                     // How much health our player currently has
    private int ammo = 15;                  // How much ammo our player currently has
    private float targetScale = 1;          // The current scale our health should animate to
    private float timer = 0;                // Timer for hurt camera effects
    private float timerScale = 1;           // How much to increase the timer each frame - set to negative when you die

    private AudioSource hurt;               // Sound we play when we're hit by an enemy
    private AudioSource death;              // Sound we play when we die

    void Awake()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        // sounds[0] are our footstep sounds
        hurt = sounds[1];
        death = sounds[2];
    }

    void Start()
    {
        health = maxHealth;
        ammo = maxAmmo;
        ammoUI.text = ammo.ToString();
    }

    void Update()
    {
        UpdateHealthUI();
        UpdatePostProcessing();
    }

    // Updates the health bar on screen
    void UpdateHealthUI()
    {
        // Interpolate to the target scale
        float newScale = Mathf.MoveTowards(healthUI.transform.localScale.x, targetScale, Time.deltaTime);
        healthUI.transform.localScale = new Vector3(newScale, healthUI.transform.localScale.y, healthUI.transform.localScale.z);
    }

    // Updates vignette and graininess that happens whenever player is damaged or killed
    void UpdatePostProcessing()
    {
        // When we're damaged, apply some vignette and graininess to the camera
        timer -= Time.deltaTime * timerScale;
        if (timer < 0) timer = 0;

        // Apply vignette
        VignetteModel.Settings vignetteSettings = profile.vignette.settings;
        vignetteSettings.intensity = Mathf.Sin(timer / effectDuration);
        profile.vignette.settings = vignetteSettings;
        
        // Apply graininess
        GrainModel.Settings grainSettings = profile.grain.settings;
        grainSettings.intensity = Mathf.Sin(timer / effectDuration);
        profile.grain.settings = grainSettings;
    }

    // Damages the player
    public void Damage(int amount)
    {
        // Take damage
        health -= amount;

        // Update UI
        targetScale = health / (float)maxHealth;
        timer = effectDuration;

        // Handle death
        if (health <= 0)
        {
            health = 0;
            // Game Over
            death.Play();
            timer = 0;
            timerScale = -.25f;
            Time.timeScale = .25f;
            StartCoroutine("End");
        }
        else
        {
            hurt.Play();
        }
    }

    // Heals the player
    public void AddHealth(int amount)
    {
        // Add Health
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        // Update UI
        targetScale = health / (float) maxHealth;
    }

    // Adds ammo to the player
    public void AddAmmo(int amount)
    {
        // Add ammo
        ammo += amount;
        if (ammo > maxAmmo)
            ammo = maxAmmo;

        // Update UI
        ammoUI.text = ammo.ToString();
    }

    // Returns true if ammo was used
    // So if it returns false, don't use the ammo
    public bool UseAmmo()
    {
        if (ammo > 0)
        {
            ammo--;

            // Update UI
            ammoUI.text = ammo.ToString();
            return true;
        }
        return false;
    }

    // Called whenever the paintgun gets picked up, and enables the function one on the player
    public void EnablePaintGun()
    {
        paintGun.SetActive(true);
    }

    // Wait a little, then quit the level
    IEnumerator End()
    {
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
