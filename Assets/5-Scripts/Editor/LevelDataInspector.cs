using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelData levelData = (LevelData)target;






    }






}
