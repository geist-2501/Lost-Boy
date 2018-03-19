using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Original framework by Brackeys - https://www.youtube.com/channel/UCYbK_tjZ2OrIZFBvU6CCMiA
Expanded upon using the scriptable object system by Blair Cross.
Controls the dialogue being sent to the screen.
 */

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private Animator anim;

    [Range(0, 0.1f)]
    [SerializeField] private float displaySpeed = 0.01f;

    [SerializeField] private Text nameText;
    [SerializeField] private Text bodyText;
    [SerializeField] private Text contText;

    private Queue<string> sentences;

    // Use this for initialization
    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void LoadDialogue(Dialogue _newDialogue)
    {
        Debug.Log("Starting dialogue with " + _newDialogue.npcName + ", from " + _newDialogue.name);
        
        anim.SetBool("DialogueActive", true);

        nameText.text = _newDialogue.npcName;

        sentences.Clear();

        foreach (string sentence in _newDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

		DisplayNextSentence();
    }

	public void DisplayNextSentence()
	{

        StopAllCoroutines();
        
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

        string _currentSentence = sentences.Dequeue();
        Debug.Log(_currentSentence);

        StartCoroutine(TypeSentenceOut(_currentSentence));
	}

    IEnumerator TypeSentenceOut (string _sentence)
    {
        contText.gameObject.SetActive(false);
        bodyText.text = "";
        foreach (char letter in _sentence.ToCharArray())
        {
            bodyText.text += letter;
            yield return new WaitForSecondsRealtime(displaySpeed);
        }
        contText.gameObject.SetActive(true);
    }

    void EndDialogue() 
    {
        anim.SetBool("DialogueActive", false);
        Debug.Log("End of convo");
        nameText.text = "";
        bodyText.text = "";
    }
}
