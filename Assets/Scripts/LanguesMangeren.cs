using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.WSA;

public class LanguesMangeren : MonoBehaviour
{
    public static GameMangeren Instance { get; private set; } //make it global
    [HideInInspector] public Action<string> reloadLangues;

    private string defaultLangue = "English";
    private string currentLangue = "";
    private string path;
    private Dictionary<string, string> langue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLangue = defaultLangue;

        //path = Path.Combine(Application.streamingAssetsPath, "Langues/" + currentLangue + ".json");
        string jsonText = File.ReadAllText(path);
    }

    public string GetText(string textId)
    {
        return textId;
    }
}
