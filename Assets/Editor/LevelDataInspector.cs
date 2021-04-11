using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : CustomInspector
{
    private LevelData levelData;
    private TileData tileData;
    private string[] tileNames;

    private void Awake()
    {
        levelData = (LevelData)target;

        tileNames = System.Enum.GetNames(typeof(TileType));

        if (tileData == null)
        {
            //TODO -----> Add error catching
            tileData = Resources.FindObjectsOfTypeAll<TileData>()[0];
        }

        if (levelData.tileStates == null || levelData.tileStates.Length != tileNames.Length)
        {
            levelData.tileStates = new LevelData.TileState[tileNames.Length];

            for (int i = 0; i < levelData.tileStates.Length; i++)
            {
                levelData.tileStates[i].type = (TileType)i;
                levelData.tileStates[i].isActive = true;
            }
        }

        if (levelData.tileQuotas == null || levelData.tileQuotas.Length != tileNames.Length)
        {
            levelData.tileQuotas = new LevelData.TileQuota[tileNames.Length];

            for (int i = 0; i < levelData.tileQuotas.Length; i++)
            {
                levelData.tileQuotas[i].type = (TileType)i;
                levelData.tileQuotas[i].target = 0;
            }
        }


    }

    public override void OnInspectorGUI()
    {
        GUI.color = Color.white;

        if (GUILayout.Button("Reset All"))
        {
            levelData.Reset();
        }

        #region Toggle Active Tiles

        DrawTitle("Toggle Active Tiles");

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        for (int i = 0; i < tileData.loadouts.Length; i++)
        {
            GUI.color = tileData.loadouts[i].primaryColor;

            if (GUILayout.Button(levelData.tileStates[i].isActive ? "✔" : "✘", GUILayout.Width(40), GUILayout.Height(40)))
            {
                levelData.tileStates[i].isActive = !levelData.tileStates[i].isActive;
            }

            GUI.color = Color.white;
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        #endregion

        #region Tile Quotas

        DrawTitle("Set Tile Quotas");

        for (int i = 0; i < levelData.tileQuotas.Length; i++)
        {
            levelData.tileQuotas[i].target = PlusMinusIntField(tileNames[i], levelData.tileQuotas[i].target, 0, int.MaxValue, 1);

            if (levelData.tileQuotas[i].target > 0)
            {
                levelData.tileStates[i].isActive = true;
            }
        }

        #endregion

        #region Scores and Limits

        DrawTitle("Scores and Limits");

        levelData.moveLimit = PlusMinusIntField("Moves Limit", levelData.moveLimit, 0, int.MaxValue, 1);

        levelData.completionBonus = PlusMinusIntField("Completion Bonus", levelData.completionBonus, 0, int.MaxValue, 25);

        #endregion

        #region Board Layout

        DrawTitle("Board Layout");

        levelData.boardLayout = EditorGUILayout.ObjectField("Board Layout:", levelData.boardLayout, typeof(BoardData), false) as BoardData;

        #endregion

        GUI.color = Color.white;
    }

}
