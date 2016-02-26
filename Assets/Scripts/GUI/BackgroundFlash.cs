using UnityEngine;
using System.Collections;

public class BackgroundFlash : MonoBehaviour {

	public bool fadeRed, fadeGreen;
	float startTime, fadeTime = .3f;
	Camera thisCam;
	Color startColor;
	public static BackgroundFlash s_instance;

	void Awake () {
		s_instance = this;
	}

	void Start() {
		thisCam = GetComponent<Camera> ();
		startColor = thisCam.backgroundColor;
	}

	
	void Update() {
		if (fadeGreen) {
			float timePassed = (Time.time - startTime);
			float fracJourney = timePassed / fadeTime;
			thisCam.backgroundColor = Color.Lerp (new Color (46f/255, 204f/255, 113f/255, 1f), startColor, fracJourney);
			if (fracJourney >= 1) {
				fadeGreen = false;
			}
		}
		if (fadeRed) {
			float timePassed = (Time.time - startTime);
			float fracJourney = timePassed / fadeTime;
			thisCam.backgroundColor = Color.Lerp (new Color (192f/255, 57f/255, 43f/255, 1f), startColor, fracJourney);
			if (fracJourney >= 1) {
				fadeRed = false;
			}
		}

	}
	
	public void FadeRed() {
		startTime = Time.time;
		fadeRed = true;
	}
	public void FadeGreen() {
		startTime = Time.time;
		fadeGreen = true;
	}
}
