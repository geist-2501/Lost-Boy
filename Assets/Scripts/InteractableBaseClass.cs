using UnityEngine;

public abstract class InteractableBaseClass : MonoBehaviour {

	//Just for those special cases where an interactable doesn't want to get interacted with.
	public bool isLocked;

	//Every class that inherits from this must implement this function.
	//This allows every interactable to be accessed generically.
	public abstract void Activate();
	
}
