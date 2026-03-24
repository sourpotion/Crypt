using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameMangeren))]
public class GameMangerEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameMangeren gameMangeren = (GameMangeren)target;

        GUILayout.Space(10);

        GUILayout.Label("GameControll", EditorStyles.boldLabel);

        if (GUILayout.Button("Save"))
        {
            gameMangeren.SaveGameFile();
        }

        if (GUILayout.Button("Toggle Hide"))
        {
            gameMangeren.plrHiding = !gameMangeren.plrHiding;
        }
    }
}
