using UnityEngine;
using System.Collections;

public class CarAnimationController : MonoBehaviour {

	public Animator thisCarAnimator;
	public float timeParameter = 0;
	public float turnRate = .1f;
	public float recoveryRate = .5f;
	public float wheelRotation = 0;
	public float pedalValue = 0;
	float lastWheelRotation;

	void Start () {
		thisCarAnimator.SetTrigger("IsTakingOff");
		thisCarAnimator = gameObject.GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		wheelRotation += (Input.GetAxis("Horizontal") * 0.1f);
		wheelRotation = Mathf.Clamp (wheelRotation, -1f, 1f);
		pedalValue = Input.GetAxis ("Vertical");
//
//		if (turnRadius != 0) {
//			timeParameter += Time.deltaTime;
//			turnRadius = Mathf.Lerp(turnRadius, 0, Time.deltaTime);			
//		}
//		if (Input.GetAxis("Horizontal") > 0 && turnRadius < 1) {
//			timeParameter = 0;
//			if (turnRadius < 0)
//				turnRadius += turnRate;
//			turnRadius += turnRate;
//
//		}
//		if (Input.GetAxis("Horizontal") < 0 && turnRadius < 1){
//			timeParameter = 0;
//			if (turnRadius > 0)
//				turnRadius -= turnRate;
//			turnRadius -= turnRate;
//		}
	
		thisCarAnimator.SetFloat("Speed", pedalValue);
		thisCarAnimator.SetFloat("Direction", wheelRotation);


	}
}
