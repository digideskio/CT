using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum storyCharacter {
	Ulfrik,
	Brunhilde,
	Hel,
	Batking,
	None
}


[System.Serializable]
public class DialogueElement
{
	public string dialogueText;
	public storyCharacter characterName;
	public float dialogueTextDisplayTime;
}