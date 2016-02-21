using UnityEngine;
using System.Collections;

public class ScreenRedFlash : MonoBehaviour {


	public float opacity;

	// Use this for initialization
	void Start () {
	
	}
	public static ScreenRedFlash s_instance;
	void Awake () {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}	
	}

	// Update is called once per frame
	void Update () {
	}
}
