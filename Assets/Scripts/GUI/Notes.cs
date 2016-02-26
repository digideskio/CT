using UnityEngine;
using System.Collections;

public class Notes : MonoBehaviour {
	public float Yseperation = 70f;
	public float XleftJustified = 160f;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			transform.GetChild(i).localPosition = new Vector3(XleftJustified, -Yseperation* (i+1), 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
