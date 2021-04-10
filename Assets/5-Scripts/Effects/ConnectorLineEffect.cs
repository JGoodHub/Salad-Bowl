﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorLineEffect : Singleton<ConnectorLineEffect>
{

    public LineRenderer lineRen;
    private List<Vector3> linePositions;

    private void Start()
    {
        linePositions = new List<Vector3>();
        ResetLine(null);

        TileChainManager.Instance.OnTileAddedToChain.AddListener(AddTileToLine);
        TileChainManager.Instance.OnTileRemovedFromChain.AddListener(RemoveTileFromLine);

        TileChainManager.Instance.OnTileChainStarted.AddListener(SetLineColour);
        TileChainManager.Instance.OnTileChainFailed.AddListener(ResetLine);
        TileChainManager.Instance.OnTileChainConsumed.AddListener(ResetLine);
    }

    private void SetLineColour(TileSelectionBehaviour tile)
    {
        SetLineColour(GameCoordinator.Instance.TileLoadouts.ConvertTileTypeToRGB(tile.type, true));
    }

    public void SetLineColour(Color color)
    {
        lineRen.sharedMaterial.color = color;
    }

    private void AddTileToLine(TileSelectionBehaviour tile)
    {
        linePositions.Add(tile.transform.position);
        UpdateLineRenderer();
    }

    private void RemoveTileFromLine(TileSelectionBehaviour tile)
    {
        linePositions.Remove(tile.transform.position);
        UpdateLineRenderer();
    }

    private void ResetLine(TileSelectionBehaviour[] tileChain)
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