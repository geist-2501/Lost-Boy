using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	public Camera targetCam;
	[SerializeField] private bool waitForTracking = false;

	private void Start() {
		if ( ! targetCam) {
			//targetCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			targetCam = Camera.main;
		}
	}

	private void LateUpdate() {
		if (! waitForTracking) {
			transform.forward = targetCam.transform.forward;
		}
	}

	public void SetTracking (bool _setTracking) {
		waitForTracking = _setTracking;
	}
}
