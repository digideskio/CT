using UnityEngine;
using System.Collections;

public class TestControls : MonoBehaviour {

	public float speed = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		transform.Translate (new Vector3 (0,0,vert * speed * Time.deltaTime));
		transform.Rotate(new Vector3(0,hori*Time.deltaTime*speed,0));
		                        
	}
}
