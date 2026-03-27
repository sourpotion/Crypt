using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMangeren : MonoBehaviour
{
    public static GameMangeren Instance { get; private set; } //make it global

    [HideInInspector] public bool audioSaveAlreadyDone = false; //so it not loading again
    [HideInInspector] public bool gameIsPause; // of the game is paused
    [HideInInspector] public Action<Vector3, float> onMakingNoise; //send to everybody who want hear
    [HideInInspector] public bool plrDied = false;
    [HideInInspector] public bool plrHiding = false;
    [HideInInspector] public bool plrStun = false;

    //saving things
    [System.Serializable]
    private class humBodyInfo
    {
        public string name;
        public bool isBroken;
    }

    [Header("Debug")]

    private Vector3 plrPos;
    private humBodyInfo[] bodysInfo = new humBodyInfo[6];
    [SerializeField] private GameObject plr;
    private string plrTag = "Player";
    [SerializeField] private HumanoidBody humBody;

    void Awake()
    {
        if (Instance == null) //so it don't dupe kk
        {
            Instance = this;                   // assign the singleton
            DontDestroyOnLoad(gameObject);     // make it persistent across scenes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);               // prevent duplicates
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!plr)
        {
            plr = GameObject.FindGameObjectWithTag(plrTag);

            if (plr != null) {humBody = plr.GetComponent<HumanoidBody>();}

        }
    }

    public void PlrDied()
    {
        //send gameover events
        GameOverUi.Instance.PlrDied();
        EnemieHolder.Instance.EnableEnemie(false);
        EnemieHolder.Instance.ResetEnemie();
        
        //make the mouse visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //saving
    //what to save brokenParts, inv, plr pos

    public void SaveGameFile()
    {
        print(humBody);

        //save
        plrPos = plr.transform.position;
        
        for (int i = 0; i < 6; i++)
        {
            var currentInfo = humBody.GetInfo(i);

            bodysInfo[i] = new humBodyInfo();

            bodysInfo[i].name = currentInfo.name;
            bodysInfo[i].isBroken = currentInfo.isBroken;
        }
    }

    public void LoadGameFile()
    {
        print(plrPos);

        //load
        plr.transform.position = plrPos;
        plrStun = false;

        //loop and set all one for one good
        foreach (humBodyInfo bodyInfo in bodysInfo)
        {
            if (bodyInfo.isBroken) {humBody.BreakPart(bodyInfo.name);}
            else {humBody.HealPart(bodyInfo.name);}
        }
    }

    public void ResetGameFile()
    {
        plrPos = Vector3.zero;
        bodysInfo = new humBodyInfo[6];
    }
}
