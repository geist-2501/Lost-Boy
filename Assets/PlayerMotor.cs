using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 cameraRotaion = Vector3.zero;

	private Rigidbody rb;


	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	//Gets a movement vector from controller.
	public void Move(Vector3 _velocity) {
		velocity = _velocity;
	}

	//Gets a y-axis rotational vector from controller.
	public void Rotate (Vector3 _rotation) {
		rotation = _rotation;
	}

	//Gets a x-axis roational vector form controller.
	public void RotateCamera(Vector3 _cameraRotaion) {
		cameraRotaion = _cameraRotaion;
	}


	private void FixedUpdate() {
		PerformMovement();
		PerformRotation();
	}


	private void PerformMovement() {
		if (velocity != Vector3.zero) {
			rb.MovePosition(rb.position + velocity * Time.deltaTime);
		}
	}


	private void PerformRotation() {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null) {
			cam.transform.Rotate(-cameraRotaion);
		}

	}

}
