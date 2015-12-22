using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GasTank : InteractableObject {

	public float initialGas = 20;
	float gasLeft;
	public float costOfGas;
	public float gasFillSpeed = 1f;
	Material thisMaterial;
	Color thisGasColor;

	bool isPlayerInProximity;

	[SerializeField]
	Text costOfGasText;

	// Use this for initialization
	void Start () {
		costOfGasText.text = "Gas\n" + "$" + costOfGas;
		gasLeft = initialGas;
		thisMaterial = transform.GetChild(0).GetComponent<MeshRenderer> ().material;
		thisGasColor = thisMaterial.GetColor ("_MainColor");
			
	}

	void OnTriggerEnter (Collider other) {
		if (!isPlayerInProximity) {
			costOfGasText.gameObject.GetComponent<Fader> ().StartFadeIn ();
		}
		if (other.tag == "Player") {
			isPlayerInProximity = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (isPlayerInProximity) {
			costOfGasText.gameObject.GetComponent<Fader> ().StartFadeOut ();
		}
		if (other.tag == "Player") {
			isPlayerInProximity = false;

		}
	}

	void Update () {
		if (isPlayerInProximity && isReceivingInteract && PlayerCar.s_instance.money > 0 && gasLeft > 0) {
			GiveGasToPlayer ();
			gasLeft -= Time.deltaTime * gasFillSpeed;

		}

		SetGasMaterialColor ();
	}

	void GiveGasToPlayer () {
		PlayerCar.s_instance.money -= Time.deltaTime * costOfGas / 60f;
		PlayerCar.s_instance.currentGas += Time.deltaTime * gasFillSpeed;
	}

	void SetGasMaterialColor() {
		Color gasBulbColor = Color.Lerp (Color.black, thisGasColor,gasLeft / initialGas);
		transform.GetChild (0).GetComponent<MeshRenderer>().material.SetColor("_MainColor", gasBulbColor);
		transform.GetChild (0).GetComponent<MeshRenderer>().material.SetColor("_RimColor", gasBulbColor);

	}
}
