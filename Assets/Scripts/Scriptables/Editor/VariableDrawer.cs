using ScriptableArchitecture.Core;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableArchitecture.EditorScript
{
    [CustomPropertyDrawer(typeof(Variable<>), true)]
    public class VariableDrawer : PropertyDrawer
    {
        bool foldoutOpen;
        bool expandedValue;
        float height = 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (position.width == 1 && position.height == 1) return;

            EditorGUI.BeginProperty(position, label, property);

            height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (property.boxedValue != null)
            {
                Rect variableRect = new Rect(EditorGUIUtility.labelWidth + position.x / 2 + 10, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(variableRect, property, GUIContent.none);

                Rect foldoutRect = new Rect(position.x + 15, position.y, 15f, EditorGUIUtility.singleLineHeight);

                //Draw foldout
                foldoutOpen = EditorGUI.Foldout(foldoutRect, foldoutOpen, label);
                if (foldoutOpen && property.objectReferenceValue != null)
                {
                    Rect valueRect = new Rect(position.x / 2 + 10, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 1, EditorGUIUtility.currentViewWidth - 22, EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel++;

                    var valueVariable = property.objectReferenceValue as Variable;
                    SerializedObject serializedObject = new SerializedObject(valueVariable);
                    SerializedProperty valueProperty = serializedObject.FindProperty("Value");

                    EditorGUI.BeginChangeCheck();

                    valueProperty.isExpanded = expandedValue;
                    EditorGUI.PropertyField(valueRect, valueProperty, true);
                    expandedValue = valueProperty.isExpanded;

                    if (EditorGUI.EndChangeCheck())
                        serializedObject.ApplyModifiedProperties();

                    height = EditorGUI.GetPropertyHeight(valueProperty) + EditorGUIUtility.standardVerticalSpacing + 4;
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                Rect valueRect = new Rect(position.x, position.y, position.width - 52f, position.height);

                EditorGUI.PropertyField(valueRect, property, GUIContent.none);

                Rect buttonRect = new Rect(position.x + position.width - 50, position.y, 50f, position.height);
                if (GUI.Button(buttonRect, "New", EditorStyles.miniButton))
                {
                    Type newType = GetVariableType(property.type, out string variableTypeName);
                    Variable newVariable = ScriptableObject.CreateInstance(newType) as Variable;

                    string path = EditorUtility.SaveFilePanel($"Create new {variableTypeName}", "Assets/Data", property.name.RemoveUnderscore().CapitalizeFirstLetter(), "asset");

                    if (!string.IsNullOrEmpty(path))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length); //Set type to Event

                        AssetDatabase.CreateAsset(newVariable, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        property.objectReferenceValue = newVariable;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property, label);

            if (foldoutOpen)
                return baseHeight + height;

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