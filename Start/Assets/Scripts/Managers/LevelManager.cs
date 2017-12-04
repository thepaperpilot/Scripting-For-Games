using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour {

	// Make this a singleton, so we can access it from everywhere but still have 
	// editable fields in the inspector
	public static LevelManager instance;

    public static int currLevel = 0;    // Index of the current level
    public static int maxLevels = 0;    // How many total levels exist

    // You can't have dictionaries in the inspector, 
    // so I make a list using structs instead, 
    // then parse them into a dictionary
    [Serializable]
    struct Tile
    {
        public string name;
        public GameObject prefab;
    }

    [Serializable]
    struct Beat
    {
        public string name;
        public AudioClip clip;
    }

    // I made this because JsonUtility also doesn't
    // support plain arrays, so I wrap it in this object
    [Serializable]
    struct CharactersContainer
    {
        public Character[] characters;
    }

    [HideInInspector]
    public Level level;                                     // Our current level, equal to levels[currLevel]
    [HideInInspector]
    public Character[] characters;                          // All player characters' default states
    [HideInInspector]
    public List<Character> activeCharacters;                // Our current set of characters in their current state
    [HideInInspector]
    public GameObject transition;                           // If we're transitioning scenes and using an animated gameObject to do so
    public float tileSize = 2;                              // Distance between the centers of adjacent tiles
    public Dictionary<string, GameObject> tilePrefabs;      // Dictionary of our tile prefabs, that make up our map
    
    // These are fields for the inspector, that then get parsed into the fields we actually use
    [SerializeField]
    private TextAsset[] levelFiles;                         // JSON files that hold our level info
    [SerializeField]
    private TextAsset characterFile;                        // JSON file that holds our characters' default states
    [SerializeField]
    private Tile[] tiles;                                   // Structs of (string, GameObject) we construct a dict out of
    [SerializeField]
    private Beat[] clips;                                   // Structs of (string, AudioClip) we construct a dict out of

    private Level[] levels;                                 // All our levels
    private Dictionary<string, AudioClip> beats;            // Dictionary of our tile prefabs, that make up our map

    private AudioSource audioSource;                        // Audio source component, used for playing beats

    // This is a MonoBehavior, and needs to make these checks on Awake(), because I want
    // to be able to edit the variables from within the inspector. The way this is setup,
    // I can edit those variables in the inspector, but not worry about creating multiple
    // levelManagers by accident, or loading the level files multiple times, etc.
    void Awake()
    {
        // Prevent multiple instances of our singleton from being created
		if (instance != null) {
			Destroy(gameObject);
			return;
		}
		instance = this;

        // Make this object persist between scenes (so we don't need to reload our levels)
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Find required components
        audioSource = GetComponent<AudioSource>();
    }

	public void Start () {
        // Read character data
        characters = JsonUtility.FromJson<CharactersContainer>(characterFile.text).characters;

        // Read in our characters
        if (PlayerPrefs.HasKey("characters"))
        {
            // Load character data
            activeCharacters = new List<Character>(JsonUtility.FromJson<Character[]>(PlayerPrefs.GetString("characters")));
        }
        else
        {
            // If this is our first time, give them their first unit
            activeCharacters = new List<Character>(new Character[] { characters[0] });
        }

        if (PlayerPrefs.HasKey("level")) {
            currLevel = PlayerPrefs.GetInt("level");
        }

        // Read level data
        levels = new Level[levelFiles.Length];
        maxLevels = levelFiles.Length;
        for (int i = 0; i < levelFiles.Length; i++)
        {
            levels[i] = JsonUtility.FromJson<Level>(levelFiles[i].text);
        }

        // Construct tile prefabs dictionary
        tilePrefabs = new Dictionary<string, GameObject>();
        for (int i = 0; i < tiles.Length; i++)
        {
            tilePrefabs.Add(tiles[i].name, tiles[i].prefab);
        }

        // Construct beats dictionary
        beats = new Dictionary<string, AudioClip>();
        for (int i = 0; i < clips.Length; i++)
        {
            beats.Add(clips[i].name, clips[i].clip);
        }

        // Load current level
        if (levels.Length > currLevel && SceneManager.GetActiveScene().name == "Game") StartCoroutine(LoadLevel(currLevel));
    }

    // Loads the level at the current index
    IEnumerator LoadLevel(int index)
    {
        yield return new WaitForEndOfFrame();

        // Set up level
        level = levels[index];
        level.Setup();
        audioSource.clip = beats[level.beat];
        audioSource.Play();
        Time.timeScale = level.timeScale;
    }

    // Remove map when scene gets changed or reloaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        if (transition != null)
        {
            Destroy(transition.transform.root.gameObject, 2);
            transition.GetComponent<Animator>().SetTrigger("Transition");
        }
        transition = null;
        audioSource.Stop();

        if (levels != null && levels.Length > currLevel && scene.name == "Game") StartCoroutine(LoadLevel(currLevel));
    }

    // Skip a beat sound
    public void SkipBeat()
    {
        audioSource.volume = 0.3f;
    }

    // Resume beat sounds
    public void ResumeBeat()
    {
        audioSource.volume = 1;
    }

    // Checks if we're currently in a state in which the game should end
    public void CheckGameOver()
    {
        // Count our entities
        int enemies = 0;
        int players = 0;
        foreach (Unit unit in level.units)
        {
            if (unit as Character != null)
                players++;
            else if (unit as Enemy != null)
                enemies++;
        }

        if (enemies == 0)
        {
            // You win (even if there are no players, if that situation ever arises)
            currLevel++;
            Transition(GameObject.FindGameObjectWithTag("victory").transform.GetChild(0).gameObject, "LevelSelect");
            Destroy(GameObject.FindObjectOfType<ActionManager>().gameObject);
        }
        else if (players == 0)
        {
            // You lose
            GameObject.FindGameObjectWithTag("defeat").transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(LoadSceneAfterDelay("LevelSelect", 6));
            Destroy(GameObject.FindObjectOfType<ActionManager>().gameObject);
        }
    }

    // Moves to another scene, with a gameobject that obscures the screen during the transition
    public void Transition(GameObject transition, string scene)
    {
        this.transition = transition;
        transition.SetActive(true);
        DontDestroyOnLoad(transition.transform.root.gameObject);
        StartCoroutine(LoadSceneAfterDelay(scene, 4));
    }
    
    // Moves to another scene after a given amount of seconds
    IEnumerator LoadSceneAfterDelay(string scene, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }

    // Used so actions can make sounds when they do stuff
    public void PlayOneShot(string clip) {
        audioSource.PlayOneShot(beats[clip]);
    }
}
