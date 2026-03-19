using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool GetExit = false;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (GetExit)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}

