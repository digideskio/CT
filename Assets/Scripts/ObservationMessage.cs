using UnityEngine;
using System.Collections;

public class ObservationMessage : MonoBehaviour {

	public string thisMessage;
	bool hasMessagePlayed = false;
	[SerializeField] int importance;
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (!hasMessagePlayed) {
				GUIManager.s_instance.SetThoughtText (thisMessage);
				GUIManager.s_instance.AddNoteToSelf (new NoteToSelf(thisMessage));
				hasMessagePlayed = true;
			}
		}
	}
}
