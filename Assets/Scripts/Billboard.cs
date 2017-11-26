using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	Camera cam;

	private void Start() {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	private void LateUpdate() {
		transform.forward = -cam.transform.forward;
	}
}
