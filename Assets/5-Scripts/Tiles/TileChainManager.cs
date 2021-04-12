using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileChainManager : Singleton<TileChainManager>
{
    public List<TileBehaviour> TileChain { get; private set; }
    public float tileConsumptionInterval = 0.1f;
    public float tileDestructionDelay = 0.5f;

    // Events for add and remove
    public UnityTileEvent OnTileAddedToChain;
    public UnityTileEvent OnTileRemovedFromChain;

    // Key start and stop events
    public UnityTileEvent OnTileChainStarted;
    public UnityTileArrayEvent OnTileChainConsumed;
    public UnityTileArrayEvent OnTileChainFailed;
    public UnityEvent OnTileChainDestroyed;

    private void Start()
    {
        TileChain = new List<TileBehaviour>();
    }

    // Start a new tile chain with the passed tile
    public void StartNewChainFromTile(TileBehaviour tile)
    {
        ClearChain();

        OnTileChainStarted?.Invoke(tile);

        AddTileToChain(tile);
    }

    // Add the tile passed to the existing chain or create one if none exists
    public void AddTileToChain(TileBehaviour tile)
    {
        // Validation checks
        Debug.Assert(TileChain != null, "Tile chain list is null");
        Debug.Assert(tile != null, "Tile is null");

        if (TileChain.Contains(tile) == false)
        {
            if (TileChain.Count == 0 || TileChain[TileChain.Count - 1].adjacents.Contains(tile))
            {
                if (TileChain.Count == 0 || TileChain[TileChain.Count - 1].type == tile.type)
                {
                    TileChain.Add(tile);
                    tile.SelectionBehaviour.SetSelectedState(true);

                    OnTileAddedToChain?.Invoke(tile);
                }
            }
        }

    }

    // Trim the chain back to until it encounters the given tile
    public void TrimChainToTile(TileBehaviour tile)
    {
        // Validation checks
        Debug.Assert(TileChain != null, "Tile chain list is null");
        Debug.Assert(tile != null, "Tile is null");

        if (TileChain.Count >= 2 && TileChain[TileChain.Count - 1] != tile && TileChain.Contains(tile))
        {
            for (int i = TileChain.Count - 1; i >= 0; i--)
            {
                if (TileChain[i] == tile)
                {
                    break;
                }
                else
                {
                    TileBehaviour removedTile = TileChain[i];
                    removedTile.SelectionBehaviour.SetSelectedState(false);
                    TileChain.RemoveAt(i);

                    OnTileRemovedFromChain?.Invoke(removedTile);
                }
            }
        }
    }

    // COnsume the chain and destroy each of the tiles in it
    public void ConsumeChain()
    {
        // Validation checks
        Debug.Assert(TileChain != null, "Tile chain is null");

        // Cancel the chain if its to short, trigger the consume and destroy methods for the tiles with the set delays
        if (TileChain.Count < 3)
        {
            OnTileChainFailed?.Invoke(TileChain.ToArray());
            ClearChain();
        }
        else
        {
            for (int i = 0; i < TileChain.Count; i++)
            {
                if (tileConsumptionInterval == 0)
                {
                    TileChain[i].ConsumeTile();
                    TileChain[i].DestroyTile();
                }
                else
                {
                    TileChain[i].Invoke("ConsumeTile", i * tileConsumptionInterval);
                    TileChain[i].Invoke("DestroyTile", (i * tileConsumptionInterval) + tileDestructionDelay);
                }
            }

            Invoke("FireTileChainDestroyedEvent", (TileChain.Count * tileConsumptionInterval) + tileDestructionDelay + 0.02f);

            OnTileChainConsumed?.Invoke(TileChain.ToArray());

            ClearChain();
        }
    }

    private void FireTileChainDestroyedEvent()
    {
        OnTileChainDestroyed?.Invoke();
    }

    private void ClearChain()
    {
        for (int i = 0; i < TileChain.Count; i++)
            TileChain[i].SelectionBehaviour.SetSelectedState(false);

        TileChain.Clear();
    }

}
