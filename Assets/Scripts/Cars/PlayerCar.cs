   using UnityEngine;
using System.Collections;
using System;
using InControl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : CarMetrics
{
	#region variables
	[SerializeField]
	bool debugCarPosition, AIdebug;
	[SerializeField]
	Transform debugTransform;
	[SerializeField]
	GameObject debugAICam;
	public static PlayerCar s_instance;

	float aclAxis = 0;
	public float forwardAcl = 30000.0f;
	public float backwardAcl = 15000.0f;
	public float strafeAcl = 5000.0f;
	public float turnStrength = 5000f;
	public float money;
	float thrustForce = 300000f;
	float currThrust = 0.0f;
	float thrustCoolDown = 1f;
	float currTurn = 0.0f;
	float currStrafe;
	public float idleGasLossRate, movingGasLossRate, currentGasLossRate;
	bool isAccelerating = false;
	bool isMovementDisabled = false;
	bool isTargeting;
	public Transform currentTarget;

	public Transform gameoverCam;
	bool lerpGameOverSwitch;
	float gameOverCamLerpTimer;
	float gameOverCamLerpDuration = 10f;
	Vector3 lerpStartPosition;
	public Transform bulldozeChildTransform;
	public Transform cameraEngagePosition;

	public delegate void Target();
	public static event Target BeginTarget;
	
	public delegate void Untarget();
	public static event Untarget EndTarget;

	public delegate void InteractEvent();
	public static event InteractEvent PlayerInteractStart;

	public delegate void DeinteractEvent();
	public static event DeinteractEvent PlayerInteractEnd;

	// mechanics
	float staminaLossRate = .001f;

	public enum MovementState {Drive, Target, Submit, OutOfGas};
	public MovementState currMovementState = MovementState.Drive;
	public bool isCharged, isBoosting;

	InputDevice inputDevice;


	#endregion
	void Awake() {

		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	new void Start ()
	{
		base.Start ();
		if (AIdebug) {
			Camera.main.gameObject.SetActive (false);
			debugAICam.SetActive (true);
		}
		else if (debugCarPosition) {
			transform.position = debugTransform.position;
		}

	}
	
	void Update ()
	{
		if (!AIdebug) {
		
			inputDevice = InputManager.ActiveDevice;

			if (currentGas < 0.5f) {
				
			}

			HandleGasLoss ();
			HandleLeftStickVertical ();
			HandleLeftStickHorizontal ();
			HandleRightStickHorizontal ();


			//__________________________________________INTERACTION__________________________________________

			if (inputDevice.Action1.WasPressed) {
				PlayerInteractStart ();
			}

			if (inputDevice.Action1.WasReleased) {
				PlayerInteractEnd ();
			}

			if (inputDevice.LeftTrigger.WasPressed) {
				LockOnToTarget ();
			}
			if (inputDevice.LeftTrigger.WasReleased) {
				UnlockFromTarget ();
			}

			if (inputDevice.RightTrigger.WasPressed) {
				AfterburnerBoost ();
			}

			if (currMovementState == MovementState.OutOfGas) {
				isMovementDisabled = true;
				if (!lerpGameOverSwitch) {
					StartGameOverCameraLerp ();
				}
				float timeElapsed = Time.time - gameOverCamLerpTimer;
				float percentage = timeElapsed / gameOverCamLerpDuration;
				Camera.main.transform.position = Vector3.Lerp (lerpStartPosition, gameoverCam.position, percentage);
				Camera.main.transform.LookAt (transform.position);
			}
		}

	}

	void UnlockFromTarget() {
		currMovementState = MovementState.Drive;
		EndTarget ();
		isTargeting = false;
		Camera.main.GetComponent<HoverFollowCam> ().thisCameraMode = HoverFollowCam.CameraMode.normalMode;
	}

	void LockOnToTarget () {
		currMovementState = MovementState.Target;
		BeginTarget ();
		isTargeting = true;
		Camera.main.GetComponent<HoverFollowCam> ().thisCameraMode = HoverFollowCam.CameraMode.targetMode;
	}

	void AfterburnerBoost() {
		if (!isBoosting) {
			isBoosting = true;
			StartCoroutine ("IsBoosting");
		}
	}

	void HandleLeftStickVertical () {
		//Car Acceleration
		currThrust = 0.0f;
		isAccelerating = false;
		aclAxis = 0;
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
	}

	void HandleRightStickHorizontal () {
		// Turning
		float turnAxis = 0.0f;
		if (inputDevice.RightStickRight != 0) {
			turnAxis = inputDevice.RightStickRight;
		} else if (inputDevice.RightStickLeft != 0) {
			turnAxis = -inputDevice.RightStickLeft;
		}
		currTurn = turnAxis;
	}

	void HandleLeftStickHorizontal () {
		// Strafe
		float strafeAxis = 0;
		if (inputDevice.LeftStickLeft != 0) {
			strafeAxis = -inputDevice.LeftStickLeft;
		} else if (inputDevice.LeftStickRight != 0) {
			strafeAxis = inputDevice.LeftStickRight;
		}
		currStrafe = strafeAxis * strafeAcl;
	}


	void StartGameOverCameraLerp () {
		lerpStartPosition = Camera.main.transform.position;
		gameOverCamLerpTimer = Time.time;
		lerpGameOverSwitch = true;
	}
	new void FixedUpdate ()
	{
		base.FixedUpdate ();
		// Turn
		if (currTurn != 0 && !isMovementDisabled) {
			m_body.AddRelativeTorque (Vector3.up * currTurn * turnStrength);
		}


		if (currStrafe != 0 && !isMovementDisabled) {
			m_body.AddForce (transform.right * currStrafe);
		}

		if (isTargeting && currentTarget!=null) {
			FaceTarget (currentTarget.position);
		}

		if (currMovementState == MovementState.Drive) {
			// Forward
			if (Mathf.Abs (currThrust) > 0 && isTouchingGround) {
				m_body.AddForce (transform.forward * currThrust);
			} else {
				m_body.AddForce (transform.forward * currThrust);
				//correct rotation
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "NPC") {
			other.GetComponent<AIHoverCar> ().isCloseToPlayer = true;
			other.GetComponent<AIHoverCar> ().currentTarget = transform;
			currentTarget = other.transform;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "NPC") {
			other.GetComponent<AIHoverCar> ().isCloseToPlayer = false;
			other.GetComponent<AIHoverCar> ().currentTarget = null;
			currentTarget = null;
		}
	}

	void HandleGasLoss () {
		if (isAccelerating) {
			currentGasLossRate = movingGasLossRate;
		} else {
			currentGasLossRate = idleGasLossRate;
		}
		currentGas -= currentGasLossRate;
	}

	IEnumerator IsBoosting () {
		m_body.AddForce (transform.forward * thrustForce);
		GUIManager.s_instance.thrustSlider.value = thrustCoolDown;
		yield return new WaitForSeconds(thrustCoolDown);
		isBoosting = false;
	}

	void RunOutOfGas () {
		currMovementState = MovementState.OutOfGas;
		Camera.main.GetComponent<HoverFollowCam> ().enabled = false;
	}

	void StealGas() {
		
	}

	void OnCollisionEnter (Collision thisCollision) {
//		if (thisCollision.collider.gameObject.tag == "NPC" && currMovementState == MovementState.Target) {
//			StealGas();
//		}
	}

}

