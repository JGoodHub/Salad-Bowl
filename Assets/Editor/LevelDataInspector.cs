using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : CustomInspector
{
    private LevelData level;
    private TileData tileData;
    private string[] tileNames;

    private void Awake()
    {
        level = (LevelData)target;

        tileNames = System.Enum.GetNames(typeof(TileType));

        if (tileData == null)
        {
            //TODO -----> Add error catching
            tileData = Resources.FindObjectsOfTypeAll<TileData>()[0];
        }

        if (level.tileStates == null || level.tileStates.Length != tileNames.Length)
        {
            level.tileStates = new LevelData.TileState[tileNames.Length];

            for (int i = 0; i < level.tileStates.Length; i++)
            {
                level.tileStates[i].type = (TileType)i;
                level.tileStates[i].isActive = true;
            }
        }

        if (level.tileQuotas == null || level.tileQuotas.Length != tileNames.Length)
        {
            level.tileQuotas = new LevelData.TileQuota[tileNames.Length];

            for (int i = 0; i < level.tileQuotas.Length; i++)
            {
                level.tileQuotas[i].type = (TileType)i;
                level.tileQuotas[i].target = 0;
            }
        }


    }

    public override void OnInspectorGUI()
    {
        GUI.color = Color.white;

        if (GUILayout.Button("Reset All"))
        {
            level.Reset();
        }

        #region Toggle Active Tiles

        DrawTitle("Toggle Active Tiles");

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        for (int i = 0; i < tileData.loadouts.Length; i++)
        {
            GUI.color = tileData.loadouts[i].primaryColor;

            if (GUILayout.Button(level.tileStates[i].isActive ? "✔" : "✘", GUILayout.Width(40), GUILayout.Height(40)))
            {
                level.tileStates[i].isActive = !level.tileStates[i].isActive;
            }

            GUI.color = Color.white;
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        #endregion

        #region Tile Quotas

        DrawTitle("Set Tile Quotas");

        for (int i = 0; i < level.tileQuotas.Length; i++)
        {
            level.tileQuotas[i].target = PlusMinusIntField(tileNames[i], level.tileQuotas[i].target, 0, int.MaxValue, 1);

            if (level.tileQuotas[i].target > 0)
            {
                level.tileStates[i].isActive = true;
            }
        }

        #endregion

        #region Scores and Limits

        DrawTitle("Scores and Limits");

        level.moveLimit = PlusMinusIntField("Moves Limit", level.moveLimit, 0, int.MaxValue, 1);

        level.completionBonus = PlusMinusIntField("Completion Bonus", level.completionBonus, 0, int.MaxValue, 25);

        #endregion

        #region Board Layout

        DrawTitle("Board Layout");

        level.boardLayout = EditorGUILayout.ObjectField("Board Layout:", level.boardLayout, typeof(BoardData), false) as BoardData;

        #endregion

        EditorUtility.SetDirty(level);

        GUI.color = Color.white;
    }

}
