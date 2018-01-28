using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerMotor : MotorBaseClass {

	[SerializeField] private Camera cam;
	[SerializeField] private float interactionRange = 2f;

	private HUDManager HUD;


	private void Start() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
		HUD = FindObjectOfType<HUDManager>();
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

	public override void PerformInteractions() {

		Ray _cameraForward = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit hit;
		InteractableBaseClass interactable = null;

		if (Physics.Raycast(_cameraForward, out hit, interactionRange)) {
			bool _interactablePresent = hit.transform.gameObject.GetComponent<InteractableBaseClass>();
			HUD.interactText.gameObject.SetActive(_interactablePresent);
			if (_interactablePresent) {
				interactable = hit.transform.gameObject.GetComponent<InteractableBaseClass>();
			} else {
				interactable = null;
			}
		} else {
			HUD.interactText.gameObject.SetActive(false);
		}


		if (isInteracting) {
			isInteracting = false;
			if (interactable) {
				interactable.Activate();
			}
		}
	}





}
