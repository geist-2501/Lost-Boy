using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspisiousState : StateBaseClass {

	GameObject player;
	GuardMotor motor;
	GuardStateController gsc;

	float timer = 0f;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		player = GameObject.FindGameObjectWithTag("Player");

		Debug.Log("Hmm?");
	}

	public override void StateUpdate() {
		motor.TurnTowards(player.transform.position);

		if (!motor.PlayerInVisionCone()) {
			gsc.ChangeState(typeof(PatrolingState));
		} else {
			timer += Time.deltaTime;
			if (timer >= 1) {
				gsc.ChangeState(typeof(ShootAtPlayerState));
			}
		}

	}

	public override void StateExit() {
		
	}

}
