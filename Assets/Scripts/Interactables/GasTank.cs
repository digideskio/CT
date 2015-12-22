using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GasTank : InteractableObject {

	public float gasLeft;
	public float costOfGas;

	bool isPlayerInProximity;

	[SerializeField]
	Text costOfGasText;

	// Use this for initialization
	void Start () {
		costOfGasText.text = "Gas\n" + "$" + costOfGas;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			isPlayerInProximity = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			isPlayerInProximity = false;
		}
	}

	void Update () {
		if (isPlayerInProximity && isReceivingInteract) {

		}
	}

	void GiveGasToPlayer () {

	}
}
