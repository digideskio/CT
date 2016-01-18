using UnityEngine;
using System.Collections;
using System;
using InControl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : CarMetrics
{
	[SerializeField]
	bool debugCarPosition;
	[SerializeField]
	Transform debugTransform;
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
	public float money;
	float thrustForce = 300000f;
	float currThrust = 0.0f;
	float thrustCoolDown = 1f;
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
		if (debugCarPosition) {
			transform.position = debugTransform.position;
		}
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



		//TODO add in the toggle weapons mode


		//__________________________________________INTERACTION__________________________________________

		if (inputDevice.Action1.WasPressed) {
			PlayerInteractStart();
		}

		if (inputDevice.Action1.WasReleased) {
			PlayerInteractEnd();
		}

		if (inputDevice.Action3.WasPressed) {
			currMovementState = MovementState.Target;
			BeginTarget();
			Camera.main.GetComponent<HoverFollowCam>().thisCameraMode = HoverFollowCam.CameraMode.targetMode;
		}
		if (inputDevice.Action3.WasReleased) {
			currMovementState = MovementState.Drive;
			EndTarget();
			Camera.main.GetComponent<HoverFollowCam>().thisCameraMode = HoverFollowCam.CameraMode.normalMode;

		}

	

		if (inputDevice.Action4.WasPressed) {
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
				if (Mathf.Abs (transform.rotation.eulerAngles.x) > 1f) {
					float negOrPosVal = (transform.rotation.eulerAngles.x < 180) || (transform.rotation.eulerAngles.x < 0) ? -1f : 1f;
					transform.Rotate (negOrPosVal * 0.1f, 0, 0);
				}
				if (Mathf.Abs (transform.rotation.eulerAngles.z) > 1f) {
					float negOrPosVal = (transform.rotation.eulerAngles.z < 180) ? -1f : 1f;
					transform.Rotate (0, 0, negOrPosVal * 0.1f);
				}
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "NPC") {
			other.GetComponent<AIHoverCar> ().isCloseToPlayer = true;
			other.GetComponent<AIHoverCar> ().currentTarget = transform;

		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "NPC") {
			other.GetComponent<AIHoverCar> ().isCloseToPlayer = false;
			other.GetComponent<AIHoverCar> ().currentTarget = null;
		}
	}

	void Thrust() {
		if (!isThrusting) {
			isThrusting = true;
			StartCoroutine ("IsThrusting");
		}
	}

	IEnumerator IsThrusting () {
		body.AddForce (transform.forward * thrustForce);
		GUIManager.s_instance.thrustSlider.value = thrustCoolDown;
		yield return new WaitForSeconds(thrustCoolDown);
		isThrusting = false;
	}

	void StealGas() {
	}

	void OnCollisionEnter (Collision thisCollision) {
		if (thisCollision.collider.gameObject.tag == "NPC" && currMovementState == MovementState.Target) {
			StealGas();
		}
	}

}

