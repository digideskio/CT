using UnityEngine;
using System.Collections;
using System;
using InControl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : CarMetrics
{
	public static PlayerCar s_instance;
	public GameObject[] m_hoverPoints;
	Rigidbody body;
	float deadZone = 0.1f;
	public float hoverForce = 3000.0f;
	public float hoverHeight = 1.0f;
	public float forwardAcl = 30000.0f;
	public float backwardAcl = 15000.0f;
	public float strafeAcl = 5000.0f;
	public float turnStrength = 5000f;
	float currThrust = 0.0f;
	float currTurn = 0.0f;
	float currStrafe;
	int layerMask;
	bool isTouchingGround;
	public float idleGasLossRate, movingGasLossRate, currentGasLossRate;
	bool isAccelerating = false;
	bool isMovementDisabled = false;


	//gameover



	public Transform gameoverCam;
	bool lerpGameOverSwitch;
	float gameOverCamLerpTimer;
	float gameOverCamLerpDuration = 10f;
	Vector3 lerpStartPosition;
	public Transform bulldozeChildTransform;
	public Transform cameraEngagePosition;

	public delegate void Engage();
	public static event Engage PlayerEngage;
	
	public delegate void Disengage();
	public static event Disengage PlayerDisengage;

	public delegate void InteractEvent();
	public static event InteractEvent PlayerInteractStart;

	public delegate void DeinteractEvent();
	public static event DeinteractEvent PlayerInteractEnd;

	//fuck mechanics
	public float fuckForce = 200000f;
	public float pullBackForce = 500f;
	float fuckCharge = 0f, lastFuckCharge, fuckChargeAbsorbRatio = 300f, staminaLossPerFuckRatio = 10000f;
	public GameObject PheremoneCharge;
	float staminaLossRate = .001f;

	public enum MovementState {Drive, Engage, Submit, OutOfGas};
	public MovementState currMovementState = MovementState.Drive;
	public bool isCharged, isThrusting;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	void Start ()
	{
		body = GetComponent<Rigidbody> ();
		
		layerMask = 1 << LayerMask.NameToLayer ("Characters");
		layerMask = ~layerMask;
	}
	
	void Update ()
	{
		
		InputDevice inputDevice = InputManager.ActiveDevice;


		if (currentGas < 0.5f) {
			currMovementState = MovementState.OutOfGas;
			Camera.main.GetComponent<HoverFollowCam>().enabled = false;
		}
		// Main Thrust
		currThrust = 0.0f;
		isAccelerating = false;
		float aclAxis = 0;
		if (inputDevice.LeftStickUp != 0) {
			aclAxis = inputDevice.LeftStickUp;
			isAccelerating = true;
		} else if (inputDevice.LeftStickDown != 0) {
			aclAxis = -inputDevice.LeftStickDown;
			isAccelerating = true;
		}
		if (aclAxis > deadZone)
			currThrust = aclAxis * forwardAcl;
		else if (aclAxis < -deadZone)
			currThrust = aclAxis * backwardAcl;

		if (isAccelerating) {
			currentGasLossRate = movingGasLossRate;
		}
		else {
			currentGasLossRate = idleGasLossRate;
		}

		currentGas-=currentGasLossRate;

		// Turning
		float turnAxis = 0.0f;
		if (inputDevice.RightStickRight != 0) {
			turnAxis = inputDevice.RightStickRight;
		} else if (inputDevice.RightStickLeft != 0) {
			turnAxis = -inputDevice.RightStickLeft;
		}
		currTurn = turnAxis;
		
		// Strafe
		float strafeAxis = 0;
		if (inputDevice.LeftStickLeft != 0) {
			strafeAxis = -inputDevice.LeftStickLeft;
		} else if (inputDevice.LeftStickRight != 0) {
			strafeAxis = inputDevice.LeftStickRight;
		}
		currStrafe = strafeAxis * strafeAcl;






		//__________________________________________INTERACTION__________________________________________
		if (inputDevice.Action1.WasPressed) {
			PlayerInteractStart();
		}

		if (inputDevice.Action1.WasReleased) {
			PlayerInteractEnd();
		}

		if (inputDevice.Action3.WasPressed) {
			currMovementState = MovementState.Engage;
			PlayerEngage();
			Camera.main.GetComponent<HoverFollowCam>().thisCameraMode = HoverFollowCam.CameraMode.engagedMode;
		}
		if (inputDevice.Action3.WasReleased) {
			currMovementState = MovementState.Drive;
			PlayerDisengage();
			Camera.main.GetComponent<HoverFollowCam>().thisCameraMode = HoverFollowCam.CameraMode.normalMode;

		}

		if (inputDevice.Action4.WasPressed && currMovementState == MovementState.Engage) {
			isCharged = true;
		}

		if (inputDevice.Action4.WasReleased && isCharged == true) {
			isThrusting = true;
			lastFuckCharge = fuckCharge;
			currentStamina-=lastFuckCharge/staminaLossPerFuckRatio;
			fuckCharge = 0f;
			Thrust ();
		}

		if (currMovementState == MovementState.OutOfGas) {
			isMovementDisabled = true;
			if (!lerpGameOverSwitch) {
				StartGameOverCameraLerp();
			}
			float timeElapsed = Time.time - gameOverCamLerpTimer;
			float percentage = timeElapsed/gameOverCamLerpDuration;
			Camera.main.transform.position = Vector3.Lerp(lerpStartPosition, gameoverCam.position, percentage);
			Camera.main.transform.LookAt(transform.position);
			//lerp gameover camera
			 
		}

	}
	void StartGameOverCameraLerp () {
		lerpStartPosition = Camera.main.transform.position;
		gameOverCamLerpTimer = Time.time;
		lerpGameOverSwitch = true;
	}
	void FixedUpdate ()
	{
		RaycastHit hit;
		for (int i = 0; i < m_hoverPoints.Length; i++) {
			var hoverPoint = m_hoverPoints [i];
			if (Physics.Raycast (hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask)) {
				body.AddForceAtPosition (Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
				isTouchingGround = true;
			} else {
				isTouchingGround = false;
				
				if (transform.position.y > hoverPoint.transform.position.y) {
					body.AddForceAtPosition (hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				} else {
					body.AddForceAtPosition (hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
				}
			}
		}
		// Turn
		if (currTurn != 0 && !isMovementDisabled) {
			body.AddRelativeTorque (Vector3.up * currTurn * turnStrength);
		}


		if (currStrafe != 0 && !isMovementDisabled) {
			body.AddForce (transform.right * currStrafe);
		}

		if (currMovementState == MovementState.Drive) {
			// Forward
			if (Mathf.Abs (currThrust) > 0 && isTouchingGround) {
				body.AddForce (transform.forward * currThrust);
			} else {
				body.AddForce (transform.forward * currThrust);
				//correct rotation
				if (Mathf.Abs (transform.rotation.eulerAngles.x) > 1f) { //need to check for 360 instead of negative
					float negOrPosVal = (transform.rotation.eulerAngles.x < 180) ? -1f : 1f;
					transform.Rotate (negOrPosVal * 0.1f, 0, 0);
				}
				if (Mathf.Abs (transform.rotation.eulerAngles.z) > 1f) {
					float negOrPosVal = (transform.rotation.eulerAngles.z < 180) ? 1f : -1f;
					transform.Rotate (0, 0, -negOrPosVal * 0.1f);
				}
			}
		}

		//______________________________________INTERCOURSE PHYSICS______________________________________

		else if (currMovementState == MovementState.Engage) {
			currentStamina-=staminaLossRate;
			if (currentStamina < 0.005) {
				currMovementState = MovementState.Drive;
				//call event to end fuckstate for other cars
			}
			if (isCharged) {
				PullBack();
				fuckCharge+=10f;
			}
		}

		
		}
	void PullBack () {
		body.AddForce (-transform.forward * pullBackForce);

	}

	void Thrust() {
		isThrusting = false;
		isCharged = false;
		body.AddForce (transform.forward * fuckForce);
	}

	void StealGas() {
		//if (make contact with car after thrusting, car spits out gas and add lastFuckCharge
		currentGas+=lastFuckCharge/fuckChargeAbsorbRatio;
		print ("gained gas " +lastFuckCharge/fuckChargeAbsorbRatio);
	}

	void OnCollisionEnter (Collision thisCollision) {
		if (thisCollision.collider.gameObject.tag == "NPC" && currMovementState == MovementState.Engage) {
			StealGas();
		}
	}

}

