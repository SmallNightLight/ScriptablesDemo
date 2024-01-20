using UnityEngine;
using UnityEditor;
using ScriptableArchitecture.Core;
using System.Reflection;
using System;
using System.Linq;

namespace ScriptableArchitecture.EditorScript
{
    [CustomEditor(typeof(Variable<>), true)]
    [CanEditMultipleObjects]
    public class VariableEditor : Editor
    {
        private Variable _target;

        private bool _canShowVariable;
        private bool _canShowGameEvent;
        private bool _showVariable;
        private bool _showGameEvent;

        private bool _showStacktrace;

        //Variable
        private SerializedProperty _valueProperty;
        private SerializedProperty _initializeTypeProperty;
        private SerializedProperty _startValueProperty;

        //Event
        private bool _showDebugEvent;
        private bool _showListeners;
        private bool[] _showSubStackTraces = new bool[3];
        private Vector2[] _scrollStacktraces = new Vector2[3];
        private SerializedProperty _debugValueProperty;
        private SerializedProperty _variableTypeProperty;
        private MethodInfo _raiseMethod;

        private void OnEnable()
        {
            _target = target as Variable;

            //Types
            _variableTypeProperty = serializedObject.FindProperty("VariableType");
            _initializeTypeProperty = serializedObject.FindProperty("InitializeType");

            //Variable
            _valueProperty = serializedObject.FindProperty("_value");
            _startValueProperty = serializedObject.FindProperty("_startValue");

            //Event
            _debugValueProperty = serializedObject.FindProperty("DebugValue");
            _raiseMethod = target.GetType().BaseType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetParameters().Length == 1).ToArray().FirstOrDefault();

            ResetFoldouts();
        }

        public override void OnInspectorGUI()
        {
            if (target == null) return;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_variableTypeProperty);

            if (EditorGUI.EndChangeCheck())
                ResetFoldouts();

            if (GUI.changed)
                EditorUtility.SetDirty(_target);

            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            if (_canShowVariable)
                DrawVariableEditor();

            EditorGUILayout.Space();
            if (_canShowGameEvent)
                DrawGameEventEditor();

            EditorGUILayout.Space();
            DrawStacktrace();
        }

        public void DrawVariableEditor()
        {
            _showVariable = EditorGUILayout.Foldout(_showVariable, "Variable", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

            if (_showVariable)
            {
                EditorGUI.indentLevel++;
                bool inPlaymode = EditorApplication.isPlaying;

                VariableType variableType = (VariableType)_variableTypeProperty.enumValueIndex;
                InitializeType initializeType = (InitializeType)_initializeTypeProperty.enumValueIndex;

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_initializeTypeProperty);

                if (variableType == VariableType.Variable || variableType == VariableType.VariableEvent)
                {
                    if (inPlaymode && initializeType != InitializeType.ReadOnly)
                        EditorGUILayout.PropertyField(_valueProperty, true);

                    if (initializeType == InitializeType.ResetOnGameStart || initializeType == InitializeType.ReadOnly)
                        EditorGUILayout.PropertyField(_startValueProperty, true);
                }

                serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel--;
            }
        }

        public void DrawGameEventEditor()
        {
            _showGameEvent = EditorGUILayout.Foldout(_showGameEvent, "Game Event", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

            if (_showGameEvent)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;

                _showDebugEvent = EditorGUILayout.Foldout(_showDebugEvent, "Debug Event", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

                if (_showDebugEvent)
                {
                    EditorGUILayout.PropertyField(_debugValueProperty);

                    serializedObject.ApplyModifiedProperties();

                    if (GUILayout.Button("Raise Event"))
                    {
                        Type targetType = _debugValueProperty.serializedObject.targetObject.GetType();
                        FieldInfo targetField = targetType.GetField("DebugValue", BindingFlags.Instance | BindingFlags.Public);
                        object debugValue = targetField.GetValue(_debugValueProperty.serializedObject.targetObject);
                        _raiseMethod.Invoke(target, new object[1] { debugValue });
                    }
                }

                EditorGUILayout.EndVertical();

                GameEventEditorHelper.DrawListeners(target as IGameEvent, ref _showListeners);
                EditorGUI.indentLevel--;
            }
        }

        private void DrawStacktrace()
        {
            _showStacktrace = EditorGUILayout.Foldout(_showStacktrace, "Stacktrace", true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });

            if (_showStacktrace)
            {
                EditorGUI.indentLevel++;

                Stacktrace[] stacktraces = (target as IGameEvent).GetStackTraces();
                
                for (int i = 0; i < stacktraces.Length; i++)
                {
                    Stacktrace stacktrace = stacktraces[i];

                    VariableType type = stacktrace.GetStackType();

                    bool drawAsVariable = _canShowVariable && (type == VariableType.Variable || type == VariableType.VariableEvent);
                    bool drawAsEvent = _canShowGameEvent && (type == VariableType.Event || type == VariableType.VariableEvent);

                    if (drawAsVariable || drawAsEvent)
                        GameEventEditorHelper.DrawStackTrace(stacktrace, ref _showSubStackTraces[i], ref _scrollStacktraces[i]);
                }

                EditorGUI.indentLevel--;
            }
        }

        private void ResetFoldouts()
        {
            _canShowVariable = (VariableType)_variableTypeProperty.enumValueIndex == VariableType.Variable || (VariableType)_variableTypeProperty.enumValueIndex == VariableType.VariableEvent;
            _canShowGameEvent = (VariableType)_variableTypeProperty.enumValueIndex == VariableType.Event || (VariableType)_variableTypeProperty.enumValueIndex == VariableType.VariableEvent;
            _showVariable = _canShowVariable;
            _showGameEvent = _canShowGameEvent;
        }
    }
}