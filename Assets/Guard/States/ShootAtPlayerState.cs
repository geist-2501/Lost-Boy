using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerState : StateBaseClass {

	GuardMotor motor;
	GuardStateController gsc;
	GameObject alert;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		alert = gsc.alertIcon;
		Instantiate(alert, transform);
		Debug.Log("Pew! Pew! I'm shooting at you!");

	}

	public override void StateUpdate() {
		motor.TurnTowards(motor.GetPlayerTransform().position);
		if (!motor.PlayerInVisionCone()) {
			gsc.ChangeState(typeof(PursueState));
		}

	}

	public override void StateExit() {
		Debug.Log("I can't see you anymore!");
	}
}
