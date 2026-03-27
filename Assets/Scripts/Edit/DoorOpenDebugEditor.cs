using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoorOpenWay))]
public class DoorOpenDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DoorOpenWay door = (DoorOpenWay)target;

        if (GUILayout.Button("Toggle Debug"))
        {
            door.ToggleDebugState();
        }
    }
}
