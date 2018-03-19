using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private DialogueManager dialogueManager;

    #region dialogue
    //Introduction. I.e tells the player what to do.
    [SerializeField] Dialogue dialogueIntro;
    //Player has accessed the server before computer, and is told to find computer.
    [SerializeField] Dialogue dialogueAccessServerBeforeComputer;
    //Player has accessed the server after accessing the computer, and is now told to hack.
    [SerializeField] Dialogue dialogueAccessServerAfterComputer;
    //Player accesses computer and is told to find server.
    [SerializeField] Dialogue dialogueAccessComputer;
    //Player hacks the server successfully.
    [SerializeField] Dialogue dialogueServerHackSuccess;
    //Player escapes.
    [SerializeField] Dialogue dialogueEscapeSuccess;
    #endregion

    #region objects
    [SerializeField] private Text fileIdText;
	[SerializeField] private Text serverIdText;
    #endregion

    private string targetFileID;

    //Game conditions.
    private bool pAccessComputer = false;
    private bool pAccessServer = false;
    private bool pSuccessHack = false;
    private bool pSuccessEscape = false;


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
        dialogueManager.LoadDialogue(dialogueIntro);
    }


    public void SetTargetFileID(string _id)
    {
        targetFileID = _id;
        fileIdText.text = targetFileID;
    }

	public void PlayerAccessedComputer()
	{
		pAccessComputer = true;
        serverIdText.text = targetFileID;
        dialogueManager.LoadDialogue(dialogueAccessComputer);
	}

    public void PlayerAccessedServer()
    {
        pAccessServer = true;
        if (pAccessComputer)
        {
            dialogueManager.LoadDialogue(dialogueAccessServerAfterComputer);
        } 
        else
        {
            dialogueManager.LoadDialogue(dialogueAccessServerBeforeComputer);
        }
    }

    public void PlayerHackedServer()
    {
		pSuccessHack = true;
        dialogueManager.LoadDialogue(dialogueServerHackSuccess);
    }

    public void PlayerLeavingLevel()
    {
        
    }
}
