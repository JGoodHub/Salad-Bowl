
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardData))]
public class BoardDataInspector : CustomInspector
{
    public override void OnInspectorGUI()
    {
        GUI.color = Color.white;

        BoardData board = target as BoardData;


        #region Size Settings

        DrawTitle("Size Settings");

        board.width = EditorGUILayout.IntSlider("Width:", board.width, 3, 9);
        board.height = EditorGUILayout.IntSlider("Height:", board.height, 3, 9);
        board.spacing = EditorGUILayout.Slider("Spacing:", board.spacing, 0f, 3f);

        #endregion

        #region Generation Settings

        DrawTitle("Generation Settings");

        board.randomiseSeed = EditorGUILayout.Toggle("Randomise Seed:", board.randomiseSeed);

        EditorGUILayout.BeginHorizontal();

        board.seed = EditorGUILayout.IntField("Seed:", board.seed);

        if (GUILayout.Button("ROLL", GUILayout.Width(45)))
        {
            Random.InitState(System.DateTime.Now.GetHashCode());
            board.seed = Random.Range(10000, 99999);
        }

        EditorGUILayout.EndHorizontal();

        //TODO ----> Randomise button

        //TODO ----> Preview of starting grid layout

        #endregion

        #region Tile Prefabs

        DrawTitle("Tile Prefabs");

        board.platePrefab = EditorGUILayout.ObjectField("Plate Prefab:", board.platePrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.LabelField("Tile Prefabs", EditorStyles.boldLabel);

        string[] typeNames = System.Enum.GetNames(typeof(TileType));

        if (board.tilePrefabs.Length != typeNames.Length)
        {
            board.tilePrefabs = new GameObject[typeNames.Length];
        }

        for (int i = 0; i < board.tilePrefabs.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"{typeNames[i].ToUpper()}:", GUILayout.Width(EditorGUIUtility.labelWidth));
            board.tilePrefabs[i] = EditorGUILayout.ObjectField(board.tilePrefabs[i], typeof(GameObject), true) as GameObject;

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Toggle Grid

        DrawTitle("Tile Toggle Grid");

        GUILayout.Space(5);

        if (board.toggleGridEditor == null || board.toggleGridEditor.Length != board.width * board.height)
        {
            board.ResetToggleGrid();
        }

        int j = 0;
        for (int y = 0; y < board.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int x = 0; x < board.width; x++)
            {
                GUI.color = board.toggleGridEditor[j] ? Color.green : Color.red;

                if (GUILayout.Button(board.toggleGridEditor[j] ? "" : "X", GUILayout.Width(30), GUILayout.Height(30)))
                {
                    board.toggleGridEditor[j] = !board.toggleGridEditor[j];
                }

                GUI.color = Color.white;

                GUILayout.Space(5);

                j++;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
        }

        if (GUILayout.Button("Reset", GUILayout.Height(25)))
        {
            board.ResetToggleGrid();
        }

        #endregion

        EditorUtility.SetDirty(board);

        GUI.color = Color.white;
    }

}