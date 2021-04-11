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

    // Editor uses a 1D version of the toggle grid due to multidimensional arrays not being serialisable in the inspector
    [Header("Tile Activations")]
    [HideInInspector] public bool[] toggleGridEditor;
    public bool[,] enabledSquaresGrid;

    /// <summary>
    /// Set the random seed and fill out the squares enabled grid
    /// </summary>
    public void Init()
    {
        if (randomiseSeed)
        {
            Random.InitState(System.DateTime.Now.GetHashCode());
            seed = Random.Range(10000, 99999);
        }

        Convert1DToggleGridTo2D();
    }

    /// <summary>
    /// Reset the space enabled grid to true
    /// </summary>
    public void ResetToggleGrid()
    {
        toggleGridEditor = new bool[width * height];

        for (int i = 0; i < toggleGridEditor.Length; i++)
        {
            toggleGridEditor[i] = true;
        }
    }

    /// <summary>
    /// Fill out the 2D version of the squares enabled grid using the 1D equivalent
    /// </summary>
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

    /// <summary>
    /// Get the prefab associated with the tile type
    /// </summary>
    /// <param name="type">The tile type of search for</param>
    /// <returns>The tile game object for the passed tile type</returns>
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
