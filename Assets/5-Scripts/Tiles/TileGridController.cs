using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGridController : Singleton<TileGridController>
{
    public delegate void TileGridGenerated(Tile[,] tileGrid);
    public event TileGridGenerated OnTileGridGenerated;

    [Header("Board Parameters")]
    public Transform tilesParent;
    private Tile[,] tileGrid;

    private BoardLayoutData boardLayout;

    private void Start()
    {
        Random.InitState(GameCoordinator.Instance.BoardLayout.seed);

        boardLayout = GameCoordinator.Instance.BoardLayout;

        // Create the tile objects and place then on the grid

        tileGrid = new Tile[boardLayout.width, boardLayout.height];

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                Tile tileData = Instantiate(boardLayout.tilePrefabs[Random.Range(0, boardLayout.tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent).GetComponent<Tile>();

                tileData.gridRef = new Vector2Int(x, y);
                tileData.transform.position = GridToWorldSpace(tileData.gridRef);

                tileGrid[x, y] = tileData;
            }
        }

        // Set the adjacents for each tile for use later
        RecalculateAdjacentTiles();

        OnTileGridGenerated?.Invoke(tileGrid);

        TileChainManager.Instance.OnTileChainDestroyed.AddListener(CheckForGapsInStacks);

    }

    public void RecalculateAdjacentTiles()
    {
        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
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

                        tileGrid[x, newY].MoveToGridRef(new Vector2Int(x, newY));
                        break;
                    }
                }
            }
        }

        FillEmptyGaps();

        RecalculateAdjacentTiles();
    }

    private void FillEmptyGaps()
    {
        for (int x = 0; x < tileGrid.GetLength(0); x++)
        {
            for (int y = 0; y < tileGrid.GetLength(1); y++)
            {
                if (tileGrid[x, y] == null)
                {
                    tileGrid[x, y] = CreateNewTileAtGridRef(x, boardLayout.height + y);
                    tileGrid[x, y].MoveToGridRef(x, y);
                }
            }
        }
    }

    public Tile CreateNewTileAtGridRef(int x, int y)
    {
        Tile tileData = Instantiate(boardLayout.tilePrefabs[Random.Range(0, boardLayout.tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent).GetComponent<Tile>();

        tileData.gridRef = new Vector2Int(x, y);
        tileData.transform.position = GridToWorldSpace(tileData.gridRef);

        return tileData;
    }

    public Vector3 GridToWorldSpace(Vector2Int gridPos)
    {
        float x = gridPos.x - (boardLayout.width / 2) + (boardLayout.width % 2 == 0 ? 0.5f : 0);
        float y = gridPos.y - (boardLayout.height / 2) + (boardLayout.height % 2 == 0 ? 0.5f : 0);
        return tilesParent.transform.position + (new Vector3(x, y, 0f) * boardLayout.spacing);
    }

    public bool IsWithinGrid(int x, int y)
    {
        return x >= 0 && x < boardLayout.width && y >= 0 && y < boardLayout.height;
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(GridToWorldSpace(new Vector2Int(x, y)), boardLayout.spacing / 2f);
            }
        }
    }

}
