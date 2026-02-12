using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiLogic : MonoBehaviour
{
    [Header("Settings")]
    public GameObject startSchrem; // Where it Start

    private GameObject currentUi; 
    private String startSceneName = "SampleScene"; // here go the name of the startScene

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUi = startSchrem;

        if (currentUi == null) { Debug.LogWarning("u need to do a public startSchrem AUB"); } //{} is more readable* 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        print("The update of later sceneLoader come soon here");

        SceneManager.LoadScene(startSceneName);
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
