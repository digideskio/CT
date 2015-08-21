using UnityEngine;
using System.Collections;

public class ActivateScript : MonoBehaviour {
	//TODO: Make this modular and not hard coded!
	//USE: Put on a trigger just after a jump and just before the landing track piece. 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<AirRotation>() != null){
			if(other.GetComponent<AirRotation>().enabled){
				other.GetComponent<AirRotation>().enabled = false;
			}
			else{
				other.GetComponent<AirRotation>().enabled = true;
			}
		}
	}
}
