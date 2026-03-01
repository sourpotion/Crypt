using System;
using UnityEngine;

public class GameMangeren : MonoBehaviour
{
    public static GameMangeren Instance { get; private set; } //make it global

    [HideInInspector] public bool audioSaveAlreadyDone = false; //so it not loading again
    [HideInInspector] public bool gameIsPause; // of the game is paused
    [HideInInspector] public Action<Vector3, float> onMakingNoise; //send to everybody who want hear

    void Awake()
    {
        if (Instance == null) //so it don't dupe kk
        {
            Instance = this;                   // assign the singleton
            DontDestroyOnLoad(gameObject);     // make it persistent across scenes
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
}
