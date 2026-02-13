using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLogic : MonoBehaviour
{
    // i thing when using remeber settings and quitToMenu is diffent in startschrem scene and RealGameScene kk else i needed to check or it is in game

    [Header("Must")]
    public GameObject startSchrem; // Where it Start or where the PauseScreenStart (;
    public GameObject loadingScreenUi; //frame of the ui
    public Slider progessBar; // slider of the progessBar
    public AudioMixer audioMixer;

    //[Header("Could")]

    [Header("Settings")]
    public bool inGame;

    [Serializable]
    public class Audios
    {
        public string soundEffectName;
        public Slider slider;
    } 
    public Audios[] audios; 

    private GameObject currentUi; 
    private String[] sceneNames = {"SampleScene", "SampleScene"}; // id of the scene first is 0 sconf is 1 so on
    private bool gameIsPause = false; //so when it is pause with esc u can go out
    private float maxVolume = 20;
    private float minVolume = -80;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUi = startSchrem;

        foreach (Audios audioInfo in audios)
        {
            audioInfo.slider.onValueChanged.AddListener((value) => {ChangeSound(audioInfo.slider.value, audioInfo.soundEffectName);}); //add the functionEvent to the slider
        }

        Reset(); //resetTheAudios

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

    public void ChangeSound(float newValue, string soundName)
    {
        float newVolume = Mathf.Lerp(minVolume, maxVolume, newValue);
        print(soundName);

        audioMixer.SetFloat(soundName, newVolume);
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

    //saving

    void Reset()
    {
        if (!GameMangeren.Instance.audioSaveAlreadyDone) //if not loaded then load 
        {
            foreach (Audios audiosInfo in audios)
            {
                float newValue = LoadAudioVolume(audiosInfo.soundEffectName + "Audio"); //load the current value of audio
                audiosInfo.slider.value = newValue;
                ChangeSound(newValue, audiosInfo.soundEffectName);   //changeItInAudio
            }

            GameMangeren.Instance.audioSaveAlreadyDone = true; //debounce
        }
        else
        {
            foreach (Audios audiosInfo in audios) //but the slider good
            {
                float currentVolume;
                if (!audioMixer.GetFloat(audiosInfo.soundEffectName, out currentVolume)) {currentVolume = 0f;} //get the volume of the audio
                float newValue = Mathf.InverseLerp(minVolume, maxVolume, currentVolume); //reverse to get value

                audiosInfo.slider.value = newValue;
            }
        }
    } 

    void OnApplicationQuit()
    {
        foreach (Audios audiosInfo in audios)
        {
            SaveAudioVolume(audiosInfo.soundEffectName + "Audio", audiosInfo.slider.value);
        }
    }

    void SaveAudioVolume(string key, float volume)
    {
        PlayerPrefs.SetFloat(key, volume);
    }

    float LoadAudioVolume(string key) //key = nameOfTheparameter + Audio
    {
        
        return PlayerPrefs.GetFloat(key, 1f);
    }
}
