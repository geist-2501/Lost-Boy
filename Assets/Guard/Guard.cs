using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	//Publics
	public float moveSpeed = 2f;
	public float turnSpeed = 2f;
	public float waitTime = 2f;

	public Transform pathParent;

	public Light spotlight;
	public float viewDistance;
	private float viewAngle;

	private GameObject player;

	private void Start() {

		player = GameObject.FindGameObjectWithTag ("Player");
		viewAngle = spotlight.spotAngle;

		//Store virtual waypoint positions.
		Vector3[] waypoints = new Vector3[pathParent.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints[i] = pathParent.GetChild(i).position;
			waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); //Offset them so the guard stays above the ground.
		}

		StartCoroutine(Patrol(waypoints));
	}

	IEnumerator Patrol(Vector3[] waypoints) {

		int nextWaypoint = 1;
		Vector3 targetDir = waypoints[nextWaypoint] - transform.position;

		while (true) {

			//Look at waypoint first, then move.
			if (transform.rotation == Quaternion.LookRotation(targetDir)) {
				transform.position = Vector3.MoveTowards(transform.position, waypoints[nextWaypoint], moveSpeed * Time.deltaTime);
			} else {
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, turnSpeed * Time.deltaTime, 0.0f));
			}


			//If guard has arrived at the waypoint, queue up the next one and wait.
			if (transform.position == waypoints[nextWaypoint]) {
				nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
				targetDir = waypoints[nextWaypoint] - transform.position;
				yield return new WaitForSeconds(waitTime);
			}
			yield return null;
		}
	}

	private void FixedUpdate() {

		DetectPlayer();

	}

	private void DetectPlayer() {

		RaycastHit hit;
		Vector3 _playerDisplacement = player.transform.position - transform.position;
		Ray _guard2player = new Ray(transform.position, _playerDisplacement);

		bool _playerVisible = Physics.Raycast(_guard2player, out hit, viewDistance, ~2); //~2 represents everything but the IgnoreRaycast layer.
		
		if (_playerVisible) {
			_playerVisible = (_playerVisible && hit.collider.gameObject == player);
			
		}

		

		if (_playerVisible && Vector3.Angle(transform.forward, _playerDisplacement) <= (viewAngle/2)) {
			Debug.Log("Guard sees player");
		}


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
