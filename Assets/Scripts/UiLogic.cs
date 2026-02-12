using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLogic : MonoBehaviour
{
    [Header("Must")]
    public GameObject startSchrem; // Where it Start
    public GameObject loadingScreenUi; //frame of the ui
    public Slider progessBar; // slider of the progessBar

    private GameObject currentUi; 
    private String startSceneName = "SampleScene"; // here go the name of the startScene

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUi = startSchrem;
        currentUi.SetActive(true);

        if (currentUi == null) { Debug.LogWarning("u need to do a public startSchrem AUB"); } //{} is more readable* 
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void StartGame()
    {
        currentUi.SetActive(false); //so u don't see this
        StartCoroutine(LoadScene(startSceneName)); //loadingScreen need wait
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
