//Legacy - no longer works or is needed in current build.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DoorOld : MonoBehaviour {


	private bool isOpen = false;
	[SerializeField]
	private float distanceToOpen = 20f;
	[SerializeField]
	private float speed = 5f;
	private Vector3 closedPosition;
	private Vector3 openPosition;


	private void Start() {
		closedPosition = transform.position;
		openPosition = (transform.up * distanceToOpen) + closedPosition;
	}

	public void Interact() {
		StopAllCoroutines();
		StartCoroutine(OpenDoor());
	}

	IEnumerator OpenDoor () {

		isOpen = ! isOpen;

		while (isOpen) {
			transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
			if (transform.position == openPosition || ! isOpen) {
				break;
			}
			yield return null;

		}

		while ( ! isOpen ) {
			transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
			if (transform.position == closedPosition || isOpen) {
				break;
			}
			yield return null;
		}

	
	}
	
}
