using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableBaseClass {

	[SerializeField] private GameObject doorObject;
	[SerializeField] private float openAmount;
	[SerializeField] private float speed = 1f;

	private Vector3 openPos;
	private Vector3 closePos;

	private bool isOpen = false;

	private void Start() {
		//Make relative to world.
		closePos = transform.position;
		openPos = transform.position + new Vector3(0f, openAmount, 0f);
	}

	private void OnDrawGizmos() {
		Gizmos.DrawSphere(transform.position, 0.5f);
		Gizmos.DrawSphere(transform.position + new Vector3(0f, openAmount, 0f), 0.5f);
	}

	//Gets called by player motor.
	public override void Activate() {

		StopAllCoroutines();

		if (isOpen) {
			StartCoroutine(Open());
		} else {
			StartCoroutine(Close());
		}

		isOpen = !isOpen;

	}

	public IEnumerator Open() {
		while (doorObject.transform.position.y < openPos.y) {
			doorObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
			//If, in the next frame, the door is past the target position, just set it to the target position.
			//This prevents weird visual 'bumpy-ness'.
			if (doorObject.transform.position.y + speed * Time.deltaTime > openPos.y) {
				doorObject.transform.position = openPos;
			}
			yield return null;
		}
	}

	public IEnumerator Close() {
		while (doorObject.transform.position.y > closePos.y) {
			doorObject.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
			//If, in the next frame, the door is past the target position, just set it to the target position.
			//This prevents weird visual 'bumpy-ness'.
			if (doorObject.transform.position.y + speed * Time.deltaTime < closePos.y) {
				doorObject.transform.position = closePos;
			}
			yield return null;
		}
	}
}

