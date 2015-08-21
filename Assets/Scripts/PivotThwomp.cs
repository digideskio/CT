using UnityEngine;
using System.Collections;

public class PivotThwomp : MonoBehaviour {
	bool moving = false; 
	
	public Vector3 rotLeft1, rotLeft2, rotRight1, rotRight2;
	
	public Transform leftObj, rightObj;
	
	float timer = 0f;

	public float speed = 2f;
	
	void OnTriggerEnter (Collider col) {
		if(col.tag == "Player") {
			if(!moving) {
				moving = true;

				leftObj.eulerAngles = rotLeft1;
				rightObj.eulerAngles = rotRight1;

				Vector3 t_rotLeft = rotLeft1;
				rotLeft1 = rotLeft2;
				rotLeft2 = t_rotLeft;

				Vector3 t_rotRight = rotRight1;
				rotRight1 = rotRight2;
				rotRight2 = t_rotRight;
			}
		}
	}

	void Update () {
		if(moving) {
			timer += Time.deltaTime / speed;
	
			leftObj.eulerAngles = Vector3.Lerp(rotLeft1, rotLeft2, timer);
			rightObj.eulerAngles = Vector3.Lerp(rotRight1, rotRight2, timer);
			if(timer > 1f) {
				moving = false;
				leftObj.eulerAngles = Vector3.Lerp(rotLeft1, rotLeft2, timer);
				rightObj.eulerAngles = Vector3.Lerp(rotRight1, rotRight2, timer);
				timer = 0f;
			}
		}
	}
}
