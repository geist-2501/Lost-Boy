using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GuardMotor : MonoBehaviour {

	[SerializeField] private int health = 100;


	[SerializeField] private GameObject pointerObject;

	[SerializeField] private float viewDistance = 5f;
	[SerializeField] private float viewAngle = 10f;

	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float turnSpeed = 2f;

	private GameObject player;
	private Rigidbody rb;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void ChangeVeiwAngle(float _newAngle) {
		if (_newAngle < 180 && _newAngle > 0) {
			viewAngle = _newAngle;
		}
	}

	public bool PlayerInVisionCone() {

		RaycastHit _hit;
		Vector3 _playerDisplacement = player.transform.position - transform.position;
		Ray _guard2player = new Ray(transform.position, _playerDisplacement);

		bool _playerVisible = Physics.Raycast(_guard2player, out _hit, viewDistance, ~8, QueryTriggerInteraction.Ignore); //~2 represents everything but the IgnoreRaycast layer.
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

	public void TurnTowards(Vector3 _dir) {
		//Calculate the vector of mag. 1 point in the supplied direction.
		Vector3 _targetDir = Vector3.Normalize(_dir - transform.position);
		//Rotate around the y axis only, hence exclude it.
		Vector3 _isolatedDirY = new Vector3(_targetDir.x, 0f, _targetDir.z).normalized;
		//Rotate around the x axis only, hence exclude it.
		Vector3 _isolatedDirX = new Vector3(0f, _targetDir.y, _targetDir.z).normalized;
		//Apply y axis rotation to guard body.
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _isolatedDirY, turnSpeed * Time.deltaTime, 0.0f), transform.up);

		if (pointerObject) {
			//Apply x axis rotation to pointer, only if it's assigned.
			pointerObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(pointerObject.transform.forward,
				_targetDir, turnSpeed * Time.deltaTime, 0.0f), transform.up);
		} 

	}

	public bool MoveTowards(Vector3 _pos) {

		Debug.DrawLine(transform.position, _pos);

		Vector3 _targetDir = _pos - transform.position;
		Vector3 _isolatedDir = new Vector3(_targetDir.x, 0f , _targetDir.z); //Isolate to guards current elevation.

		if (_isolatedDir.magnitude <= 0.5f) {
			return false; //Returning false because it can't get any closer to _pos.
		}

		//If there is a large difference in direction, stop and correct it,
		//Otherwise it must be a small difference and can be corrected while moving.
		if (Vector3.Angle(_isolatedDir.normalized, transform.forward) <= 5.0f) {
			TurnTowards(_pos);
			rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
		} else {
			TurnTowards(_pos);
		}

		return true; //Return true if can still move.

		
	}

	public void TakeDamage(int _damage) {
		health -= _damage;
		if (health <= 0) {
			Destroy(gameObject);
		}
	}


}
