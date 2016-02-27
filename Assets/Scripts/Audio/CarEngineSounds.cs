using UnityEngine;
using System.Collections;


public class CarEngineSounds : MonoBehaviour {

	[SerializeField] AudioSource engine;
	[SerializeField] AudioSource afterBurner;
	float maxPitch = 2f;
	void Start () {

	}

	void Update () {
		if (PlayerCar.s_instance.isAccelerating && engine.pitch < maxPitch) {
			engine.pitch += .01f;
		} else if (!PlayerCar.s_instance.isAccelerating && engine.pitch > 1) {
			engine.pitch -= .1f;
		}
	}

	public void PlayBoostSound() {
		afterBurner.Play ();
	}

}
