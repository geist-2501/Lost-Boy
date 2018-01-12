using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ServerManager : MonoBehaviour {

	[SerializeField] Text listBox;

	private string[] linesOfIDs = new string[100];
	private string[] formattedLinesOfIDs = new string[100]; //without dash.

	private Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();

		string filePath = "Assets/Enviroment/Server/fileIDs.txt";

		linesOfIDs = File.ReadAllLines(filePath);

		for (int i = 0; i < linesOfIDs.Length; i++) {
			formattedLinesOfIDs[i] = linesOfIDs[i].Remove(3, 1); //removing dash.
		}

		//Bubble sort.
		//TODO move this to game startup and order the actual textfile instead.
		for (int outerLoop = formattedLinesOfIDs.Length - 2; outerLoop > 0; outerLoop--) {
			for (int i = 0; i < outerLoop; i++) {
				if (string.Compare(formattedLinesOfIDs[i], formattedLinesOfIDs[i+1]) > 0) {
					Swap(ref formattedLinesOfIDs[i], ref formattedLinesOfIDs[i + 1]);
					Swap(ref linesOfIDs[i], ref linesOfIDs[i + 1]);
				}
			}
		}

		for (int i = 0; i < linesOfIDs.Length; i++) {
			listBox.text += linesOfIDs[i] + "\n";
		}
	}

	private void Swap <T> (ref T a, ref T b) {
		T temp = a;
		a = b;
		b = temp;
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

		BinarySearch(_searchKey.Remove(3,1));
	}

	private void BinarySearch(string _searchKey) {

		bool found = false;
		int startPos = 0;
		int endPos = formattedLinesOfIDs.Length;
		int midPos = 0;

		//Using string.Compare(strA, strB);
		//Less than zero, strA precedes strB in the sort order.
		//Zero, strA occurs in the same position as strB in the sort order.
		//Greater than zero, strA follows strB in the sort order.

		while (found == false && startPos < endPos) {
			midPos = (startPos + endPos) / 2;
			if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) < 0) {
				startPos = midPos + 1;
			} else if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) > 0) {
				endPos = midPos - 1;
			}

			if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) == 0) {
				found = true;
				Debug.Log("found file: " + linesOfIDs[midPos]);
			}
		}

		if (found == false) {
			Debug.Log("no such file."); 
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
