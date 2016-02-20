using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	public enum TurretStates {Inactive, LookingAtPlayer, ShootingAtPlayer};
	public TurretStates currState = TurretStates.Inactive;
	[SerializeField] GameObject bullet;
	public bool killPlayer, lookAtPlayer;
	[SerializeField] float LOS = 100f;
	[SerializeField] Transform exitPoint1, exitPoint2;
	float timeBetweenEachShot = .3f, shotTimer = 0;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		switch (currState) {
		case TurretStates.Inactive: 
			if (Vector3.Distance(PlayerCar.s_instance.transform.position, gameObject.transform.position) < LOS) {
				currState = TurretStates.LookingAtPlayer;
			}

				break;
		case TurretStates.LookingAtPlayer:
			if (Vector3.Distance (PlayerCar.s_instance.transform.position, gameObject.transform.position) > LOS) {
				currState = TurretStates.Inactive;
			}
			TurretFaceTarget (new Vector3(PlayerCar.s_instance.transform.position.x,PlayerCar.s_instance.transform.position.y+.5f,PlayerCar.s_instance.transform.position.z));
			if (killPlayer) {
				ShootAtPlayer ();
			}
			break;
		}
	}
		
	public void TurretFaceTarget(Vector3 target) {
		if (Vector3.Angle (new Vector3 (target.x - transform.position.x, transform.position.y, target.z - transform.position.z), target - transform.position) < 60f) {
			transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, target - transform.position, .015f, .4f), Vector3.up);

		} else { //just rotate around y
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x, 
				Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, target - transform.position, .015f, .4f), Vector3.up).eulerAngles.y,
				transform.rotation.eulerAngles.z));
		}
		Debug.DrawRay (transform.position, transform.forward, Color.cyan);
	}

	void ShootAtPlayer() {
		shotTimer += Time.deltaTime;
		if (shotTimer > timeBetweenEachShot) {

			Instantiate (bullet, exitPoint1.position, transform.rotation);
			Instantiate (bullet, exitPoint2.position, transform.rotation);
			shotTimer = 0;
		}
	}

	public void TriggerFightState () {
		killPlayer = true;
	}


}
