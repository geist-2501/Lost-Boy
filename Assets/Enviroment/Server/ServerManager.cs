using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ServerManager : MonoBehaviour {

	[SerializeField] Text listBox;

	private string[] linesOfIDs = new string[100];
	private int[] formattedLinesOfIDs = new int[100];

	private Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();

		string filePath = "Assets/Enviroment/Server/fileIDs.txt";

		linesOfIDs = File.ReadAllLines(filePath);
		for (int i = 0; i < linesOfIDs.Length; i++) {
			//from 123-4AB to 123, Just the first three ordered numbers as they are ing order.
			int.TryParse(linesOfIDs[i].Remove(3, 4), out formattedLinesOfIDs[i]);
			listBox.text += linesOfIDs[i] + "\n";
		}
	}
	
	public void OnEnterSearchKey(string _searchKey) {

		//Valid search key takes the form of III-ICC, where I is an integer and C is a char.

		//Check if right length.
		if (_searchKey.Length != 7) {
			anim.SetTrigger("InvalidID");
			return;
		}


		char[] letters = new char[7];
		letters = _searchKey.ToCharArray();

		//Now go through and make sure the keys takes the form III-ICC, where I is an integer and C is a char.

		bool firstHalfValid = char.IsNumber(letters[0]) && char.IsNumber(letters[1]) && char.IsNumber(letters[2]);
		bool dashValid = letters[3].ToString() == "-";
		bool secondHalfValid = char.IsNumber(letters[4]) && char.IsLetter(letters[5]) && char.IsLetter(letters[6]);

		if (firstHalfValid && dashValid && secondHalfValid) {
			Debug.Log(_searchKey + "is valid!");
		} else {
			Debug.Log(_searchKey + "isn't valid!");
			anim.SetTrigger("InvalidID");
			return;
		}


		//TODO binary search.
	}

	private void BinarySearch(string _searchKey) {

		int formattedKey;
		int.TryParse(_searchKey.Remove(3, 4), out formattedKey);

		bool found = false;
		int startPos = 0;
		int endPos = formattedLinesOfIDs.Length;
		int midPos = 0;

		while (found == false && startPos < endPos) {
			midPos = (startPos + endPos) / 2;
			if (formattedLinesOfIDs[midPos] < formattedKey) {
				startPos = midPos + 1;
			}
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
