using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueState : StateBaseClass {

	GuardMotor motor;
	GuardStateController gsc;
	Transform navTarget;
	NavMeshPath pathToLastKnownPos;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		navTarget = motor.GetPlayerTransform();
		pathToLastKnownPos = new NavMeshPath();
		NavMesh.CalculatePath(gameObject.transform.position, navTarget.position, NavMesh.AllAreas, pathToLastKnownPos); //Generate path to follow
	}

	public override void StateUpdate() {
		for (int i = 0; i < pathToLastKnownPos.corners.Length - 1; i++)
			Debug.DrawLine(pathToLastKnownPos.corners[i], pathToLastKnownPos.corners[i + 1], Color.red);
	}


	public override void StateExit() {

	}
}
