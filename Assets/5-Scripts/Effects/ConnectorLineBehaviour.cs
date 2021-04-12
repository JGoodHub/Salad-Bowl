using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorLineBehaviour : Singleton<ConnectorLineBehaviour>
{

    public LineRenderer lineRen;
    private List<Vector3> linePositions;

    /// <summary>
    /// Initialise the behaviour and subscribe to the tile chain events
    /// </summary>
    private void Start()
    {
        linePositions = new List<Vector3>();
        ResetLine(null);

        //TileChainManager.Instance.OnTileChainStarted.AddListener(SetLineColour);

        TileChainManager.Instance.OnTileAddedToChain.AddListener(UpdateLineRenderer);
        TileChainManager.Instance.OnTileRemovedFromChain.AddListener(UpdateLineRenderer);

        TileChainManager.Instance.OnTileChainFailed.AddListener(ResetLine);
        TileChainManager.Instance.OnTileChainConsumed.AddListener(ResetLine);
    }

    /// <summary>
    /// Set the colour of the line renderer to the tile colour
    /// </summary>
    /// <param name="tile">The tile to extract the colour from and apply to the line renderer</param>
    private void SetLineColour(TileBehaviour tile)
    {
        SetLineColour(GameCoordinator.Instance.TileData.ConvertTileTypeToRGB(tile.type, true));
    }

    /// <summary>
    /// Set the colour of the line renderer to the colour
    /// </summary>
    /// <param name="color">The colour to apply to the line renderer</param>
    public void SetLineColour(Color color)
    {
        lineRen.sharedMaterial.color = color;
    }

    /// <summary>
    /// Add the position of a tile to the line renderer
    /// </summary>
    /// <param name="tile">The tile whose position should be added</param>
    private void AddTileToLine(TileBehaviour tile)
    {
        linePositions.Add(tile.transform.position);
       // UpdateLineRenderer();
    }

    /// <summary>
    /// Remove the position of the tile from the line renderer
    /// </summary>
    /// <param name="tile">The tile whose position should be removed</param>
    private void RemoveTileFromLine(TileBehaviour tile)
    {
        linePositions.Remove(tile.transform.position);
        //UpdateLineRenderer();
    }

    /// <summary>
    /// Reset the line positions
    /// </summary>
    /// <param name="tileChain"></param>
    private void ResetLine(TileBehaviour[] tileChain)
    {
        linePositions.Clear();
        lineRen.positionCount = 0;
        lineRen.SetPositions(new Vector3[0]);
    }

    /// <summary>
    /// Sync the line renderer component to the linePositions array
    /// </summary>
    private void UpdateLineRenderer(TileBehaviour tile)
    {
        SetLineColour(TileChainManager.Instance.TileChain[0]);

        lineRen.positionCount = TileChainManager.Instance.TileChain.Count;
        for (int i = 0; i < TileChainManager.Instance.TileChain.Count; i++)
        {
            lineRen.SetPosition(i, TileChainManager.Instance.TileChain[i].transform.position);
        }
    }

}
