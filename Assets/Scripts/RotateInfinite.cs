using UnityEngine;
using System.Collections;

public class RotateInfinite : MonoBehaviour {

	public float rotateSpeed = 3f;
	public string Axis;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(Axis){
		case "x" : transform.Rotate (rotateSpeed, 0f, 0f); break;
		case "y" : transform.Rotate (0f, rotateSpeed, 0f); break;
		case "z" : transform.Rotate (0f, 0f, rotateSpeed); break;
		}
	}
}
