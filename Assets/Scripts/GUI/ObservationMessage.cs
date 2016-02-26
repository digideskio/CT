using UnityEngine;
using System.Collections;

public class ObservationMessage : MonoBehaviour {

	public string thisMessage;
	public string[] seriesOfMessages;
	bool hasMessagePlayed = false;
	[SerializeField] bool displayThought = true;
	[SerializeField] int importance;
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (!hasMessagePlayed) {
				if (displayThought) {
					GUIManager.s_instance.SetThoughtText (thisMessage);
				}
				if (importance > 0) {
					GUIManager.s_instance.AddNoteToSelf (new NoteToSelf (thisMessage));
				}
				hasMessagePlayed = true;
			}
		}
	}
}
