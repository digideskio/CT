using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class GUIManager : MonoBehaviour {

	public Slider gasSlider, stamina;
	public GameObject inventoryPanel;
	bool isInventoryOn;
	public Text subtitleText;
	public Text thoughtText;
	public Text gameover;
	public Text timeDisplay;
	public static GUIManager s_instance;

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
		if (subtitleTimer > Time.time - subtitleTimerStart) {
			subtitleText.enabled = true;
		}
		else {
			subtitleText.enabled = false;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		gasSlider.value = PlayerCar.s_instance.currentGas;

		timeDisplay.text = GameManager.s_instance.ReturnHour().ToString() +":"+ GameManager.s_instance.ReturnMinute().ToString();

		if (stamina.value > 0f){
			stamina.value -= .001f;
		}

		InputDevice inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Command.WasPressed) {
			if (isInventoryOn) {
				isInventoryOn = false;

				for (int i = 0; i < inventoryPanel.transform.childCount; i++) {
					inventoryPanel.transform.GetChild (i).gameObject.SetActive(false);
				}

			}
			else if (!isInventoryOn){
				isInventoryOn = true;
				for (int i = 0; i < inventoryPanel.transform.childCount; i++) {
					inventoryPanel.transform.GetChild (i).gameObject.SetActive(true);
				}
			}
		}

		if (thoughtTimer > 0) {
			thoughtText.enabled = true;
			thoughtTimer -= Time.deltaTime;
		} else if (thoughtTimer <= 0) {
			thoughtText.enabled = false;

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

	public void SetThoughtText (string text, float timer = 5f) {
		thoughtTimer = timer;
		thoughtText.text = text;
	}

	public void SetSubtitleText (string text, float timer = 5f) {
		subtitleTimer = timer;
		subtitleText.text = text;
	}
}
