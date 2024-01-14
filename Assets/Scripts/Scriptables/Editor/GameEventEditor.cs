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
        private bool _showListeners;
        private bool _showStackTrace;
        private Vector2 _scrollStacktrace;
       
        private void OnEnable()
        {
            _target = (GameEventBase)target;
        }

        public override void OnInspectorGUI()
        {
            //Draw debug options
            EditorGUILayout.Space();
            serializedObject.Update();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Debug Event", EditorStyles.boldLabel);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

            if (GUILayout.Button("Raise Event"))
                _target.Raise();

            EditorGUILayout.EndVertical();

            GameEventEditorHelper.DrawListeners(target as IGameEvent, ref _showListeners);
            GameEventEditorHelper.DrawStackTrace(target as IGameEvent, ref _showStackTrace, ref _scrollStacktrace);
        }
    }

    [CustomEditor(typeof(GameEventBase<>), true)]
    public class GameEventEditorGeneric : Editor
    {
        private SerializedProperty _debugValueProperty;
        private MethodInfo _raiseMethod;

        private bool _showListeners;
        private bool _showStackTrace;
        private Vector2 _scrollStacktrace;

        private void OnEnable()
        {
            _debugValueProperty = serializedObject.FindProperty("DefaultValue");
            _raiseMethod = target.GetType().BaseType.GetMethod("Raise", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Debug Options", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_debugValueProperty);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Raise Event"))
            {
                Type targetType = _debugValueProperty.serializedObject.targetObject.GetType();
                FieldInfo targetField = targetType.GetField("DefaultValue", BindingFlags.Instance | BindingFlags.Public);
                object debugValue = targetField.GetValue(_debugValueProperty.serializedObject.targetObject);
                _raiseMethod.Invoke(target, new object[1] { debugValue });
            }

            EditorGUILayout.EndVertical();

            GameEventEditorHelper.DrawListeners(target as IGameEvent, ref _showListeners);
            GameEventEditorHelper.DrawStackTrace(target as IGameEvent, ref _showStackTrace, ref _scrollStacktrace);
        }
    }
}