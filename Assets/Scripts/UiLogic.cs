using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLogic : MonoBehaviour
{
    // i thing when using remeber settings and quitToMenu is diffent in startschrem scene and RealGameScene kk else i needed to check or it is in game

    [Header("Must")]
    public GameObject startSchrem; // Where it Start or where the PauseScreenStart (;
    public GameObject loadingScreenUi; //frame of the ui
    public Slider progessBar; // slider of the progessBar

    //[Header("Could")]

    [Header("Settings")]
    public bool inGame;

    private GameObject currentUi; 
    private String[] sceneNames = {"SampleScene", "SampleScene"}; // id of the scene first is 0 sconf is 1 so on
    private bool gameIsPause = false; //so when it is pause with esc u can go out

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUi = startSchrem;

        if (currentUi == null) { Debug.LogWarning("u need to do a public startSchrem AUB"); } //{} is more readable* 
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {TogglePauseGame();}
        }
    }

    public void TogglePauseGame()
    {
        currentUi.SetActive(false);

        if (!gameIsPause) {Time.timeScale = 0;} else {Time.timeScale = 1;} //pause the game*
        startSchrem.SetActive(!gameIsPause); // it can just deactive a deavtive ui*
        currentUi = startSchrem;

        gameIsPause = !gameIsPause; //toggle this
    }

    IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        loadingScreenUi.SetActive(true);

        while (!operation.isDone)
        {
            float progess = progessBar.value;
            progessBar.value = progess;

            yield return null;
        }
    }

    public void SwitchScene(int id)
    {
        string sceneToLoadName = sceneNames[id]; //so i don't need 2 funtion /:

        currentUi.SetActive(false); //so u don't see this
        StartCoroutine(LoadScene(sceneToLoadName)); //loadingScreen need wait
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //turn off when in testingMode
        #endif

        Application.Quit(); // quit the game...
    }

    public void SwitchUi(GameObject newUi)
    {
        newUi.SetActive(true); //turn on the new ui
        currentUi.SetActive(false); //turn off the old one

        currentUi = newUi; //make the old one the new one
    }
}
