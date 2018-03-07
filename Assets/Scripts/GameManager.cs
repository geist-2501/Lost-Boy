using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private DialogueManager dialogueManager;

	#region dialogue
	[SerializeField] Dialogue intro;
	#endregion

	#region objects
	[SerializeField] private Text fileIdText;
	#endregion

	private string targetFileID;
	private bool targetFileFound;
	

	public void SetTargetFileID(string _id) {
		targetFileID = _id;
		fileIdText.text = targetFileID;
	}

	void Start(){
		dialogueManager = GetComponent<DialogueManager>();
		dialogueManager.LoadDialogue(intro);
	}



}
