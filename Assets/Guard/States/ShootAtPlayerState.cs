using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerState : StateBaseClass {

	GuardMotor motor;
	GuardStateController gsc;
	GameObject alert;

	Vector3 playerPos;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		alert = gsc.alertIcon;

		Instantiate(alert, transform);
		gsc.AIweapon.engage = true;
		Debug.Log("Pew! Pew! I'm shooting at you!");

	}

	public override void StateUpdate() {
		playerPos = motor.GetPlayerTransform().position;
		motor.TurnTowards(playerPos);
		if (!motor.PlayerInVisionCone()) {
			gsc.ChangeState(typeof(PursueState));
		}

	}

	public override void StateExit() {
		gsc.AIweapon.engage = false;
		Debug.Log("I can't see you anymore!");
	}
}
