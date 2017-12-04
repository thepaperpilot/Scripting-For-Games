using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MusicManager : MonoBehaviour {

	/*
	The Procedurally Generated Music didn't really work well at all,
		it sounded terrible and honestly it was basically almost just
		someone else's work. I didn't like it, I didn't feel right using
		it, so it's gone now. I left some parts in here, but there was a
		bunch of other classes related to procedurally generated music
		which I have removed because I don't feel right saying I wrote
		them because of how closely they mirrored existing proc generated
		music implementations in Unity, and I don't want you looking at
		them while determining my grade.
	*/

    // Make this a singleton, so we can access it from everywhere but still have 
    // editable fields in the inspector
	public static MusicManager instance;

	/* Procedurally Generated Music Stuff
    public TextAsset chordData;
    public TextAsset progressionData;
    public TextAsset scaleData;
    public List<AudioClip> sounds;
    */

    //private MusicGenerator generator;
    //private Melody melody;
    private AudioSource bgm;                // Music to play in background on main menu

    void Awake()
    {
        // Prevent multiple instances of our singleton from being created
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
		instance = this;

        // Make this object persist between scenes (so we don't need to reload our generator)
        DontDestroyOnLoad(gameObject);
        //SceneManager.sceneLoaded += OnSceneLoaded;

        // Find required components
        bgm = GetComponent<AudioSource>();
    }

	/* All following code is solely dealing with procedurally generating music,
		which has been removed and no longer works

    void Start()
    {
        generator = new MusicGenerator();

        for (int i = 0; i < generator.numChannels; i++)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;
        }

        Generate();
	}

    // Start or stop the music depending on what scene we're in
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            // Turn off menu BGM
            bgm.volume = 0;

            // Start game BGM (procedurally generated)
            Play();

            // Generate new BGM for next game scene
            Generate();
        }
        else
        {
            // Turn on menu BGM
            bgm.volume = 1;

            // Stop game BGM
            StopCoroutine("PlaySong");
        }
    }

    public void Generate()
    {
        // Use coroutines so that the music is generated in a different thread
        StopCoroutine("GenerateSong");
        StartCoroutine("GenerateSong");
    }

    public void Play()
    {
        // Use coroutines so that the music can be played and stopped in a different thread
        StopCoroutine("PlaySong");
        StartCoroutine("PlaySong", melody);
    }

    private IEnumerator GenerateSong()
    {
        melody = generator.GenerateMelody();
        yield break;
    }

    private IEnumerator PlaySong(Melody melody)
    {
        int pitchindex = 0;
        int durationindex = 0;
        int velocityindex = 0;
        int sampleindex = 0;
        int channelindex = 0;

        AudioSource[] audiosources = GetComponents<AudioSource>();

        while (true)
        {
            float pitch = melody.frequencies[pitchindex];
            float duration = melody.durations[durationindex];
            float velocity = melody.volumes[velocityindex] / 4;
            AudioClip sample = melody.sounds[sampleindex];

            AudioSource a_s;
            if (melody.specifychannels)
            {
                a_s = audiosources[melody.channels[channelindex]];
            }
            else
            {
                a_s = audiosources[channelindex];
            }

            a_s.pitch = pitch;
            a_s.volume = velocity;
            a_s.PlayOneShot(sample);
            // Wait until next beat
            yield return new WaitForSeconds(duration);

            pitchindex = (pitchindex + 1) % melody.frequencies.Count;
            durationindex = (durationindex + 1) % melody.durations.Count;
            velocityindex = (velocityindex + 1) % melody.volumes.Count;
            sampleindex = (sampleindex + 1) % melody.sounds.Count;
            if (melody.specifychannels)
            {
                channelindex = (channelindex + 1) % melody.channels.Count;
            }
            else
            {
                channelindex = (channelindex + 1) % audiosources.Length;
            }
        }
    }

    */
}
