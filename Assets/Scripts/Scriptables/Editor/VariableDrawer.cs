using ScriptableArchitecture.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableArchitecture.EditorScript
{
    [CustomPropertyDrawer(typeof(Variable<>), true)]
    public class VariableDrawer : PropertyDrawer
    {
        private bool _foldoutOpen;
        private bool _expandedValue;
        private bool _expandedStartValue;
        private float _height = 18f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (position.width == 1f && position.height == 1f) return;

            EditorGUI.BeginProperty(position, label, property);

            _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (property.boxedValue != null)
            {
                Rect variableRect = new Rect(EditorGUIUtility.labelWidth + position.x / 2f + 10f, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(variableRect, property, GUIContent.none);

                Rect foldoutRect = new Rect(position.x + 15f, position.y, 15f, EditorGUIUtility.singleLineHeight);

                //Draw foldout
                _foldoutOpen = EditorGUI.Foldout(foldoutRect, _foldoutOpen, label);
                if (_foldoutOpen && property.objectReferenceValue != null)
                {
                    Rect valueRect = new Rect(position.x / 2f + 10f, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 1f, EditorGUIUtility.currentViewWidth - 22f, EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel++;
                    bool inPlaymode = EditorApplication.isPlaying;

                    var valueVariable = property.objectReferenceValue as Variable;
                    SerializedObject serializedObject = new SerializedObject(valueVariable);
                    SerializedProperty valueProperty = serializedObject.FindProperty("_value");
                    SerializedProperty startValueProperty = serializedObject.FindProperty("_startValue"); 
                    SerializedProperty _variableTypeProperty = serializedObject.FindProperty("VariableType");
                    SerializedProperty _initializeTypeProperty = serializedObject.FindProperty("InitializeType");

                    VariableType variableType = (VariableType)_variableTypeProperty.enumValueIndex;
                    InitializeType initializeType = (InitializeType)_initializeTypeProperty.enumValueIndex;

                    EditorGUI.BeginChangeCheck();

                    if (variableType == VariableType.Variable || variableType == VariableType.VariableEvent)
                    {
                        if (inPlaymode && initializeType != InitializeType.ReadOnly)
                        {
                            valueProperty.isExpanded = _expandedValue;
                            EditorGUI.PropertyField(valueRect, valueProperty, true);
                            _expandedValue = valueProperty.isExpanded;

                            float height = EditorGUI.GetPropertyHeight(valueProperty) + EditorGUIUtility.standardVerticalSpacing + 1;
                            _height += height;
                            valueRect.y += height;
                        }

                        if (initializeType == InitializeType.ResetOnGameStart || initializeType == InitializeType.ReadOnly)
                        {
                            startValueProperty.isExpanded = _expandedStartValue;
                            EditorGUI.PropertyField(valueRect, startValueProperty, true);
                            _expandedStartValue = startValueProperty.isExpanded;

                            float height = EditorGUI.GetPropertyHeight(startValueProperty) + EditorGUIUtility.standardVerticalSpacing + 1;
                            _height += height;
                            valueRect.y += height;
                        }
                    }

                    EditorGUI.PropertyField(valueRect, _variableTypeProperty, true);

                    valueRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    _height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    EditorGUI.PropertyField(valueRect, _initializeTypeProperty, true);
                    _height++;

                    if (EditorGUI.EndChangeCheck())
                        serializedObject.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                
                Rect valueRect = new Rect(position.x, position.y, position.width - 52f, position.height);
                EditorGUI.PropertyField(valueRect, property, GUIContent.none);

                Rect buttonRect = new Rect(position.x + position.width - 50f, position.y, 50f, position.height);
                if (GUI.Button(buttonRect, "New", EditorStyles.miniButton))
                {
                    Type newType = GetVariableType(property.type, out string variableTypeName);
                    Variable newVariable = ScriptableObject.CreateInstance(newType) as Variable;
                    
                    if (label.text.ToLower().Contains("event"))
                        newVariable.VariableType = VariableType.Event;

                    string path = EditorUtility.SaveFilePanel($"Create new {variableTypeName}", "Assets/Data", property.propertyPath.RemoveUnderscore().CapitalizeFirstLetter().RemoveAfterDot(), "asset");

                    if (!string.IsNullOrEmpty(path))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);

                        AssetDatabase.CreateAsset(newVariable, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        property.objectReferenceValue = newVariable;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    GUIUtility.ExitGUI();
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property, label);

            if (_foldoutOpen)
                return baseHeight + _height;

            return baseHeight;
        }

        private Type GetVariableType(string name, out string variableTypeName)
        {
            int start = name.IndexOf("<") + 2;
            int end = name.LastIndexOf(">");

            variableTypeName = name.Substring(start, end - start);
            return Type.GetType($"ScriptableArchitecture.Data.{variableTypeName}, ScriptableAssembly.Data");
        }
    }
}