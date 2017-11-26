using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateController : MonoBehaviour {

	public StateBaseClass currentState;
	public GameObject alertIcon;
	public Transform pathParent;

	private void Start() {

		currentState = gameObject.AddComponent<PatrolingState>();
	
		currentState.StateEnter();
	}

	// Update is called once per frame
	void Update () {
		currentState.StateUpdate();
	}

	public void ChangeState(System.Type type) {
		currentState.StateExit();
		Debug.Log("I'm changing state from " + currentState + ",");
		Destroy(currentState);
		currentState = gameObject.AddComponent(type) as StateBaseClass;
		Debug.Log("to " + currentState + "!");
		currentState.StateEnter();
	}
}
