using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void loadScene(string _scene) {
		 SceneManager.LoadScene(_scene);
	}

	public void RequestQuit () {
		Application.Quit();
	}

}
