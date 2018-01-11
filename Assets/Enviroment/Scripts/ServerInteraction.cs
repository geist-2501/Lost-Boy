﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInteraction : InteractableBaseClass {

	[SerializeField] private Camera subCam;

	private bool isFocus = false;

	private Camera mainCam;
	private PlayerController playerController;
	private HUDManager HUD;

	[SerializeField] private Animator serverAnim;

	public override void Activate() {

		isFocus = true;

		playerController.SetCanMove(false);
		HUD.SetInteractText(false);
		HUD.SetRecticleVisibility(false);

		mainCam.enabled = false;
		subCam.enabled = true;

		Cursor.lockState = CursorLockMode.None;

		serverAnim.SetBool("Shutdown", false);

	}

	// Use this for initialization
	void Start () {
		playerController = FindObjectOfType<PlayerController>();
		HUD = FindObjectOfType<HUDManager>();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFocus && Input.GetKeyDown(KeyCode.Escape)) {
			isFocus = false;

			playerController.SetCanMove(true);
			HUD.SetInteractText(true);
			HUD.SetRecticleVisibility(true);

			mainCam.enabled = true;
			subCam.enabled = false;

			Cursor.lockState = CursorLockMode.Locked;

			serverAnim.SetBool("Shutdown", true);
		}
	}
}
