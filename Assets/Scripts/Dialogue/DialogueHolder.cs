using UnityEngine;
using System.Collections;

public class DialogueHolder : MonoBehaviour {

	public Dialogue[] m_dialogues;
	public int m_dialogueIndex = 0; //how many times player has talked to this character
	public Dialogue ReturnCurrentDialogue () {
		return m_dialogues [m_dialogueIndex];
	}


}
