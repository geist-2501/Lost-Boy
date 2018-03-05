using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	#region objects
	[SerializeField] private Text fileIdText;
	#endregion

	private string targetFileID;
	private bool targetFileFound;

	public void SetTargetFileID(string _id) {
		targetFileID = _id;
		fileIdText.text = targetFileID;
	}



}
