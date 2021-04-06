using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
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
                tileData.transform.localPosition = new Vector3(x - (width / 2) + (width % 2 == 0 ? 0.5f : 0), y - (height / 2) + (height % 2 == 0 ? 0.5f : 0), 0f);
                tileData.transform.localPosition *= spacing;

                tileGrid[x, y] = tileData;
            }
        }

        // Set the adjacents for each tile for use later

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

                        if (y + yOff >= 0 && y + yOff < height && x + xOff >= 0 && x + xOff < width)
                        {
                            tileGrid[x, y].adjacents.Add(tileGrid[x + xOff, y + yOff]);
                        }
                    }
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 gizmoCente = tilesParent.transform.position;
                gizmoCente += new Vector3(x - (width / 2) + (width % 2 == 0 ? 0.5f : 0), y - (height / 2) + (height % 2 == 0 ? 0.5f : 0), 0f) * spacing;

                Gizmos.DrawWireSphere(gizmoCente, spacing / 2f);
            }
        }
    }

}
