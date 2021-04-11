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

    private TileBehaviour[,] tileGrid;

    private List<Vector2Int> enabledGridRefs;

    private BoardData board;

    private int tileMovingCount;
    public bool gridLocked;

    private void Start()
    {
        board = GameCoordinator.Instance.LevelData.boardLayout;

        Random.InitState(board.seed);

        // Create the background plate objects
        // Create the tile objects and place then on the grid

        ClearAndFillPlateGrid();

        enabledGridRefs = new List<Vector2Int>();
        tileGrid = new TileBehaviour[board.width, board.height];

        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                if (board.enabledSquaresGrid[x, y] == true)
                {
                    SetTile(x, y, CreateTileAtGridRef(x, y));

                    enabledGridRefs.Add(new Vector2Int(x, y));
                }
            }
        }

        // Set the adjacents for each tile for use later
        RecalculateAdjacentTiles();

        OnTileGridGenerated?.Invoke(tileGrid);

        TileChainManager.Instance.OnTileChainDestroyed.AddListener(OnTileChainDestroyed);
    }

    #region Object Creation

    public void ClearAndFillPlateGrid()
    {
        for (int c = 0; c < platesParent.childCount; c++)
        {
            Destroy(platesParent.GetChild(c).gameObject);
        }

        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                if (board.enabledSquaresGrid[x, y] == true)
                {
                    CreatePlateAtGridRef(x, y);
                }
            }
        }
    }

    public GameObject CreatePlateAtGridRef(int x, int y)
    {
        GameObject plateObject = Instantiate(board.platePrefab, Vector3.zero, Quaternion.identity, platesParent);
        plateObject.transform.position = GridToWorldSpace(x, y, platesParent.position.z);

        return plateObject;
    }

    public TileBehaviour CreateTileAtGridRef(int x, int y)
    {
        GameObject tileObject = Instantiate(board.tilePrefabs[Random.Range(0, board.tilePrefabs.Length)], Vector3.zero, Quaternion.identity, tilesParent);
        tileObject.transform.position = GridToWorldSpace(x, y, tilesParent.position.z);

        TileBehaviour tile = tileObject.GetComponent<TileBehaviour>();

        tile.MovementBehaviour.OnTileStartedMoving.AddListener(RegisterTileStartedMoving);
        tile.MovementBehaviour.OnTileFinishedMoving.AddListener(RegisterTileStoppedMoving);

        return tile;
    }

    #endregion

    #region Event Handlers

    public void OnTileChainDestroyed()
    {
        gridLocked = true;

        CompressTileGrid();
        TopUpTileGrid();

        SyncTilesToMatchGrid(false);
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
                ShuffleTileGrid(200);
                RecalculateAdjacentTiles();
                PlantChainOfLength(3);
                SyncTilesToMatchGrid(false);
            }
        }
    }

    #endregion

    #region Grid Manipulation Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shuffleIterations"></param>
    public void ShuffleTileGrid(int shuffleIterations)
    {
        for (int r = 0; r < shuffleIterations; r++)
        {
            Vector2Int gridRefA = GetRandomValidGridRef();
            Vector2Int gridRefB = GetRandomValidGridRef();

            SwapTilesInGrid(gridRefA, gridRefB);
        }
    }

    public void PlantChainOfLength(int length)
    {
        // Find a random tile in the grid
        Vector2Int gridRef;
        do
        {
            gridRef = GetRandomValidGridRef();
        } while (GetTileCount(GetTile(gridRef).type) < length);

        TileType type = GetTile(gridRef).type;
        TileBehaviour[] tilesOfType = GetTilesOfType(type);

        List<Vector2Int> targetGridRefs = new List<Vector2Int>();
        targetGridRefs.Add(tilesOfType[0].MovementBehaviour.gridRef);
        for (int i = 1; i < length; i++)
        {
            List<Vector2Int> adjacentRefs = GetAdjacentGridRefs(targetGridRefs[i - 1]);
            Vector2Int nextGridRef = adjacentRefs[Random.Range(0, adjacentRefs.Count)];

            if (targetGridRefs.Contains(nextGridRef))
            {
                i--;
            }
            else
            {
                targetGridRefs.Add(nextGridRef);
            }
        }

        for (int i = 1; i < length; i++)
        {
            Vector2Int gridRefA = tilesOfType[i].MovementBehaviour.gridRef;
            Vector2Int gridRefB = GetTile(targetGridRefs[i]).MovementBehaviour.gridRef;

            SwapTilesInGrid(gridRefA, gridRefB);
        }
    }

    private void CompressTileGrid()
    {
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 1; y < board.height; y++)
            {
                if (tileGrid[x, y] == null)
                    continue;

                for (int newY = 0; newY < y; newY++)
                {
                    if (board.enabledSquaresGrid[x, newY] == false)
                        continue;

                    if (tileGrid[x, newY] == null)
                    {
                        SetTile(x, newY, tileGrid[x, y]);
                        SetTile(x, y, null);
                        break;
                    }
                }
            }
        }
    }

    private void TopUpTileGrid()
    {
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (tileGrid[x, y] == null && board.enabledSquaresGrid[x, y] == true)
                {
                    SetTile(x, y, CreateTileAtGridRef(x, board.height + y));
                }
            }
        }
    }

    #endregion

    #region Tile Manipulation

    public void RecalculateAdjacentTiles()
    {
        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                if (tileGrid[x, y] == null)
                    continue;

                tileGrid[x, y].adjacents.Clear();
                List<Vector2Int> adjacentRefs = GetAdjacentGridRefs(x, y);

                for (int i = 0; i < adjacentRefs.Count; i++)
                {
                    if (GetTile(adjacentRefs[i]) == null)
                        continue;

                    tileGrid[x, y].adjacents.Add(GetTile(adjacentRefs[i]));
                }
            }
        }
    }

    public void SyncTilesToMatchGrid(bool instant)
    {
        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                if (tileGrid[x, y] != null && board.enabledSquaresGrid[x, y] == true)
                {
                    tileGrid[x, y].MovementBehaviour.SyncPositionToGridRef(instant);
                }
            }
        }
    }

    #endregion

    #region Grid Utility Methods

    public void SwapTilesInGrid(Vector2Int gridRefA, Vector2Int gridRefB)
    {
        if (IsWithinBoardAndEnabled(gridRefA) && IsWithinBoardAndEnabled(gridRefB))
        {
            TileBehaviour temp = tileGrid[gridRefA.x, gridRefA.y];

            SetTile(gridRefA, GetTile(gridRefB));
            SetTile(gridRefB, temp);
        }
    }

    public Vector3 GridToWorldSpace(int gridX, int gridY)
    {
        return GridToWorldSpace(gridX, gridY, 0);
    }

    public Vector3 GridToWorldSpace(int gridX, int gridY, float worldZ)
    {
        float worldX = gridX - (board.width / 2) + (board.width % 2 == 0 ? 0.5f : 0);
        float worldY = gridY - (board.height / 2) + (board.height % 2 == 0 ? 0.5f : 0);
        return tilesParent.transform.position + (new Vector3(worldX, worldY, worldZ) * board.spacing);
    }

    public List<Vector2Int> GetAdjacentGridRefs(Vector2Int gridRef)
    {
        return GetAdjacentGridRefs(gridRef.x, gridRef.y);
    }

    public List<Vector2Int> GetAdjacentGridRefs(int x, int y)
    {
        List<Vector2Int> adjacentRefs = new List<Vector2Int>();

        for (int yOff = -1; yOff <= 1; yOff++)
        {
            for (int xOff = -1; xOff <= 1; xOff++)
            {
                if (xOff == 0 && yOff == 0)
                    continue;

                if (IsWithinBoardAndEnabled(x + xOff, y + yOff) == false)
                    continue;

                adjacentRefs.Add(new Vector2Int(x + xOff, y + yOff));
            }
        }

        return adjacentRefs;
    }

    public Vector2Int GetRandomValidGridRef()
    {
        return enabledGridRefs[Random.Range(0, enabledGridRefs.Count)];
    }

    public bool IsWithinBoard(int x, int y)
    {
        return x >= 0 && x < board.width && y >= 0 && y < board.height;
    }

    public bool IsWithinBoard(Vector2Int gridRef)
    {
        return gridRef.x >= 0 && gridRef.x < board.width && gridRef.y >= 0 && gridRef.y < board.height;
    }

    public bool IsWithinBoardAndEnabled(int x, int y)
    {
        return (x >= 0 && x < board.width && y >= 0 && y < board.height) && board.enabledSquaresGrid[x, y];
    }

    public bool IsWithinBoardAndEnabled(Vector2Int gridRef)
    {
        return (gridRef.x >= 0 && gridRef.x < board.width && gridRef.y >= 0 && gridRef.y < board.height) && board.enabledSquaresGrid[gridRef.x, gridRef.y];
    }

    public void SetTile(int x, int y, TileBehaviour tile)
    {
        Debug.Assert(IsWithinBoard(x, y), $"Grid Reference '{x}, {y}' passed is not within the grid");

        if (IsWithinBoard(x, y))
            tileGrid[x, y] = tile;

        if (tile != null)
            tile.MovementBehaviour.gridRef = new Vector2Int(x, y);
    }

    public void SetTile(Vector2Int gridRef, TileBehaviour tile)
    {
        Debug.Assert(IsWithinBoard(gridRef), $"Grid Reference '{gridRef.x}, {gridRef.y}' passed is not within the grid");

        if (IsWithinBoard(gridRef))
            tileGrid[gridRef.x, gridRef.y] = tile;

        if (tile != null)
            tile.MovementBehaviour.gridRef = gridRef;
    }

    public TileBehaviour GetTile(int x, int y)
    {
        Debug.Assert(IsWithinBoardAndEnabled(x, y), $"Grid Reference '{x}, {y}' passed is either not within the grid or is an empty space");

        if (IsWithinBoardAndEnabled(x, y))
            return tileGrid[x, y];
        else
            return null;
    }

    public TileBehaviour GetTile(Vector2Int gridRef)
    {
        Debug.Assert(IsWithinBoardAndEnabled(gridRef), $"Grid Reference '{gridRef.x}, {gridRef.y}' passed is either not within the grid or is an empty space");

        if (IsWithinBoardAndEnabled(gridRef))
            return tileGrid[gridRef.x, gridRef.y];
        else
            return null;
    }

    public TileBehaviour[] GetTilesOfType(TileType type)
    {
        List<TileBehaviour> tiles = new List<TileBehaviour>();
        foreach (TileBehaviour tile in tileGrid)
        {
            if (tile != null && tile.type == type)
            {
                tiles.Add(tile);
            }
        }

        return tiles.ToArray();
    }

    public int GetTileCount(TileType type)
    {
        return GetTilesOfType(type).Length;
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
            if (tile == null || tile.type != type || closedSet.Contains(tile))
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

    #endregion

    /// <summary>
    /// Draws a preview of the board layout
    /// </summary>
    private void OnDrawGizmos()
    {
        board = GameCoordinator.Instance.LevelData.boardLayout;

        if (board == null)
            return;

        for (int y = 0; y < board.height; y++)
        {
            for (int x = 0; x < board.width; x++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(GridToWorldSpace(x, y), board.spacing / 2f);
            }
        }
    }
}
