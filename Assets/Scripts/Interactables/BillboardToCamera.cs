using UnityEngine;
using System.Collections;

public class BillboardToCamera : MonoBehaviour {
	void Update () {
		if (Camera.main != null) {
			transform.LookAt (transform.position + Camera.main.transform.rotation * Vector3.forward,
		                  Camera.main.transform.rotation * Vector3.up);
		}
	}
}
