using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

	protected bool isReceivingInteract;

	protected void OnEnable() {
		PlayerCar.PlayerInteractStart+=ReceiveInteract;
		PlayerCar.PlayerInteractEnd+=ReceiveDeinteract;
	}

	protected void OnDisable() {
		PlayerCar.PlayerInteractStart-=ReceiveInteract;
		PlayerCar.PlayerInteractEnd-=ReceiveDeinteract;
	}

	protected void ReceiveInteract () {
		isReceivingInteract = true;
	}

	protected void ReceiveDeinteract () {
		isReceivingInteract = false;

	}
}
