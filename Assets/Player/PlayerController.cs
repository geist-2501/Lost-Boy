using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float health = 100f;
	[SerializeField]
	private float energy = 50f;
	[SerializeField]
	private float energyConsumptionRate = 1f;
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float sprintModifier = 1.5f;
	[SerializeField]
	private float lookSensitivity = 5f;
	[SerializeField]
	private float jumpForce = 5f;
	[SerializeField]
	private Gun currentGun;

	private bool canMove = true;
	private bool focusedOnOther = false;

	[SerializeField]
	private MotorBaseClass motor;
	private HUDManager HUD;
	

	private void Start() {
		HUD = FindObjectOfType<HUDManager>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ChangeMotor(MotorBaseClass _motor) {
		motor = _motor;
	}

	public void SetCanMove(bool _canMove) {
		canMove = _canMove;
		motor.canMove = _canMove;
		currentGun.isAbleToShoot = _canMove;
	}

	public void TakeDamage(float _damage) {
		health -= _damage;
		if (health <= 0) {
			//TODO gameover!
			Debug.Log("You are dead!");
		}
	}

	private void Update() {

		//Update HUD.
		HUD.healthBar.setValue(health);
		HUD.energyBar.setValue(energy);

		if (motor.canMove) {

			float _xMov = Input.GetAxisRaw("Horizontal");
			float _zMov = Input.GetAxisRaw("Vertical");

			Vector3 _xVel = transform.right * _xMov;
			Vector3 _zVel = transform.forward * _zMov;

			//Direction * speed.
			Vector3 _velocity = (_xVel + _zVel).normalized * speed;

			//Apply sprint modifier if shift key is down.
			if (Input.GetAxisRaw("Fire3") == 1 && energy > 0) {
				_velocity *= sprintModifier;
				energy -= energyConsumptionRate;
			}

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

			//Send jump signal to motor.
			bool _isJumping = Input.GetKey(KeyCode.Space);
			if (_isJumping) {
				motor.Jump(jumpForce);
			} else {
				_isJumping = false;
			}

			//Send interact signal to motor.
			if (Input.GetKeyDown(KeyCode.E)) {
				motor.Interact();
			}
		}
	}

}
