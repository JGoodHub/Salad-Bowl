using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGrid : Singleton<TileGrid>
{
    public delegate void TileGridGenerated(TileData[,] tileGrid);
    public event TileGridGenerated OnTileGridGenerated;

    [Header("Board Parameters")]

    public int width;
    public int height;
    public float spacing;

    public int seed;
    public bool randomSeed = true;

    public GameObject[] tilePrefabs;
    public Transform tilesParent;
    private TileData[,] tileGrid;

    private void OnValidate()
    {
        width = Mathf.Clamp(width, 3, 9);
        height = Mathf.Clamp(height, 3, 9);

        spacing = Mathf.Clamp(spacing, 0f, float.MaxValue);
    }

    private void Start()
    {
        if (randomSeed)
            seed = System.DateTime.Now.GetHashCode();

        Random.InitState(seed);

        // Clear any existing tiles

        // Create the tile objects and place then on the grid

        tileGrid = new TileData[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileData tileData = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent).GetComponent<TileData>();

                tileData.gridPosition = new Vector2Int(x, y);
                tileData.transform.position = GridToWorldSpace(tileData.gridPosition);

                tileGrid[x, y] = tileData;
            }
        }

        // Set the adjacents for each tile for use later
        RecalculateAdjacentTiles();

        OnTileGridGenerated?.Invoke(tileGrid);

        TileLogic.Instance.OnTileChainDestroyed.AddListener(CheckForGapsInStacks);

    }

    public void RecalculateAdjacentTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int yOff = -1; yOff <= 1; yOff++)
                {
                    for (int xOff = -1; xOff <= 1; xOff++)
                    {
                        if (xOff == 0 && yOff == 0)
                            continue;

                        if (IsWithinGrid(x + xOff, y + yOff) == false)
                            continue;

                        if (tileGrid[x, y] == null || tileGrid[x + xOff, y + yOff] == null)
                            continue;

                        tileGrid[x, y].adjacents.Add(tileGrid[x + xOff, y + yOff]);
                    }
                }
            }
        }
    }

    private void CheckForGapsInStacks()
    {
        for (int x = 0; x < tileGrid.GetLength(0); x++)
        {
            for (int currY = 1; currY < tileGrid.GetLength(1); currY++)
            {
                if (tileGrid[x, currY] == null)
                    continue;

                for (int newY = 0; newY < currY; newY++)
                {
                    if (tileGrid[x, newY] == null)
                    {
                        tileGrid[x, newY] = tileGrid[x, currY];
                        tileGrid[x, currY] = null;

                        tileGrid[x, newY].MoveToGridPosition(new Vector2Int(x, newY));
                        break;
                    }
                }
            }
        }

        RecalculateAdjacentTiles();
    }

    public Vector3 GridToWorldSpace(Vector2Int gridPos)
    {
        float x = gridPos.x - (width / 2) + (width % 2 == 0 ? 0.5f : 0);
        float y = gridPos.y - (height / 2) + (height % 2 == 0 ? 0.5f : 0);
        return tilesParent.transform.position + (new Vector3(x, y, 0f) * spacing);
    }

    public bool IsWithinGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(GridToWorldSpace(new Vector2Int(x, y)), spacing / 2f);
            }
        }
    }

}
