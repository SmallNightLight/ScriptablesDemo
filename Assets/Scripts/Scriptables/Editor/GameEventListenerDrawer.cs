using ScriptableArchitecture.Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventListenerBase<>))]
public class GameEventListenerDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();

        // You can add custom GUI elements or modify the existing ones here if needed
    }
}