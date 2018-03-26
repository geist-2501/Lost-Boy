using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Blair Cross
This script is attached to the little exlemation mark
that appears over enemies head when they see you.
The functions here are just to let the animator call them.
So the alert pops up, the animator calls PlaySound(), 
and at the end it calls DeleteAlert().
*/


public class Alert : MonoBehaviour {

	//Sound to play when the alert is generated.
	[SerializeField] private AudioClip alerted;

	private void Start() {
	}

	private void PlaySound () {
		AudioSource.PlayClipAtPoint(alerted, transform.position);
	}

	private void DeleteAlert () {
		Destroy(gameObject);
	}
}
