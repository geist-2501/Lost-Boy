using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Blair Cross
This script sits on a guard and controls its interactions
with the world.
*/

[RequireComponent(typeof(Collider))]
public class GuardMotor : MonoBehaviour {

	[SerializeField] private int health = 100;

	//What the guard points with. I.e a gun.
	[SerializeField] private GameObject pointerObject;

	//Guards feild of veiw. Keep in mind these values might
	//get changed in the editor and 5f and 10f won't be their 
	//actual values.
	[SerializeField] private float viewDistance = 5f;
	[SerializeField] private float viewAngle = 10f;

	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float turnSpeed = 2f;

	//Reference to the player.
	private GameObject player;
	//Guards rigidbody component. A rigidbody component allows
	//gameObjects to have physics.
	private Rigidbody rb;

	private void Start() {
		//Find the rigidbody component on the guard.
		rb = GetComponent<Rigidbody>();
		//Find the player.
		player = GameObject.FindGameObjectWithTag("Player");
	}

	
	public void ChangeVeiwAngle(float _newAngle) {
		if (_newAngle < 180 && _newAngle > 0) {
			viewAngle = _newAngle;
		}
	}

	//Gets called to check if the player is 
	//in the guards vision cone.
	//Returns true if it is, false if it isn't.
	public bool PlayerInVisionCone() {

		RaycastHit _hit;
		//3D vector from the guard to the player.
		Vector3 _playerDisplacement = player.transform.position - transform.position;
		//Ray from the guard to the player.
		Ray _guard2player = new Ray(transform.position, _playerDisplacement);

		//True if there is a clear line of sight between the guard
		//and the player. 
		bool _playerVisible = Physics.Raycast(_guard2player, out _hit, viewDistance, ~8, QueryTriggerInteraction.Ignore);

		//Draws a line from the guard to the player for debugging.
		Debug.DrawRay(transform.position, _playerDisplacement, Color.red);

		//Angle in radians.
		float _angleInRad = viewAngle * Mathf.Deg2Rad;

		//The couple of following lines of code are for visualising the 
		//guards vision cone.
		//Unit vector that is pointing in the forward direction relative to the guard.
		Vector3 _newViewRay = transform.forward;

		//Rotate a vector on the xz plane by however many 
		//radians the guard can see and mirror it.

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

		if (_playerVisible) 
		{ 
			//If something was detected, check if it was the player.
			_playerVisible = (_playerVisible && _hit.collider.gameObject == player); 
		} 

		//Flatten the displacement vector, incase the 
		//guard and player are on different elevations.
		//This makes the vision cone more like a vision V.
		Vector3 _flatPlayerDisplacement = new Vector3(_playerDisplacement.x, 0f, _playerDisplacement.z).normalized;

		//If the player is visible, and within the 
		//vision cone, the guar dcan see the player.
		if (_playerVisible && Vector3.Angle(transform.forward, _flatPlayerDisplacement) <= viewAngle) {
			return true;
		}

		return false;
	}

	public Transform GetPlayerTransform() {
		return player.transform;
	}

	//Motor command that makes the guard turn towards a direction.
	public void TurnTowards(Vector3 _dir) {
		//Calculate the vector of mag. 1 point in the supplied direction.
		Vector3 _targetDir = Vector3.Normalize(_dir - transform.position);
		//Rotate around the y axis only, hence exclude it.
		Vector3 _isolatedDirY = new Vector3(_targetDir.x, 0f, _targetDir.z).normalized;
		//Rotate around the x axis only, hence exclude it.
		Vector3 _isolatedDirX = new Vector3(0f, _targetDir.y, _targetDir.z).normalized;
		//Apply y axis rotation to guard body.
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, 
			_isolatedDirY, turnSpeed * Time.deltaTime, 0.0f), transform.up);

		if (pointerObject) {
			//Apply x axis rotation to pointer, only if it's assigned.
			pointerObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(pointerObject.transform.forward,
				_targetDir, turnSpeed * Time.deltaTime, 0.0f), transform.up);
		} 

	}

	//Motor command that makes the guards move towards a position.
	//Returns false if its within 0.5f of the target position.
	public bool MoveTowards(Vector3 _pos) {

		Debug.DrawLine(transform.position, _pos);

		Vector3 _targetDir = _pos - transform.position;
		//Isolate to guards current elevation.
		Vector3 _isolatedDir = new Vector3(_targetDir.x, 0f , _targetDir.z); 

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

	//If the guard collides with a door, open it.
	private void OnTriggerEnter(Collider other) {
		DoorInteraction interactable = other.GetComponent<DoorInteraction>();
		//Interactable will be null if there is no interactable script on the object.
		if (interactable) {	
			interactable.ForceOpen();
		}
	}

}
