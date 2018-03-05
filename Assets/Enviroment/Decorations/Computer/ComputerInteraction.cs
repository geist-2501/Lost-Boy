using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteraction : InteractableBaseClass {

    [SerializeField] private Camera subCam;

    private bool isFocus = false;

    private Camera mainCam;
    private PlayerController playerController;
    private HUDManager HUD;

    private Animator compAnim;

    public override void Activate()
    {

        isFocus = true;

        playerController.SetCanMove(false);
        HUD.SetHUDvisibility(false);

        mainCam.enabled = false;
        subCam.enabled = true;

        Cursor.lockState = CursorLockMode.None;

        compAnim.SetTrigger("StartupTrigger");

    }

    // Use this for initialization
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        HUD = FindObjectOfType<HUDManager>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		compAnim = GetComponentInChildren<Animator>();

        subCam.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus && Input.GetKeyDown(KeyCode.Escape))
        {
			ExitComp();
        }
    }

    public void ExitComp()
    {
        isFocus = false;

        playerController.SetCanMove(true);
        HUD.SetHUDvisibility(true);

        mainCam.enabled = true;
        subCam.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;

        compAnim.SetTrigger("ShutdownTrigger");
    }

    //This method is required to let the animator set boolean parameters
    //because Unity only allows you to set trigger parameters for whatever reason.
    public void ToggleOpenEmail()
    {
        compAnim.SetBool("EmailOpenBool", !compAnim.GetBool("EmailOpenBool"));
    }
}
