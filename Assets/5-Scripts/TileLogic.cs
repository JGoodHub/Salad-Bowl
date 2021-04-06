﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityTileDataEvent : UnityEvent<TileData> { }

public class TileLogic : Singleton<TileLogic>
{
    private List<TileData> tileChain;

    // Events for add and remove
    public UnityTileDataEvent OnTileAddedToChain;
    public UnityTileDataEvent OnTileRemovedFromChain;

    // Key start and stop events
    public UnityTileDataEvent OnTileChainStarted;
    public UnityEvent OnTileChainCancelled;
    public UnityEvent OnTileChainCompleted;

    private void Start()
    {
        tileChain = new List<TileData>();
    }

    // Start a new tile chain with the passed tile
    public void StartNewChainFromTile(TileData tile)
    {
        ClearChain();

        OnTileChainStarted?.Invoke(tile);

        AddTileToChain(tile);
    }

    // Add the tile passed to the existing chain or create one if none exists
    public void AddTileToChain(TileData tile)
    {
        // Validation checks
        Debug.Assert(tileChain != null, "Tile chain list is null");
        Debug.Assert(tile != null, "Tile is null");

        if (tileChain.Contains(tile) == false)
        {
            if (tileChain.Count == 0 || tileChain[tileChain.Count - 1].adjacents.Contains(tile))
            {
                if (tileChain.Count == 0 || tileChain[tileChain.Count - 1].color == tile.color)
                {
                    tileChain.Add(tile);
                    tile.SetSelectedState(true);

                    OnTileAddedToChain?.Invoke(tile);
                }
                else
                {
                    Debug.Log("Tried adding a tile that wasn't the same colour as the last tile in the chain");
                }
            }
            else
            {
                Debug.Log("Tried adding a tile that wasn't adjacent to the last tile in the chain");
            }
        }
        else
        {
            Debug.Log("Tile chain already contains that tile");
        }

    }

    public void TrimChainToTile(TileData tile)
    {
        // Validation checks
        Debug.Assert(tileChain != null, "Tile chain list is null");
        Debug.Assert(tile != null, "Tile is null");

        if (tileChain.Count >= 2 && tileChain[tileChain.Count - 1] != tile && tileChain.Contains(tile))
        {
            for (int i = tileChain.Count - 1; i >= 0; i--)
            {
                if (tileChain[i] == tile)
                {
                    break;
                }
                else
                {
                    TileData removedTile = tileChain[i];
                    removedTile.SetSelectedState(false);
                    tileChain.RemoveAt(i);

                    OnTileRemovedFromChain?.Invoke(removedTile);
                }
            }
        }
    }

    public void CommitChain()
    {
        // Validation checks
        Debug.Assert(tileChain != null, "Tile chain is null");

        if (tileChain.Count < 3)
        {
            ClearChain();
            OnTileChainCancelled?.Invoke();
        }
        else
        {
            ClearChain();

            // ...

            OnTileChainCompleted?.Invoke();
        }
    }

    private void ClearChain()
    {
        for (int i = 0; i < tileChain.Count; i++)
            tileChain[i].SetSelectedState(false);

        tileChain.Clear();
    }

}