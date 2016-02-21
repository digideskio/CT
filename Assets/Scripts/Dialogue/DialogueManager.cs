using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : InteractableObject 
{
	// Input ID for input
	[HideInInspector]
	private int currentDialogueIndex = 0;
	private Dialogue currentDialogue;

	Text dialogueText;
	private bool isDialogueRunning, isTargeted;

	void Start () {
		dialogueText = GameObject.FindGameObjectWithTag ("Subtitle").GetComponent<Text>();
	}
	public void initializeDialogueGUI () {
		isDialogueRunning = true;
		currentDialogue = GetComponent<DialogueHolder> ().ReturnCurrentDialogue ();
		currentDialogueIndex = 0;
		displayCurrentDialogueElement();
		GUIManager.s_instance.isInDialogue = true;

	}

	private void displayCurrentDialogueElement() {
		dialogueText.text = "\"" + currentDialogue.DialogItems[currentDialogueIndex].dialogueText +  "\"";
		if (currentDialogue.DialogItems [currentDialogueIndex].characterName == StoryCharacter.You) {
			print ("COLOR SET");
			dialogueText.color = new Color (155, 216, 138);
		} else {
			dialogueText.color = Color.white;
		}
	}

	private void displayNextDialogueElement() {
		if (currentDialogue.DialogItems.Count > currentDialogueIndex + 1) {
			currentDialogueIndex++;
			displayCurrentDialogueElement();
		}
		else {
			endDialogueSequence();
		}
	}

	private void endDialogueSequence() {
		GetComponent<DialogueHolder> ().m_dialogueIndex++;
		dialogueText.text = "";
		print ("EndDialogueSeq");
		GUIManager.s_instance.isInDialogue = false;

	}

	private void InterruptDialogueSequence () {
		isDialogueRunning = false;
		//TODO current car gets mad that you interrupted convo
	}

	public override void ReceiveInteract () {
		if (isTargeted && !isDialogueRunning) {
			initializeDialogueGUI ();
		}

		else if (isDialogueRunning) {
			displayNextDialogueElement ();
			print ("DISPLAYED NEXT ELEMENT");
		}
	}
	public override void ReceiveTarget () {
		isTargeted = true;
	}
	public override void ReceiveUntarget () {
		isTargeted = false;
	}

	void Update () {
//		if (!isTargeted && isDialogueRunning) {
//			InterruptDialogueSequence ();
//		}
	}
}



