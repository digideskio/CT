using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

	protected bool isReceivingInteract;

	protected void OnEnable() {
		PlayerCar.PlayerInteractStart+=ReceiveInteract;
		PlayerCar.PlayerInteractEnd+=ReceiveDeinteract;
		PlayerCar.BeginTarget += ReceiveTarget;
		PlayerCar.EndTarget += ReceiveUntarget;

	}

	protected void OnDisable() {
		PlayerCar.PlayerInteractStart-=ReceiveInteract;
		PlayerCar.PlayerInteractEnd-=ReceiveDeinteract;
		PlayerCar.BeginTarget -= ReceiveTarget;
		PlayerCar.EndTarget -= ReceiveUntarget;
	}

	public virtual void ReceiveInteract () {
		isReceivingInteract = true;
	}

	public virtual void ReceiveDeinteract () {
		isReceivingInteract = false;

	}
	public virtual void ReceiveTarget () {
		isReceivingInteract = true;
	}

	public virtual void ReceiveUntarget () {
		isReceivingInteract = false;

	}
}
