using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomInspector : Editor
{
    private GUIStyle sectionHeader;

    public void DrawTitle(string headerText)
    {
        if (sectionHeader == null)
            CreateStyles();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(headerText, sectionHeader);
        Rect labelRect = GUILayoutUtility.GetLastRect();

        labelRect.yMin = labelRect.yMin - 4;
        labelRect.yMax = labelRect.yMax + 5;

        Texture2D lineTex = new Texture2D(1, 1);
        lineTex.SetPixel(0, 0, Color.black);
        lineTex.wrapMode = TextureWrapMode.Repeat;
        lineTex.Apply();

        GUI.Box(labelRect, lineTex);

        EditorGUILayout.Space();
    }

    public int PlusMinusIntField(string label, int value, int min, int max, int step)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(25));

        if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(25)))
        {
            value = Mathf.Clamp(value - step, min, max);
        }

        value = EditorGUILayout.IntField(value, GUILayout.Height(25));

        if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(25)))
        {
            value = Mathf.Clamp(value + step, min, max);
        }

        EditorGUILayout.EndHorizontal();

        return value;
    }

    private void CreateStyles()
    {
        sectionHeader = new GUIStyle(EditorStyles.boldLabel);
        sectionHeader.fontSize = 16;
        sectionHeader.normal.textColor = Color.white;
        sectionHeader.alignment = TextAnchor.MiddleCenter;
    }

}
