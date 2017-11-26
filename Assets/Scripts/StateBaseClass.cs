using UnityEngine;

public abstract class StateBaseClass : MonoBehaviour {

	public abstract void StateEnter();
	public abstract void StateUpdate();
	public abstract void StateExit();

}
