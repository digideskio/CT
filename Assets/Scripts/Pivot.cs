using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

//	public bool moving = true; 

//	Vector3 startRotLeft, startRotRight;
//	public Vector3 targetRotLeft, targetRotRight;

//	public Transform leftObj, rightObj;

//	float timer = 0f;

	void OnTriggerEnter (Collider col) {
		if(col.tag == "Player") {
			GetComponent<Animator>().SetTrigger("isTriggered");
//			if(!moving) {
//				moving = true;

//				startRotLeft = leftObj.eulerAngles;
//				startRotRight = rightObj.eulerAngles;

//				Vector3 t_tarRotLeft = targetRotLeft;
//				targetRotLeft = targetRotRight;
//				targetRotRight = t_tarRotLeft;
//			}
		}
	}
	
	// Use this for initialization
//	void Start () {
	
//	}
	
	void Update () {
//		if(moving) {
//			timer += Time.deltaTime;
//
//			leftObj.eulerAngles = Vector3.Slerp(startRotLeft, targetRotLeft, timer);
//			rightObj.eulerAngles = Vector3.Slerp(startRotRight, targetRotRight, timer);
//			if(timer > 1f) {
//				moving = false;
//				leftObj.eulerAngles = Vector3.Slerp(startRotLeft, targetRotLeft, timer);
//				rightObj.eulerAngles = Vector3.Slerp(startRotRight, targetRotRight, timer);
//				timer = 0f;
//			}
//		}
	}
}
