using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerMotor : MotorBaseClass {

	[SerializeField] private Camera cam;
	[SerializeField] private int health = 100;


	private void Start() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
	}

	public override void PerformMovement() {
		if (velocity != Vector3.zero) {
			rb.MovePosition(rb.position + velocity * Time.deltaTime);
		}

		Ray _checkGroundClearance = new Ray(transform.position, -transform.up); //shoot a ray downwards.
		canJump = Physics.Raycast(_checkGroundClearance, (col.height / 2) + 0.01f);

		if (canJump) {
			rb.AddForce(jumpForce, ForceMode.Impulse);
			jumpForce = Vector3.zero;
		}
	}

	public override void PerformRotation() {
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null) {
			cam.transform.Rotate(-cameraRotaion);
		}

	}

	public void TakeDamage(int _damage) {
		health -= _damage;
		if (health <= 0) {
			//TODO gameover!
			Debug.Log("You are dead!");
		}
	}

}
