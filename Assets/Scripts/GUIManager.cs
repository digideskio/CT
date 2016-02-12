using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;

public class NoteToSelf {
	public string noteContents;
	public int numberOfTimesViewed; //used to determine when 
	public int importance;
	public string title; //for deletion sake
	public NoteToSelf(string newNoteContents, int newImportance = 5) {
		noteContents = newNoteContents;
		importance = newImportance;
	}
}

public class GUIManager : MonoBehaviour {

	//TODO implement stamina logic
	public Slider gasSlider, thrustSlider;
	public GameObject inventoryPanel;
	bool isInventoryOn = true;
	[SerializeField]
	Text subtitleText, thoughtText, gameover, timeDisplay, notes, money;
	public static GUIManager s_instance;

	public List<NoteToSelf> notesToSelf = new List<NoteToSelf> ();

	bool thoughtTimerSwitch = false;
	bool dialogueTimerSwitch = false;
	float thoughtDuration, thoughtTimer;
	float dialogueDuration, dialogueTimer;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	
	float subtitleTimer = 0, subtitleTimerStart;
	// Use this for initialization
	void Start () {
		EnableInventory ();
//		if (subtitleTimer > Time.time - subtitleTimerStart) {
//			subtitleText.enabled = true;
//		}
//		else {
//			subtitleText.enabled = false;
//		}
//			
	}
	
	// Update is called once per frame
	void Update () {
		gasSlider.value = PlayerCar.s_instance.currentGas;
		money.text = "Money: $" + PlayerCar.s_instance.money.ToString("F2");
		timeDisplay.text = GameManager.s_instance.ReturnHour().ToString() +":"+ GameManager.s_instance.ReturnMinute().ToString();

		if (thrustSlider.value > 0f){
			thrustSlider.value -= Time.deltaTime;
		}

		InputDevice inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Command.WasPressed) {
			if (!isInventoryOn) {
				EnableInventory ();

			}
			else if (isInventoryOn){
				DisableInventory ();
			}
		}

		if (thoughtTimer > 0) {
			thoughtTimer -= Time.deltaTime;
		} else if (thoughtTimer <= 0 && isDisplayingThought) {
			isDisplayingThought = false;
			thoughtText.gameObject.GetComponent<Fader> ().StartFadeOut ();

		}

		if (subtitleTimer > 0) {
			subtitleText.enabled = true;
			subtitleTimer -= Time.deltaTime;
		} else if (subtitleTimer <= 0) {
			subtitleText.enabled = false;
		}

		if (PlayerCar.s_instance.currMovementState == PlayerCar.MovementState.OutOfGas) {
			gameover.gameObject.SetActive(true);
		}
	}
	bool isDisplayingThought = false;
	public void SetThoughtText (string text, float timer = 3.5f) {
		thoughtTimer = timer;
		thoughtText.text = text;
		thoughtText.gameObject.GetComponent<Fader> ().StartFadeIn ();
		isDisplayingThought = true;

	}

	public void SetSubtitleText (string text, float timer = 3f) {
		subtitleTimer = timer;
		subtitleText.text = text;
	}

	public void AddNoteToSelf(NoteToSelf thisNote) {
		notesToSelf.Add (thisNote);
	}

	void EnableInventory () {
		Time.timeScale = 0;
		isInventoryOn = true;
		for (int i = 0; i < inventoryPanel.transform.childCount; i++) {
			inventoryPanel.transform.GetChild (i).gameObject.SetActive(true);
		}
		DisplayNotesToSelf ();

	}

	void DisableInventory () {
		Time.timeScale = 1f;

		isInventoryOn = false;
		for (int i = 0; i < inventoryPanel.transform.childCount; i++) {
			inventoryPanel.transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	void DisplayNotesToSelf () {
		string allNotesToSelf = "Notes to Self:\n";
		foreach (NoteToSelf x in notesToSelf) {
			if (x.numberOfTimesViewed >= x.importance) {
				notesToSelf.Remove (x);
			} else {
				x.numberOfTimesViewed++;
				allNotesToSelf += x.noteContents + "\n";
			}
		}
		notes.text = allNotesToSelf;
	}
}
