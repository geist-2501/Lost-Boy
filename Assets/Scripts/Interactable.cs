using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {

	public void ActivateInteractables () {
		SendMessage("Interact");
	}
	
}
