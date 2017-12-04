using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LightIntensitySlider : MonoBehaviour {

	public Text text;
	public Light light;

	private Slider slider;

	void Awake() {
		slider = GetComponent<Slider> ();
	}

	void Start() {
		slider.onValueChanged.AddListener (OnValueChanged);
	}

	void OnValueChanged(float value) {
		light.intensity = value;
		text.text = value.ToString();
	}
}
