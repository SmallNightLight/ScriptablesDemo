using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using ScriptableArchitecture.Core;

namespace ScriptableArchitecture.EditorScript
{
    [CustomEditor(typeof(GameEventBase), true)]
    public class GameEventEditor : Editor
    {
        private GameEventBase _target;
        private bool _showDebugEvent;
        private bool _showListeners;
        private bool[] _showStackTrace = new bool[1];
        private Vector2 _scrollStacktrace;
       
        private void OnEnable()
        {
            _target = (GameEventBase)target;

            _showDebugEvent = true;
        }

        public override void OnInspectorGUI()
        {
            //Draw debug options
            EditorGUILayout.Space();
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;

            _showDebugEvent = EditorGUILayout.Foldout(_showDebugEvent, "Game Event", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

            if (_showDebugEvent)
            {
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.Space();

                if (GUILayout.Button("Raise Event"))
                    _target.Raise();
            }

            EditorGUILayout.EndVertical();
            GameEventEditorHelper.DrawListeners(target as IGameEvent, ref _showListeners);
            EditorGUI.indentLevel--;
            //GameEventEditorHelper.DrawStackTrace(target as IGameEvent, ref _showStackTrace, ref _scrollStacktrace);
        }
    }
}