using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BoardLayout", menuName = "ScriptableObjects/Create Board Layout")]
public class BoardData : ScriptableObject
{
    [Header("Board Size Parameters")]
    public int width;
    public int height;
    public float spacing;

    [Header("Generation Parameters")]
    public int seed;
    public bool randomiseSeed = true;

    [Header("Tile and Backing Prefabs")]
    public GameObject platePrefab;
    public GameObject[] tilePrefabs;

    [Header("Tile Activations")]
    [HideInInspector] public bool[] toggleGridEditor;
    public bool[,] enabledSquaresGrid;

    public void Init()
    {
        if (randomiseSeed)
        {
            Random.InitState(System.DateTime.Now.GetHashCode());
            seed = Random.Range(10000, 99999);
        }

        Convert1DToggleGridTo2D();
    }

    public void ResetToggleGrid()
    {
        toggleGridEditor = new bool[width * height];

        for (int i = 0; i < toggleGridEditor.Length; i++)
        {
            toggleGridEditor[i] = true;
        }
    }

    private void Convert1DToggleGridTo2D()
    {
        enabledSquaresGrid = new bool[width, height];

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                enabledSquaresGrid[x, (height - 1) - y] = toggleGridEditor[i];

                i++;
            }
        }
    }

    public GameObject GetPrefabForType(TileType type)
    {
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            if (tilePrefabs[i].GetComponent<TileBehaviour>().type == type)
            {
                return tilePrefabs[i];
            }
        }

        return null;
    }
}
