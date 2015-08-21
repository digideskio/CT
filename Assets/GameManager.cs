using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int day=0;
	public float minute=360;
	float minuteDuration = 3f, minuteTimer;
	// Use this for initialization
	public static GameManager s_instance;
	void Start () {
		minuteTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (minute < 1439 && (Time.time - minuteTimer > minuteDuration ) ){
			//change display for minute
			minuteTimer = Time.time;
			minute++;
		}
		else if (minute >= 1400) {
			//change display for minute, hour, and day
			day++;
			minute = 0;
		}
	}
	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	public string ReturnMinute ()
	{
		string tempminute = ((int)(minute % 60)).ToString();
		if (tempminute.Length ==1) {
			tempminute = "0"+tempminute;
		}
		return tempminute;
	}
	
	public string ReturnHour ()
	{
		string temphour = (Mathf.FloorToInt (minute / 60)).ToString();
		if (temphour.Length==1) {
			temphour = "0"+temphour;
		}
		return temphour;
	}
	
	
	
	public void IncrementTime (float minutes)
	{
		if (minute + minutes >= 1440)
			day++;
		minute = ((minute+minutes) % 1440);
	}
}
