using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour {

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
