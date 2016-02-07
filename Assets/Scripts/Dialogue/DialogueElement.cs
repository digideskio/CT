using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StoryCharacter {Mechanic, You};

[System.Serializable]
public class DialogueElement
{
	public string dialogueText;
	public StoryCharacter characterName;
	public float dialogueTextDisplayTime;
}