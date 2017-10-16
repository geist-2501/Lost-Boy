using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {

	private bool isFocus = false;
	private PlayerController playerController;
	public Transform mountPoint;
	private Camera cam;

	private void Start() {
		playerController = FindObjectOfType<PlayerController>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	public void Interact() {
		isFocus = !isFocus;

		if (isFocus) {
			playerController.canMove = false;
			cam.transform.position = mountPoint.position;
			cam.transform.rotation = Quaternion.LookRotation(transform.position - mountPoint.position);
		} else {
			playerController.canMove = true;
		}
	}

}
