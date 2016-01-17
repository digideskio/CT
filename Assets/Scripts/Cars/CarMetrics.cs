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
	
	public void TakeDamage(float x) {
		currentHealth -= x;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public virtual void Die () {

	}


}
