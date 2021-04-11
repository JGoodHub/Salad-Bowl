using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileChainManager : Singleton<TileChainManager>
{
    private List<TileBehaviour> tileChain;
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
        tileChain = new List<TileBehaviour>();
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
        Debug.Assert(tileChain != null, "Tile chain list is null");
        Debug.Assert(tile != null, "Tile is null");

        if (tileChain.Contains(tile) == false)
        {
            if (tileChain.Count == 0 || tileChain[tileChain.Count - 1].adjacents.Contains(tile))
            {
                if (tileChain.Count == 0 || tileChain[tileChain.Count - 1].type == tile.type)
                {
                    tileChain.Add(tile);
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
                    TileBehaviour removedTile = tileChain[i];
                    removedTile.SelectionBehaviour.SetSelectedState(false);
                    tileChain.RemoveAt(i);

                    OnTileRemovedFromChain?.Invoke(removedTile);
                }
            }
        }
    }

    // COnsume the chain and destroy each of the tiles in it
    public void ConsumeChain()
    {
        // Validation checks
        Debug.Assert(tileChain != null, "Tile chain is null");

        if (tileChain.Count < 3)
        {
            OnTileChainFailed?.Invoke(tileChain.ToArray());
            ClearChain();
        }
        else
        {
            for (int i = 0; i < tileChain.Count; i++)
            {
                if (tileConsumptionInterval == 0)
                {
                    tileChain[i].ConsumeTile();
                    tileChain[i].DestroyTile();
                }
                else
                {
                    tileChain[i].Invoke("ConsumeTile", i * tileConsumptionInterval);
                    tileChain[i].Invoke("DestroyTile", (i * tileConsumptionInterval) + tileDestructionDelay);
                }
            }

            Invoke("FireTileChainDestroyedEvent", (tileChain.Count * tileConsumptionInterval) + tileDestructionDelay + 0.02f);

            OnTileChainConsumed?.Invoke(tileChain.ToArray());

            ClearChain();
        }
    }

    private void FireTileChainDestroyedEvent()
    {
        OnTileChainDestroyed?.Invoke();
    }

    private void ClearChain()
    {
        for (int i = 0; i < tileChain.Count; i++)
            tileChain[i].SelectionBehaviour.SetSelectedState(false);

        tileChain.Clear();
    }

}
