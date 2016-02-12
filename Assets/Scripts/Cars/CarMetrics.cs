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
	public Light[] headLights;
	public Light[] tailLights;
	[SerializeField] protected Material blackened;

	
	public void TakeDamage(float x) {
		currentHealth -= x;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public virtual void Die () {

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
}
