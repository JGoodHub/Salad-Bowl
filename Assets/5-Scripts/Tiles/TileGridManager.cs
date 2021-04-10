using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGridManager : Singleton<TileGridManager>
{
    public delegate void TileGridGenerated(TileBehaviour[,] tileGrid);
    public event TileGridGenerated OnTileGridGenerated;

    [Header("Board Parents")]
    public Transform platesParent;
    public Transform tilesParent;

    private GameObject[,] plateGrid;
    private TileBehaviour[,] tileGrid;

    private BoardData boardLayout;

    private int tileMovingCount;
    public bool gridLocked;

    private void Start()
    {
        boardLayout = GameCoordinator.Instance.LevelData.boardLayout;

        Random.InitState(boardLayout.seed);

        // Create the background plate objects

        plateGrid = new GameObject[boardLayout.width, boardLayout.height];

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                plateGrid[x, y] = CreatePlateAtGridRef(x, y);
            }
        }

        // Create the tile objects and place then on the grid

        tileGrid = new TileBehaviour[boardLayout.width, boardLayout.height];

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                tileGrid[x, y] = CreateTileAtGridRef(x, y);
            }
        }

        // Set the adjacents for each tile for use later
        RecalculateAdjacentTiles();

        OnTileGridGenerated?.Invoke(tileGrid);

        TileChainManager.Instance.OnTileChainConsumed.AddListener(OnTileChianDestroyed);
        TileChainManager.Instance.OnTileChainDestroyed.AddListener(CheckForGapsInStacks);
    }

    public void RecalculateAdjacentTiles()
    {
        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                tileGrid[x, y].adjacents.Clear();

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
    }

    private void FillEmptyGaps()
    {
        for (int x = 0; x < tileGrid.GetLength(0); x++)
        {
            for (int y = 0; y < tileGrid.GetLength(1); y++)
            {
                if (tileGrid[x, y] == null)
                {
                    tileGrid[x, y] = CreateTileAtGridRef(x, boardLayout.height + y);
                    tileGrid[x, y].GetComponent<TileMovementBehaviour>().MoveToGridRef(x, y, false);
                }
            }
        }
    }

    public GameObject CreatePlateAtGridRef(int x, int y)
    {
        GameObject plateObject = Instantiate(boardLayout.platePrefab, Vector3.zero, Quaternion.identity, platesParent);
        plateObject.transform.position = GridToWorldSpace(x, y, platesParent.position.z);

        return plateObject;
    }

    public TileBehaviour CreateTileAtGridRef(int x, int y)
    {
        GameObject tileObject = Instantiate(boardLayout.tilePrefabs[Random.Range(0, boardLayout.tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent);
        tileObject.transform.position = GridToWorldSpace(x, y, tilesParent.position.z);

        TileBehaviour tileBehaviour = tileObject.GetComponent<TileBehaviour>();

        tileBehaviour.MovementBehaviour.OnTileStartedMoving.AddListener(RegisterTileStartedMoving);
        tileBehaviour.MovementBehaviour.OnTileFinishedMoving.AddListener(RegisterTileStoppedMoving);

        tileBehaviour.MovementBehaviour.MoveToGridRef(x, y, true);

        return tileBehaviour;
    }

    public Vector3 GridToWorldSpace(int gridX, int gridY)
    {
        return GridToWorldSpace(gridX, gridY, 0);
    }

    public Vector3 GridToWorldSpace(int gridX, int gridY, float worldZ)
    {
        float worldX = gridX - (boardLayout.width / 2) + (boardLayout.width % 2 == 0 ? 0.5f : 0);
        float worldY = gridY - (boardLayout.height / 2) + (boardLayout.height % 2 == 0 ? 0.5f : 0);
        return tilesParent.transform.position + (new Vector3(worldX, worldY, worldZ) * boardLayout.spacing);
    }

    public bool IsWithinGrid(int x, int y)
    {
        return x >= 0 && x < boardLayout.width && y >= 0 && y < boardLayout.height;
    }

    public void OnTileChianDestroyed(TileBehaviour[] tileChain)
    {
        gridLocked = true;
    }

    private void RegisterTileStartedMoving(TileBehaviour tile)
    {
        tileMovingCount++;
        gridLocked = true;
    }

    private void RegisterTileStoppedMoving(TileBehaviour tile)
    {
        tileMovingCount--;

        Debug.Assert(tileMovingCount >= 0, "tileMovingCount is somehow below zero, this is an invalid state");

        if (tileMovingCount == 0)
        {
            gridLocked = false;

            RecalculateAdjacentTiles();

            if (GridContainsValidChain(3) == false)
            {
                ShuffleTiles(200);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shuffleIterations"></param>
    public void ShuffleTiles(int shuffleIterations)
    {
        Vector2Int[,] newGridRefs = new Vector2Int[tileGrid.GetLength(0), tileGrid.GetLength(1)];

        for (int y = 0; y < newGridRefs.GetLength(1); y++)
        {
            for (int x = 0; x < newGridRefs.GetLength(0); x++)
            {
                newGridRefs[x, y] = tileGrid[x, y].GetComponent<TileMovementBehaviour>().GridRef;
            }
        }

        for (int r = 0; r < shuffleIterations; r++)
        {
            Vector2Int randomRefA = new Vector2Int(Random.Range(0, newGridRefs.GetLength(0)), Random.Range(0, newGridRefs.GetLength(1)));
            Vector2Int randomRefB = new Vector2Int(Random.Range(0, newGridRefs.GetLength(0)), Random.Range(0, newGridRefs.GetLength(1)));

            Vector2Int temp = newGridRefs[randomRefA.x, randomRefA.y];
            newGridRefs[randomRefA.x, randomRefA.y] = newGridRefs[randomRefB.x, randomRefB.y];
            newGridRefs[randomRefB.x, randomRefB.y] = temp;
        }

        for (int y = 0; y < newGridRefs.GetLength(1); y++)
        {
            for (int x = 0; x < newGridRefs.GetLength(0); x++)
            {
                tileGrid[x, y].GetComponent<TileMovementBehaviour>().MoveToGridRef(newGridRefs[x, y].x, newGridRefs[x, y].y, false);
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="validLength"></param>
    /// <returns></returns>
    public bool CheckForValidChain(TileType type, int validLength)
    {
        HashSet<TileBehaviour> closedSet = new HashSet<TileBehaviour>();
        Queue<TileBehaviour> searchQueue = new Queue<TileBehaviour>();
        Dictionary<TileBehaviour, int> tileLengths = new Dictionary<TileBehaviour, int>();

        int maxChainLength = 0;
        foreach (TileBehaviour tile in tileGrid)
        {
            if (tile.type != type || closedSet.Contains(tile))
                continue;

            searchQueue.Enqueue(tile);
            tileLengths.Add(tile, 1);

            maxChainLength = Mathf.Max(maxChainLength, 1);

            while (searchQueue.Count > 0)
            {
                TileBehaviour searchTile = searchQueue.Dequeue();

                foreach (TileBehaviour adjacent in searchTile.adjacents)
                {
                    if (adjacent.type != type || closedSet.Contains(adjacent))
                        continue;

                    searchQueue.Enqueue(adjacent);

                    if (tileLengths.ContainsKey(adjacent))
                        tileLengths[adjacent] = tileLengths[searchTile] + 1;
                    else
                        tileLengths.Add(adjacent, tileLengths[searchTile] + 1);

                    maxChainLength = Mathf.Max(maxChainLength, tileLengths[searchTile] + 1);
                }

                closedSet.Add(searchTile);
            }
        }

        return maxChainLength >= validLength;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validChainLength"></param>
    /// <returns></returns>
    public bool GridContainsValidChain(int validChainLength)
    {
        bool validChainExists = false;
        foreach (TileType type in System.Enum.GetValues(typeof(TileType)))
        {
            if (CheckForValidChain(type, validChainLength) == true)
            {
                validChainExists = true;
            }
        }

        return validChainExists;
    }

    /// <summary>
    /// Draws a preview of the board layout
    /// </summary>
    private void OnDrawGizmos()
    {
        boardLayout = GameCoordinator.Instance.LevelData.boardLayout;

        if (boardLayout == null)
            return;

        for (int y = 0; y < boardLayout.height; y++)
        {
            for (int x = 0; x < boardLayout.width; x++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(GridToWorldSpace(x, y), boardLayout.spacing / 2f);
            }
        }
    }
}
