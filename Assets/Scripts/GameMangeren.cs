using UnityEngine;

public class GameMangeren : MonoBehaviour
{
    public static GameMangeren Instance { get; private set; } //make it global

    public bool audioSaveAlreadyDone = false;

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
