using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour 
{
	// Input ID for input
	[HideInInspector]
	private int currentDialogueIndex = 0;
	private Dialogue currentDialogue;

	public GameObject dialogueLayout;
	public Text characterName, dialogueText;
//	public Image portrait;

	// Must be referenced by index i.e. ulfrik = 0, brunhilde = 1
//	public List<Sprite> characterPortraits;

	public void initializeDialogueGUI (Dialogue newDialogue) {
		currentDialogue = newDialogue;
		dialogueLayout.SetActive(true);
		currentDialogueIndex = 0;
		displayCurrentDialogueElement();
	}

	private void displayCurrentDialogueElement() {
		dialogueText.text = currentDialogue.DialogItems[currentDialogueIndex].dialogueText;
		setCharacterNameAndPortrait(currentDialogue.DialogItems[currentDialogueIndex].characterName);

		//TODO:
		//Implement a way to play VO audio
	}

	private void displayNextDialogueElement() {
		if(currentDialogue.DialogItems.Count > currentDialogueIndex) {
			currentDialogueIndex++;
			displayCurrentDialogueElement();
		}
		else {
			endDialogueSequence();
		}
	}

	private void endDialogueSequence() {
		dialogueLayout.SetActive(false);
	}

	private void setCharacterNameAndPortrait(StoryCharacter characterEnum) {
		
	}

	void Update () {
//		if (InputManager.Devices [playerInputID].Action1.WasPressed == true && dialogueLayout.activeSelf) {
//			displayNextDialogueElement();
//		}
	}
}












	/* 
	 * JOHN'S OLD CODE
	private GameManager gameM;
	private List<Transform> dialogueObjects;
	private float dialogueEndTimer = 0.0f;
	private float dialogueEndDelay = 0.0f;
	private DialogueController currentDialogueC;
	public float fadeDelay = 1.0f;
	private float fadeTimer = 0.0f;
	private bool dialogueActive = false;
	private bool dialogueFadeout = false;
	private float portraitFFT = 1.0f;
	public Font dialogueFont;
	public Texture2D dialogueBackground;




	void Start () 
	{
		gameM = GameObject.Find("_GameManager").GetComponent<GameManager>();
		dialogueObjects = new List<Transform>();
		
		// take inventory of dialogue child objects
		foreach (Transform child in transform)
		{		
			if(child.GetComponent<DialogueController>() != null)
				dialogueObjects.Add(child);
		}
				
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.U) == true && gameM.debugBuild == true)
		{
			PlayDialogue("temp_themost");	
		}
		
		if(dialogueActive == true)
		{
			if(fadeTimer < fadeDelay)
			{
				fadeTimer += Time.deltaTime;
			}
			else
			{
				dialogueEndTimer += Time.deltaTime;
				if(dialogueEndTimer > dialogueEndDelay)
				{
					// start fading out 	
					dialogueFadeout = true;
					dialogueActive = false;
				}

				if(audio.isPlaying == false)
				{
					audio.clip = currentDialogueC.dialogueVO;
					audio.Play();
					print ("PLAYING AUDIO");
				}				
			}
			
			float[] data = new float[1024];
			GetComponent<AudioSource>().GetSpectrumData(data, 0, FFTWindow.BlackmanHarris); //, FFTWindow.Rectangular);
			
			portraitFFT = 0.0f;
			
			for(int x = 0; x < data.Length; x++)
			{
				portraitFFT += System.Math.Abs(data[x]);
			}
			
			portraitFFT *= 0.0285f;	
		}
		
		if(dialogueFadeout == true)
		{
			if(fadeTimer > 0.0f)
			{
				fadeTimer -= Time.deltaTime;
			}
			else
			{
				dialogueFadeout = false;
				dialogueEndDelay = 0.0f;
			}
		}
		
		
	}
	
	public void PlayDialogue(string name)
	{
		foreach(Transform dialogue in dialogueObjects)
		{
			if(dialogue.name == name)
			{
				currentDialogueC = dialogue.GetComponent<DialogueController>();
				dialogueEndDelay = currentDialogueC.dialogueVO.length - 0.05f;
				fadeTimer = 0.0f;
				dialogueEndTimer = 0.0f;
				dialogueActive = true;
				print(dialogue.name);
			}
		}
	}
	
	public void OnGUI()
	{
		float widthScale = ((float)Screen.width / 1280.0f);
		GUIStyle dialogueStyle = new GUIStyle();
		dialogueStyle.fontSize = (int)(widthScale * 15.0f);
		dialogueStyle.normal.textColor = Color.white;	
		dialogueStyle.font = dialogueFont;			
		GUIStyle dialogueNameStyle = new GUIStyle();
		dialogueNameStyle.fontSize = (int)(widthScale * 20.0f);
		dialogueNameStyle.normal.textColor = new Color(0.1f, 0.3f, 0.9f);	
		dialogueNameStyle.font = dialogueFont;			
		
		if(dialogueActive == true || dialogueFadeout == true)
		{	
			GUI.color = new Color(1.0f, 1.0f, 1.0f, (fadeTimer / fadeDelay));
			
			GUI.DrawTexture(screenRect(0.0f, 0.0f, 1.0f, 1.0f), dialogueBackground);
			
			GUI.DrawTexture(screenRect(0.633f-(portraitFFT*0.5f), 0.1333f-(portraitFFT*0.5f), 0.215f+portraitFFT, 0.302f+portraitFFT), currentDialogueC.portait);
			
			string dialogueContents = "";
			string speakersName = currentDialogueC.speakersName+":";
			
			for(int i = 0; i < currentDialogueC.dialogueTextDisplayTime.Count; i++)
			{
				if(dialogueEndTimer > currentDialogueC.dialogueTextDisplayTime[i])
				{
					dialogueContents = currentDialogueC.dialogueText[i];
				}
			}			
			
			GUI.Label(screenRect(0.266f, 0.2f, 1.0f, 1.0f), speakersName, dialogueNameStyle);
			GUI.Label(screenRect(0.266f, 0.255f, 1.0f, 1.0f), dialogueContents, dialogueStyle);
		}
	}
				
    public Rect screenRect(float tx, float ty, float tw, float th) 
    {
        float x1 = tx * Screen.width;
        float y1 = ty * Screen.height;
        float sw = tw * Screen.width;
        float sh = th * Screen.height;
        return new Rect(x1,y1,sw,sh);
    }
    */
