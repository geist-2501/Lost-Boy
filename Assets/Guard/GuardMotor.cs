using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GuardMotor : MonoBehaviour {

	public float viewDistance = 5f;
	[SerializeField] private float viewAngle = 10f;

	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float turnSpeed = 2f;
	[SerializeField] private float waitTime = 2f;



	private GameObject player;
	private Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public bool PlayerInVisionCone() {

		RaycastHit _hit;
		Vector3 _playerDisplacement = player.transform.position - transform.position;
		Ray _guard2player = new Ray(transform.position, _playerDisplacement);

		bool _playerVisible = Physics.Raycast(_guard2player, out _hit, viewDistance, ~8); //~2 represents everything but the IgnoreRaycast layer.
		Debug.DrawRay(transform.position, _playerDisplacement, Color.red);

		float _angleInRad = viewAngle * Mathf.Deg2Rad;

		Vector3 _newViewRay = transform.forward;
		//Rotation matrix;
		//x' = xCosO - zSinO
		//z' = xSinO + zCosO
		float _newX = _newViewRay.x * Mathf.Cos(_angleInRad) - _newViewRay.z * Mathf.Sin(_angleInRad);
		float _newZ = _newViewRay.x * Mathf.Sin(_angleInRad) + _newViewRay.z * Mathf.Cos(_angleInRad);
		_newViewRay = new Vector3(_newX, 0f, _newZ).normalized;
		//Mirror against vector;
		//mirrVect = origVect - 2 * (origVect dot axisVect) * axisVect
		Vector3 _mirroredVeiwRay = -_newViewRay - 2 * Vector3.Dot(-_newViewRay, transform.forward) * transform.forward;
		Debug.DrawRay(transform.position, _newViewRay);
		Debug.DrawRay(transform.position, transform.forward);
		Debug.DrawRay(transform.position, _mirroredVeiwRay);

		//Debug.Log(Vector3.Angle(transform.forward, _playerDisplacement));

		if (_playerVisible) { _playerVisible = (_playerVisible && _hit.collider.gameObject == player); } //If something was detected, check if it was the player.

		Vector3 _flatPlayerDisplacement = new Vector3(_playerDisplacement.x, 0f, _playerDisplacement.z).normalized;

		if (_playerVisible && Vector3.Angle(transform.forward, _flatPlayerDisplacement) <= viewAngle) {
			return true;
		}

		return false;
	}

	public Transform GetPlayerTransform() {
		return player.transform;
	}

	public void StartPatrol (Transform _path) {
		if (_path.childCount == 0) {
			throw new UnityException("Path has no waypoints!");
		}

		//Store virtual waypoint positions.
		Vector3[] _waypoints = new Vector3[_path.childCount];
		for (int i = 0; i < _waypoints.Length; i++) {
			_waypoints[i] = _path.GetChild(i).position;
			_waypoints[i] = new Vector3(_waypoints[i].x, transform.position.y, _waypoints[i].z); //Raise waypoints level to the guard.
		}

		StartCoroutine(Patrol(_waypoints));
	}

	public void StopPatrol () {
		StopAllCoroutines();
	}

	public void TurnTowards(Vector3 _dir) {
		Vector3 _targetDir = Vector3.Normalize(_dir - transform.position);
		//Rotate around the z axis only, hence exclude it.
		Vector3 _isolatedDir = new Vector3(_targetDir.x, 0f, _targetDir.z).normalized;
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _isolatedDir, turnSpeed * Time.deltaTime, 0.0f), transform.up);
	}

	public bool MoveTowards(Vector3 _pos) {
		Debug.DrawLine(transform.position, _pos);
		Vector3 _targetDir = _pos - transform.position;
		Vector3 _isolatedDir = new Vector3(_targetDir.x, 0f , _targetDir.z); //Isolate to guards current elevation.

		if (_isolatedDir.magnitude <= 0.5f) {
			return false; //Returning false because it can't get any closer to _pos.
		}

		if (Vector3.Angle(_isolatedDir.normalized, transform.forward) <= 5.0f) {
			TurnTowards(_pos);
			rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
		} else {
			TurnTowards(_pos);
		}

		return true; //Return true if can still move.

		
	}

	private IEnumerator Patrol (Vector3[] _waypoints) {

		int _nextWaypoint = 0; //Initialize.

		//Find closest waypoint.
		for (int i = 0; i < _waypoints.Length; i++) {
			float closestWaypointDist = (_waypoints[_nextWaypoint] - transform.position).magnitude;
			float currentWaypointDist = (_waypoints[i] - transform.position).magnitude;
			if (currentWaypointDist < closestWaypointDist) {
				_nextWaypoint = i;
			}
		}

		Vector3 _targetWaypointDir = Vector3.Normalize(_waypoints[_nextWaypoint] - transform.position);

		while (true) {

			//Look at waypoint first, then move.
			if (transform.rotation == Quaternion.LookRotation(_targetWaypointDir)) {
				transform.position = Vector3.MoveTowards(transform.position, _waypoints[_nextWaypoint], moveSpeed * Time.deltaTime);
			} else {
				TurnTowards(_waypoints[_nextWaypoint]); ;
			}

			//If guard has arrived at the waypoint, queue up the next one and wait.
			if (transform.position == _waypoints[_nextWaypoint]) {
				_nextWaypoint = (_nextWaypoint + 1) % _waypoints.Length;
				_targetWaypointDir = _waypoints[_nextWaypoint] - transform.position;
				yield return new WaitForSeconds(waitTime);
			}
			yield return null;
		}
	}
}
