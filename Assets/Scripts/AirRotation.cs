using UnityEngine;
using System.Collections;

public class AirRotation : MonoBehaviour {
	//casts rays downward. makes sure the car is parallel with a hidden curve under the car when launched in the air
	//place on a car and deactivate. Use with "ActivateScript" script. 
	public Transform backLeft;
	public Transform backRight;
	public Transform frontLeft;
	public Transform frontRight;
	public RaycastHit lr;
	public RaycastHit rr;
	public RaycastHit lf;
	public RaycastHit rf;
	public RaycastHit hit;
	
	public Vector3 upDir;
	
	void Update () {
//		Origional
//		Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
//		Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
//		Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
//		Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);
//		
//		upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
//		         Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
//		         Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
//		         Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
//		         ).normalized;
//		Debug.DrawRay(rr.point, Vector3.up);
//		Debug.DrawRay(lr.point, Vector3.up);
//		Debug.DrawRay(lf.point, Vector3.up);
//		Debug.DrawRay(rf.point, Vector3.up);

//		Physics.Raycast(backLeft.position + backLeft.up, -backLeft.up, out lr);
//		Physics.Raycast(backRight.position + backRight.up, -backRight.up, out rr);
//		Physics.Raycast(frontLeft.position + frontLeft.up, -frontLeft.up, out lf);
//		Physics.Raycast(frontRight.position + frontRight.up, -frontRight.up, out rf);

		
//		upDir = (Vector3.Cross(rr.point - transform.up, lr.point - transform.up) +
//		         Vector3.Cross(lr.point - transform.up, lf.point - transform.up) +
//		         Vector3.Cross(lf.point - transform.up, rf.point - transform.up) +
//		         Vector3.Cross(rf.point - transform.up, rr.point - transform.up)
//		         ).normalized;




//		Debug.DrawRay(rr.point, Vector3.up);	
//		Debug.DrawRay(lr.point, Vector3.up);
//		Debug.DrawRay(lf.point, Vector3.up);
//		Debug.DrawRay(rf.point, Vector3.up);

		
	//	gameObject.transform.


	//	gameObject.transform.Rotate(
	}

	void FixedUpdate(){
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, 1000f)){
			upDir = -hit.normal;
			Debug.DrawRay(transform.position, -Vector3.up * 1000f, Color.cyan);
			Vector3 temp = transform.forward;
			//transform.up = (transform.up * 10f + upDir)/11f;	
			this.gameObject.GetComponent<Rigidbody>().AddForce(Physics.gravity * 20f, ForceMode.Acceleration);
			transform.up = upDir;
			transform.forward = temp;
		}
	}
}
