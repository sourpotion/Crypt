using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
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

    [System.Serializable]
    public class Audios
    {
        public string audioName; //name of the exposed parameter of the audio 
        public Slider slider; //slider that say the value of the sound
    }

    [System.Serializable]
    public class KeyRebinding
    {
        public string actionName; //binding of the keybind like pause or jump
        public TextMeshProUGUI text; //text to change when using the rebindFunction
        public Button buttonToActive; //active button
    }

    public Audios[] audios;
    public KeyRebinding[] keyRebinding; //so it can be save and loaded

    private GameObject currentUi;
    private float maxVolume = 20;
    private float minVolume = -80;
    private PlayerController plrControl;

    //saving for old setting in pause
    private CursorLockMode oldCursorLockMode; //so when in puzzle mabye then i don't have to check and fix that th emouse is not gona first person
    private bool oldCursorVisible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        plrControl = new PlayerController();

        OnEnable();
    }

    void Start()
    {
        currentUi = startSchrem;
        GameMangeren.Instance.gameIsPause = false;

        foreach (Audios audioInfo in audios)
        {
            audioInfo.slider.onValueChanged.AddListener((value) => { ChangeSound(audioInfo.slider.value, audioInfo.audioName); }); //add the functionEvent to the slider
        }

        foreach (KeyRebinding info in keyRebinding)
        {
            info.buttonToActive.onClick.AddListener(() => { RebindKey(info); }); //but the action of rebind into the button
        }

        Reset(); //resetTheAudios and the keybinds

        if (currentUi == null) { Debug.LogWarning("u need to do a public startSchrem AUB"); } //{} is more readable* 
    }

    // Update is called once per frame
    void Update()
    {

    }

    //on press

    void OnEnable()  //little ai but it make it enable
    {
        plrControl.Enable();
        plrControl.Player.Pause.performed += OnPauseButton;
    }

    void OnDisable() //disable key
    {
        plrControl.Player.Pause.performed -= OnPauseButton;
        plrControl.Disable();
    }

    private void OnPauseButton(InputAction.CallbackContext context)
    {
        if (inGame)
        {
            TogglePauseGame();
        }
    }

    //functions

    public void ChangeSound(float newValue, string soundName)
    {
        float newVolume = Mathf.Lerp(minVolume, maxVolume, newValue);

        audioMixer.SetFloat(soundName, newVolume);
    }

    public void TogglePauseGame()
    {
        currentUi.SetActive(false);

        //pause the game and unlokc the mouse
        if (!GameMangeren.Instance.gameIsPause)
        {
            //pause the game
            Time.timeScale = 0;

            //save the oldMouse settings
            oldCursorLockMode = Cursor.lockState;
            oldCursorVisible = Cursor.visible;

            //make the mouse visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // pause the game
            Time.timeScale = 1;

            //load the old mouse settings
            Cursor.lockState = oldCursorLockMode;
            Cursor.visible = oldCursorVisible;
        }

        startSchrem.SetActive(!GameMangeren.Instance.gameIsPause); // it can just deactive a deavtive ui*
        currentUi = startSchrem;

        GameMangeren.Instance.gameIsPause = !GameMangeren.Instance.gameIsPause; //toggle this
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

    public void SwitchScene(string sceneToLoadName)
    {
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

    void RebindKey(KeyRebinding info)
    {
        InputAction action = plrControl.asset.FindAction(info.actionName);
        info.text.text = "...";

        // Disable action before rebinding
        action.Disable();

        action.PerformInteractiveRebinding()
            .OnComplete(operation => //wait for awner
            {
                operation.Dispose(); //make the keybind true
                action.Enable();

                // put the text to the new key
                string keybindName = action.bindings[0].ToDisplayString(); //get the string of the new key
                info.text.text = keybindName;

                //save it
                string bindingPath = action.bindings[0].effectivePath;
                PlayerPrefs.SetString(info.actionName + "Keybind", bindingPath);
                PlayerPrefs.Save();
            })
            .Start();

    }

    //saving and loading

    void Reset()
    {
        if (!GameMangeren.Instance.audioSaveAlreadyDone) //if not loaded then load 
        {
            //load everything
            foreach (Audios audiosInfo in audios)
            {
                float newValue = LoadAudioVolume(audiosInfo.audioName + "Audio"); //load the current value of audio
                audiosInfo.slider.value = newValue;
                ChangeSound(newValue, audiosInfo.audioName);   //changeItInAudio
            }

            foreach (KeyRebinding info in keyRebinding)
            {
                var action = plrControl.asset.FindAction(info.actionName);
                string keybindPath = LoadKeyBinds(info.actionName + "Keybind");

                if (!string.IsNullOrEmpty(keybindPath))
                {
                    action.ApplyBindingOverride(0, keybindPath); //pput the newOne in here
                }

                info.text.text = action.bindings[0].ToDisplayString(); //show the current keybind 
            }

            GameMangeren.Instance.audioSaveAlreadyDone = true; //debounce
        }
        else
        {
            foreach (Audios audiosInfo in audios) //but the slider good
            {
                float currentVolume;
                if (!audioMixer.GetFloat(audiosInfo.audioName, out currentVolume)) { currentVolume = 0f; } //get the volume of the audio
                float newValue = Mathf.InverseLerp(minVolume, maxVolume, currentVolume); //reverse to get value

                audiosInfo.slider.value = newValue;
            }

            foreach (KeyRebinding info in keyRebinding)
            {
                var action = plrControl.asset.FindAction(info.actionName);

                info.text.text = action.bindings[0].ToDisplayString(); //show the current keybind 
            }
        }
    }

    void OnApplicationQuit()
    {
        foreach (Audios audiosInfo in audios)
        {
            SaveAudioVolume(audiosInfo.audioName + "Audio", audiosInfo.slider.value);
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

    string LoadKeyBinds(string key) //key == nameOfTheAction + Keybind
    {
        return PlayerPrefs.GetString(key, "");
    }

}
