using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

#region Variables
	//Movement.
	public float moveSpeed = 2f;
	public float turnSpeed = 2f;
	public float waitTime = 2f;
	public Transform pathParent;
	//Detection.
	public float timeToSpotPlayer = 1f;
	private float playerVisibleTimer;
	public Light spotlight;
	public float viewDistance;
	private float viewAngle;
	private Color originalSpotlightColour;
	//Misc.
	private GameObject player;
#endregion

	private void Start() {

		originalSpotlightColour = spotlight.color;
		player = GameObject.FindGameObjectWithTag ("Player");
		viewAngle = spotlight.spotAngle;

		//Store virtual waypoint positions.
		Vector3[] _waypoints = new Vector3[pathParent.childCount];
		for (int i = 0; i < _waypoints.Length; i++) {
			_waypoints[i] = pathParent.GetChild(i).position;
			_waypoints[i] = new Vector3(_waypoints[i].x, transform.position.y, _waypoints[i].z); //Offset them so the guard stays above the ground.
		}

		StartCoroutine(Patrol(_waypoints));
	}

	private void Update() {
		if (CanSeePlayer()) {
			playerVisibleTimer += Time.deltaTime;
		} else {
			playerVisibleTimer -= Time.deltaTime;
		}

		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);
	}

	IEnumerator Patrol(Vector3[] waypoints) {

		int _nextWaypoint = 0;

		//Find closest waypoint to start patrolling.
		for (int i = 1; i < waypoints.Length; i++) {
			float closestWaypointDist = (waypoints[_nextWaypoint] - transform.position).magnitude;
			float currentWaypointDist = (waypoints[i] - transform.position).magnitude;
			if (currentWaypointDist < closestWaypointDist) {
				_nextWaypoint = i;
			}
		}

		Vector3 _targetDir = waypoints[_nextWaypoint] - transform.position;

		while (true) {

			//Look at waypoint first, then move.
			if (transform.rotation == Quaternion.LookRotation(_targetDir)) {
				transform.position = Vector3.MoveTowards(transform.position, waypoints[_nextWaypoint], moveSpeed * Time.deltaTime);
			} else {
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _targetDir, turnSpeed * Time.deltaTime, 0.0f));
			}


			//If guard has arrived at the waypoint, queue up the next one and wait.
			if (transform.position == waypoints[_nextWaypoint]) {
				_nextWaypoint = (_nextWaypoint + 1) % waypoints.Length;
				_targetDir = waypoints[_nextWaypoint] - transform.position;
				yield return new WaitForSeconds(waitTime);
			}
			yield return null;
		}
	}

	private bool CanSeePlayer() {

		RaycastHit _hit;
		Vector3 _playerDisplacement = player.transform.position - transform.position;
		Ray _guard2player = new Ray(transform.position, _playerDisplacement);

		bool _playerVisible = Physics.Raycast(_guard2player, out _hit, viewDistance, ~8); //~2 represents everything but the IgnoreRaycast layer.

		if (_playerVisible) { _playerVisible = (_playerVisible && _hit.collider.gameObject == player); } //If something was detected, check if it was the player.

		if (_playerVisible && Vector3.Angle(transform.forward, _playerDisplacement) <= (viewAngle / 2)) {
			return true;
		}

		return false;
	}

	private void OnDrawGizmos() {

		//For visualising the waypoints in scene veiw.
		Vector3 startPos = pathParent.GetChild(0).position;



		foreach (Transform waypoint in pathParent) {

			Gizmos.color = Color.white;
			Gizmos.DrawSphere(waypoint.position, 0.3f);

			Gizmos.color = Color.green;
			Gizmos.DrawLine(startPos, waypoint.position);
			startPos = waypoint.position;
		}

		Gizmos.DrawLine(startPos, pathParent.GetChild(0).position);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
	}

}
