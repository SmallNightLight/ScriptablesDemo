using ScriptableArchitecture.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableArchitecture.EditorScript
{
    //[CustomPropertyDrawer(typeof(GameEventBase<>), true)]
    //public class GameEventDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.BeginProperty(position, label, property);

    //        if (property.boxedValue == null)
    //        {
    //            //Draw create new button
    //            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
    //            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - 52f, position.height), property, GUIContent.none);

    //            Rect newRect = new Rect(position.x + position.width - 50f, position.y, 50f, position.height);
    //            if (GUI.Button(newRect, "New", EditorStyles.miniButton))
    //            {
    //                Type newType = GetVariableType(property.type, out string variableTypeName);

    //                if (variableTypeName == null)
    //                    newType = property.serializedObject.targetObject.GetType();

    //                string path = EditorUtility.SaveFilePanel($"Create new {variableTypeName}", "Assets/Data", property.name.RemoveUnderscore().CapitalizeFirstLetter(), "asset");

    //                if (!string.IsNullOrEmpty(path))
    //                {
    //                    path = "Assets" + path.Substring(Application.dataPath.Length);

    //                    ScriptableObject newGameEvent = ScriptableObject.CreateInstance(newType);

    //                    if (newGameEvent == null)
    //                        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(newType) as GameEventBase, path);
    //                    else
    //                        AssetDatabase.CreateAsset(newGameEvent, path);

    //                    AssetDatabase.SaveAssets();
    //                    AssetDatabase.Refresh();

    //                    property.objectReferenceValue = newGameEvent;
    //                    property.serializedObject.ApplyModifiedProperties();
    //                }
    //            }
    //        }
    //        else
    //        {
    //            EditorGUI.PropertyField(position, property);
    //        }
    //    }

    //    private Type GetVariableType(string name, out string variableTypeName)
    //    {
    //        int start = name.IndexOf("<") + 2;
    //        int end = name.LastIndexOf(">");

    //        variableTypeName = name.Substring(start, end - start);
    //        return Type.GetType($"ScriptableArchitecture.Data.{variableTypeName}, ScriptableAssembly.Data");
    //    }
    //}
}