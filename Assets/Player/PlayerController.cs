using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public bool canMove = true;

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 5f;
	[SerializeField]
	private float jumpForce = 5f;
	[SerializeField]


	private MotorBaseClass motor;
	private Camera cam;

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;

		cam = transform.GetComponentInChildren<Camera>();
	}

	public void ChangeMotor(MotorBaseClass _motor) {
		motor = _motor;
	}

	private void Update() {

		if (canMove) {
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

			bool _isJumping = Input.GetButtonDown("Jump");
			if (_isJumping) {
				motor.Jump(jumpForce);
			} else {
				_isJumping = false;
			}

		}
		
	}


}
