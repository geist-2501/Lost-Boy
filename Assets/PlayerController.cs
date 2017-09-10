using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 5f;

	private PlayerMotor motor;

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		motor = GetComponent<PlayerMotor>();
	}


	private void Update() {

		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _xVel = transform.right * _xMov;
		Vector3 _zVel = transform.forward * _zMov;

		//Direction * speed.
		Vector3 _velocity = (_xVel + _zVel).normalized * speed;

		//Apply movement.
		motor.Move(_velocity);

		//Pivot around y-axis.
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0, _yRot, 0) * lookSensitivity;

		//Apply rotation.
		motor.Rotate(_rotation);

		//Pivot camera up and down.
		float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot, 0, 0) * lookSensitivity;

		//Apply rotation.
		motor.RotateCamera(_cameraRotation);
	}

}
