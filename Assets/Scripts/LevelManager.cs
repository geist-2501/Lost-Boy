using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    static string nextLevelToLoad = "";

    private bool loadingScene = false;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        nextLevelToLoad = "";

        Debug.Log("I've started!");

        //Scene indicies;
        //0 - Splash screen.
        //1 - Loading screen.

        //If on splash screen.
        if (currentSceneIndex == 0)
        {
            //Wait 3 seconds then load main menu.
            StartCoroutine(LoadSceneDelay("Start", 3));
        }
    }

    void Awake()
    {
        if (nextLevelToLoad != "")
        {
            StartCoroutine(LoadSceneDelay(nextLevelToLoad, 0.5f));
        }
    }

    //Buttons can call this to quit.
    public void RequestQuit()
    {
        Application.Quit();
    }

    //Loads a scene without a loading screen.
    public void LoadSceneBasic(string _scene)
    {
        if (!loadingScene)
        {
            loadingScene = true; //Prevent another scene from being loaded.
            StartCoroutine(LoadSceneAsync(_scene));
        }
    }

    //Loads a scene with a loading screen.
    public void LoadSceneAdvanced(string _scene)
    {
        nextLevelToLoad = _scene;
        LoadSceneBasic("Loading");
    }

    //Used by the above functions to load a level without performance drops.
    IEnumerator LoadSceneAsync(string _scene)
    {
        //This will load a new level in the background.
        //Doing this instead of using LoadScene() prevents jittering,
        //As it waits for every to be done before switching.
        AsyncOperation async = SceneManager.LoadSceneAsync(_scene);
        while (!async.isDone)
        {
            yield return null;
        }

        loadingScene = false;
    }

    //Loads a scene after _delay seconds.
    IEnumerator LoadSceneDelay(string _scene, float _delay)
    {
        //Wait for _delay seconds.
        yield return new WaitForSeconds(_delay);
        //Then load the scene.
        LoadSceneBasic(_scene);
    }

}
