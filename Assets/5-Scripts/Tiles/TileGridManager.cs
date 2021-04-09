using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGridManager : Singleton<TileGridManager>
{
    public delegate void TileGridGenerated(TileSelectionBehaviour[,] tileGrid);
    public event TileGridGenerated OnTileGridGenerated;

    [Header("Board Parameters")]
    public Transform tilesParent;
    private TileSelectionBehaviour[,] tileGrid;

    private BoardLayoutData boardLayout;

    public bool GridLocked { get; private set; }

    private void Start()
    {
        boardLayout = GameCoordinator.Instance.BoardLayout;

        Random.InitState(boardLayout.seed);

        // Create the tile objects and place then on the grid

        tileGrid = new TileSelectionBehaviour[boardLayout.width, boardLayout.height];

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                TileSelectionBehaviour tileSelection = CreateNewTileAtGridRef(x, y);
                TileMovementBehaviour tileMovement = tileSelection.GetComponent<TileMovementBehaviour>();

                tileGrid[x, y] = tileSelection;
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

                        tileGrid[x, newY].GetComponent<TileMovementBehaviour>().MoveToGridRef(x, newY, false);
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
                    tileGrid[x, y].GetComponent<TileMovementBehaviour>().MoveToGridRef(x, y, false);
                }
            }
        }
    }

    public void SetGridLocked(bool isLocked)
    {
        GridLocked = isLocked;
    }

    public bool IsGridMoving()
    {
        bool tilesMoving = false;
        //foreach (TileSelectionBehaviour tile in tileGrid)
        //{
        //    if (tile.Moving == true)
        //    {
        //        tilesMoving = true;
        //        break;
        //    }
        //}

        return tilesMoving;
    }

    public TileSelectionBehaviour CreateNewTileAtGridRef(int x, int y)
    {
        GameObject tileObject = Instantiate(boardLayout.tilePrefabs[Random.Range(0, boardLayout.tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent);

        TileSelectionBehaviour tileSelection = tileObject.GetComponent<TileSelectionBehaviour>();
        TileMovementBehaviour tileMovement = tileObject.GetComponent<TileMovementBehaviour>();

        tileMovement.MoveToGridRef(x, y, true);

        return tileSelection;
    }

    public Vector3 GridToWorldSpace(int x, int y)
    {
        return GridToWorldSpace(new Vector2Int(x, y));
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
        boardLayout = GameCoordinator.Instance.BoardLayout;

        if (boardLayout == null)
            return;

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
