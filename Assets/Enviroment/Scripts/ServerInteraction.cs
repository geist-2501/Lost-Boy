using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInteraction : InteractableBaseClass
{
    private GameManager gameManager; 


    [SerializeField] private Camera subCam;

    private bool isFocus = false;
    private bool firstTimeAccess = true;

    private Camera mainCam;
    private PlayerController playerController;
    private HUDManager HUD;

    [SerializeField] private Animator serverAnim;

    public override void Activate()
    {

        isFocus = true;

        if (isFocus && firstTimeAccess)
        {
            gameManager.PlayerAccessedServer();
            firstTimeAccess = false;
        }

        playerController.SetCanMove(false);
        HUD.SetHUDvisibility(false);

        mainCam.enabled = false;
        subCam.enabled = true;

        Cursor.lockState = CursorLockMode.None;

        serverAnim.SetTrigger("StartupTrigger");

    }

    // Use this for initialization
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        HUD = FindObjectOfType<HUDManager>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gameManager = FindObjectOfType<GameManager>();

        subCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus && Input.GetKeyDown(KeyCode.Escape))
        {
			ExitServer();
        }
    }

    public void ExitServer()
    {
        isFocus = false;

        playerController.SetCanMove(true);
        HUD.SetHUDvisibility(true);

        mainCam.enabled = true;
        subCam.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;

        serverAnim.SetTrigger("ShutdownTrigger");
    }


}
