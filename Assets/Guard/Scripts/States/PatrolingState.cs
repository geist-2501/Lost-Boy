using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingState : StateBaseClass {

	public float timeToSpotPlayer = 1f;
	private float playerVisibleTimer;
	private Transform patrolPath;
	private Vector3[] waypoints;
	private int nextWaypoint;

	GuardMotor motor;
	GuardStateController gsc;

	public override void StateEnter() {
		motor = GetComponent<GuardMotor>();
		gsc = GetComponent<GuardStateController>();
		patrolPath = gsc.pathParent;

		waypoints = new Vector3[patrolPath.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints[i] = patrolPath.GetChild(i).position;
		}

		nextWaypoint = 0;

		//Find closest waypoint.
		for (int i = 0; i < waypoints.Length; i++) {
			float closestWaypointDist = (waypoints[nextWaypoint] - transform.position).magnitude;
			float currentWaypointDist = (waypoints[i] - transform.position).magnitude;
			if (currentWaypointDist < closestWaypointDist) {
				nextWaypoint = i;
			}
		}

	}

	public override void StateUpdate() {

		if (!motor.MoveTowards(waypoints[nextWaypoint])) {
			nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
		}

		if (motor.PlayerInVisionCone()) {
			gsc.ChangeState(typeof(SuspisiousState));
		}
	}

	public override void StateExit() {

	}

}
