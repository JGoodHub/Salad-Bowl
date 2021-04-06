using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorLine : Singleton<ConnectorLine>
{

    public LineRenderer lineRen;
    private List<Vector3> linePositions;

    private void Start()
    {
        linePositions = new List<Vector3>();
        ResetLine();

        TileLogic.Instance.OnTileAddedToChain.AddListener(AddTileToLine);
        TileLogic.Instance.OnTileRemovedFromChain.AddListener(RemoveTileFromLine);

        TileLogic.Instance.OnTileChainStarted.AddListener(SetLineColour);
        TileLogic.Instance.OnTileChainCancelled.AddListener(ResetLine);
        TileLogic.Instance.OnTileChainCompleted.AddListener(ResetLine);
    }

    private void SetLineColour(TileData tile)
    {
        SetLineColour(ColorPalette.ConvertTileColourToRGB(tile.color, true));
    }

    public void SetLineColour(Color color)
    {
        lineRen.sharedMaterial.color = color;
    }

    private void AddTileToLine(TileData tile)
    {
        linePositions.Add(tile.transform.position);
        UpdateLineRenderer();
    }

    private void RemoveTileFromLine(TileData tile)
    {
        linePositions.Remove(tile.transform.position);
        UpdateLineRenderer();
    }

    private void ResetLine()
    {
        linePositions.Clear();
        lineRen.positionCount = 0;
        lineRen.SetPositions(new Vector3[0]);
    }

    private void UpdateLineRenderer()
    {
        lineRen.positionCount = linePositions.Count;
        lineRen.SetPositions(linePositions.ToArray());
    }

}
