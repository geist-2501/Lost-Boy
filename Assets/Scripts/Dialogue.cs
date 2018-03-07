using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogue")]
[System.Serializable]
public class Dialogue : ScriptableObject{
	public string npcName;
	[TextArea(3, 10)]
	public string[] sentences;
}
