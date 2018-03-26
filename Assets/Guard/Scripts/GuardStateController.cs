using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Blair Cross
Central storage for AI, and facilitates state
changes.
 */

public class GuardStateController : MonoBehaviour {

	public StateBaseClass currentState;
	public GameObject alertIcon;
	public Transform pathParent;
	public AIgun AIweapon;

	private void Start() {
		currentState = gameObject.AddComponent<PatrolingState>();
		currentState.StateEnter();
	}

	// Update is called once per frame.
	void Update () {
		currentState.StateUpdate();
	}

	//Called by states to change into a different state.
	public void ChangeState(System.Type type) {
		//Call the old state's exit method. 
		currentState.StateExit();
		Debug.Log("I'm changing state from " + currentState + ",");
		//Remove the old state.
		Destroy(currentState);
		//Add the new state.
		currentState = gameObject.AddComponent(type) as StateBaseClass;
		Debug.Log("to " + currentState + "!");
		//Call the new state's entry method.
		currentState.StateEnter();
	}
}
