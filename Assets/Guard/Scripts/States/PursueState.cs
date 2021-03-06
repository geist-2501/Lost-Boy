﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueState : StateBaseClass {

	GuardMotor motor;
	GuardStateController gsc;
	Transform navTarget;
	NavMeshPath pathToLastKnownPos;
	int CurrentCorner = 0;
	float timer = 0;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		navTarget = motor.GetPlayerTransform();
		pathToLastKnownPos = new NavMeshPath();
		NavMesh.CalculatePath(gameObject.transform.position, navTarget.position, NavMesh.AllAreas, pathToLastKnownPos); //Generate path to follow

	}

	public override void StateUpdate() {

		for (int i = 0; i < pathToLastKnownPos.corners.Length - 1; i++) {
			Debug.DrawLine(pathToLastKnownPos.corners[i], pathToLastKnownPos.corners[i + 1], Color.blue);
		}

		//If can see player, go back to attacking!
		if (motor.PlayerInVisionCone()) {
			gsc.ChangeState(typeof(ShootAtPlayerState));
		}

		if (pathToLastKnownPos == null)
		{
			//Path is invalid somehow. I.e play is in mid air when they go out of the guards vision.
			//For now just jump back to patrolling untill a solution is found.
			gsc.ChangeState(typeof(PatrolingState));
		}

		if (!motor.MoveTowards(pathToLastKnownPos.corners[CurrentCorner])) {
			if (CurrentCorner < pathToLastKnownPos.corners.Length - 1) {
				CurrentCorner++;
			} else {
				motor.ChangeVeiwAngle(90);
				timer += Time.deltaTime;
				if (timer >= 1) {
					gsc.ChangeState(typeof(PatrolingState));
				}
			}
		}
	}


	public override void StateExit() {
		motor.ChangeVeiwAngle(60);
	}
}
