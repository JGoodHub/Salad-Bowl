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
    public bool randomSeed = true;

    [Header("Tile and Backing Prefabs")]
    public GameObject platePrefab;
    public GameObject[] tilePrefabs;

    [Header("Tile Activations")]
    public bool[,] tileToggles;

    private void OnValidate()
    {
        width = Mathf.Clamp(width, 3, 9);
        height = Mathf.Clamp(height, 3, 9);

        spacing = Mathf.Clamp(spacing, 0f, float.MaxValue);
    }

    public void Init()
    {
        if (randomSeed)
            seed = System.DateTime.Now.GetHashCode();
    }
}
