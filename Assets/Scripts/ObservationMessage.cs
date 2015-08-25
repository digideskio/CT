using UnityEngine;
using System.Collections;

public class ObservationMessage : MonoBehaviour {

	public string thisMessage;
	bool hasMessagePlayed = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (!hasMessagePlayed) {
				GUIManager.s_instance.SetThoughtText (thisMessage);
				hasMessagePlayed = true;
			}
		}
	}
}
