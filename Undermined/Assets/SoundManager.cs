using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioClip songToPlay;

	AudioSource audioSource;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			audioSource = GetComponent<AudioSource> ();
		} else
			Destroy (gameObject);
	}

	void Start() {
		PlayAudio ();
	}

	void PlayAudio() {
		audioSource.clip = songToPlay;
		audioSource.Play ();
	}
}
