using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	public enum TurretStates {Inactive, LookingAtPlayer, ShootingAtPlayer};
	public TurretStates currState = TurretStates.Inactive;
	public bool killPlayer, lookAtPlayer;
	[SerializeField] float LOS = 100f;
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
			TurretFaceTarget (PlayerCar.s_instance.transform.position);
			break;
		}
	}
		
	public void TurretFaceTarget(Vector3 target) {
		if (Mathf.Abs(transform.rotation.eulerAngles.x) < 30f) {
			transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, target - transform.position, .015f, .4f), Vector3.up);

		} else { //just rotate around y
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x, 
				Quaternion.LookRotation (Vector3.RotateTowards (transform.forward, target - transform.position, .015f, .4f), Vector3.up).eulerAngles.y,
				transform.rotation.eulerAngles.z));
		}
	}


}
