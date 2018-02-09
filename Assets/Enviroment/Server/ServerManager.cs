using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ServerManager : MonoBehaviour {

	[SerializeField] private Text listBox;
	[SerializeField] private HackPuzzle hackPuzzle;

	private string[] linesOfIDs = new string[100];
	private string[] formattedLinesOfIDs = new string[100]; //without dash.
	private int targetFileIndex = 0; //Index of 'file' that the player has to find.

	private Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();

		string idsFilePath = "Assets/Enviroment/Server/fileIDs.txt";

		linesOfIDs = File.ReadAllLines(idsFilePath);

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

		targetFileIndex = Random.Range(0, linesOfIDs.Length - 1);
		Debug.Log(targetFileIndex);
	}

	private void Swap <T> (ref T a, ref T b) {
		T temp = a;
		a = b;
		b = temp;
	}

	public int GetTargetFileIndex() {
		return targetFileIndex;
	}

	public void OnEnterSearchKey(string _searchKey) {

		anim.SetBool("TargetFileFound", false);

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

		int _indexOfFileSearched = BinarySearch(_searchKey.Remove(3, 1));

		if (_indexOfFileSearched == -1) {
			Debug.Log("no such file.");
			anim.SetTrigger("NoFileFound");
			return;
		}

		Debug.Log("found file: " + linesOfIDs[_indexOfFileSearched]);

		if (_indexOfFileSearched == targetFileIndex) {
			anim.SetBool("TargetFileFound", true);
			Debug.Log("Tah dah! You've found the file!");
		}



	}

	private int BinarySearch(string _searchKey) {

		bool found = false;
		int startPos = 0;
		int endPos = formattedLinesOfIDs.Length - 1;
		int midPos = 0;

		//Using string.Compare(strA, strB);
		//Less than zero, strA precedes strB in the sort order.
		//Zero, strA occurs in the same position as strB in the sort order.
		//Greater than zero, strA follows strB in the sort order.

		while (found == false && startPos < endPos) {
			
			midPos = (startPos + endPos) / 2;

			Debug.Log("M: " + midPos + ". S: " + startPos + ". E: " + endPos);

			if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) < 0) {
				startPos = midPos + 1;
			} else if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) > 0) {
				endPos = midPos - 1;
			}

			if (endPos == startPos + 1) {
				//I.e if they are next to each other, and the middle becomes a decimal.
				if (string.Compare(formattedLinesOfIDs[startPos], _searchKey) == 0) {
					found = true;
					return startPos;
				} else if (string.Compare(formattedLinesOfIDs[endPos], _searchKey) == 0) {
					found = true;
					return endPos;
				}
			}
			

			if (string.Compare(formattedLinesOfIDs[midPos], _searchKey) == 0) {
				found = true;
				return midPos;
			}
		}

		//-1 is an invalid array position, and can be used to say that nothing was found.
		return -1;
		
	}

	//Just lets the animator call the minigame.
	void BeginHack() {
		hackPuzzle.CreateGrid();
	}

	//Hack puzzle calls this once its done.
	public void EndHack(bool _wasSuccessful){
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
