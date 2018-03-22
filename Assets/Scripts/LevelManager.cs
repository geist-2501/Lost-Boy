using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private bool loadingScene = false;

    //If the level designer wants to automatically load into a different level, 
    //they will put the name of it here.
    //Otherwise if left black, nothing will happen.
    [SerializeField] private string autoLoad = "";

    public void loadScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void RequestQuit()
    {
        Application.Quit();
    }

    void Start()
    {
        if (autoLoad != "")
        {
            if (Application.CanStreamedLevelBeLoaded(autoLoad))
            {
                LoadSceneAdvanced(autoLoad);
            }
            else
            {
                Debug.LogError("No such scene: " + autoLoad);
            }
        }
    }

    public void LoadSceneAdvanced(string _scene)
    {
        if (!loadingScene)
        {
            loadingScene = true; //Prevent another scene from being loaded.
            StartCoroutine(LoadSceneAsync(_scene));
        }
    }

    IEnumerator LoadSceneAsync(string _scene)
    {
        //This will load a new level in the background.
        //Doing this instead of using LoadScene() prevents jittering
        AsyncOperation async = SceneManager.LoadSceneAsync(_scene);
        while (!async.isDone)
        {
            yield return null;
        }
    }

}
