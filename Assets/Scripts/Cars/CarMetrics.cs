using UnityEngine;
using System.Collections;

public class CarMetrics : MonoBehaviour {

	public float currentGas = 50f;
	public float maxGas = 50f;
	public float currentHealth = 50f;
	public float maxHealth = 100f;
	public float currentReputation = 50f;
	public float maxReputation = 50f;
	public float currentStamina = 0f;
	public float maxStamina = 100f;
	float fakeGravityForce;

	public Light[] headLights;
	public Light[] tailLights;


	[SerializeField] protected Material blackened;
	[SerializeField] protected Transform[] m_hoverPoints;

	protected Rigidbody m_body;
	protected bool isTouchingGround;

	protected float deadZone = 0.1f;
	protected float hoverForce = 3000.0f;
	protected float hoverHeight = 1.0f;

	protected int m_layerMask;


	public void TakeDamage(float x) {
		currentHealth -= x;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public virtual void Die () {

	}

	protected void Start() {
		m_body = GetComponent<Rigidbody> ();
		m_layerMask = 1 << LayerMask.NameToLayer ("Characters");
		m_layerMask = ~m_layerMask;
	}

	public void FaceTarget(Vector3 target) {
		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, Quaternion.LookRotation(Vector3.RotateTowards (transform.forward, target - transform.position, .015f,.4f),Vector3.up).eulerAngles.y,transform.rotation.eulerAngles.z));
	}

	protected void BlackenCarMaterials() {
		foreach (MeshRenderer x in GetComponentsInChildren<MeshRenderer>()) {
			Material[] mats = new Material[x.materials.Length];
			for (int i = 0; i < x.materials.Length; i++) { 
				mats[i] = blackened;
			}
			x.materials = mats;
		}
	}

	protected void ToggleLights(bool isOn){
		foreach (Light x in tailLights) {
			x.enabled = isOn;
		}
		foreach (Light y in headLights) {
			y.enabled = isOn;
		}
	}

	protected void FixedUpdate () {
		HoverPhysics ();
	}

	protected void StabilizeMidAir () {
		if (Mathf.Abs (transform.rotation.eulerAngles.x) > 1f) {
			float negOrPosVal = (transform.rotation.eulerAngles.x < 180) || (transform.rotation.eulerAngles.x < 0) ? -1f : 1f;
			transform.Rotate (negOrPosVal * 0.1f, 0, 0);
		}
		if (Mathf.Abs (transform.rotation.eulerAngles.z) > 1f) {
			float negOrPosVal = (transform.rotation.eulerAngles.z < 180) ? -1f : 1f;
			transform.Rotate (0, 0, negOrPosVal * 0.1f);
		}
	}

	void HoverPhysics() {
		RaycastHit hit;
		for (int i = 0; i < m_hoverPoints.Length; i++) {
			Transform hoverPoint = m_hoverPoints [i];
			if (Physics.Raycast (hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight, m_layerMask)) {
				m_body.AddForceAtPosition (Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
				isTouchingGround = true;
			} else {
				isTouchingGround = false;
				StabilizeMidAir ();
				if (transform.position.y > hoverPoint.transform.position.y) {
					m_body.AddForceAtPosition (hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				} else {
					m_body.AddForceAtPosition (hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
				}
			}
		}
	}
}
