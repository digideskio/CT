using UnityEngine;
using System.Collections;

public class LightFadeEx : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GetComponent<Light>().range = Mathf.Lerp (GetComponent<Light>().range, 0, Time.deltaTime);
	}
}
