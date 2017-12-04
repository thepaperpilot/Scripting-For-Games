using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    public Button reset;        // Button that tells us to reset player data
    public Slider music;        // Slider for changing music volume
    public Slider sound;        // Slider for changing sound effects volume
    public AudioMixer mixer;    // AudioMixer for changing audio volumes

    void Start()
    {
        reset.onClick.AddListener(Reset);
        music.onValueChanged.AddListener(ChangeMusic);
        sound.onValueChanged.AddListener(ChangeSound);
    }

    void Reset()
    {
        PlayerPrefs.DeleteAll();
        // Reload everything
        if (LevelManager.instance != null) 
            LevelManager.instance.Start();
    }

    void ChangeMusic(float volume)
    {
        mixer.SetFloat("musicVol", volume);
    }

    void ChangeSound(float volume)
    {
        mixer.SetFloat("soundVol", volume);
    }
}
