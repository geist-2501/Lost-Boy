using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour {

	private GameManager gameManager;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Player")
		{
			gameManager.PlayerLeavingLevel();
		}
	}
}
