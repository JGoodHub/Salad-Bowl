using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileData : MonoBehaviour
{


    // Tile state
    public ColorPalette.TileType color;
    public bool selected;

    public List<TileData> adjacents = new List<TileData>();


    // Tile events
    public UnityTileDataEvent OnTileInitalise;

    public UnityTileDataEvent OnTileSelected;
    public UnityTileDataEvent OnTileUnselected;

    public UnityTileDataEvent OnTileConsumed;
    public UnityTileDataEvent OnTileDestroyed;

    private void Start()
    {
        OnTileInitalise?.Invoke(this);
    }

    // Start a new chain on this tile
    private void OnMouseDown()
    {
        TileLogic.Instance.StartNewChainFromTile(this);
    }

    private void OnMouseUp()
    {
        TileLogic.Instance.CommitChain();
    }

    // Add or remove this tile based on its selected status
    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            if (selected)
            {
                TileLogic.Instance.TrimChainToTile(this);
            }
            else
            {
                TileLogic.Instance.AddTileToChain(this);
            }
        }
    }

    // Toggle the selected state to the opposite of its current value
    public void ToggleSelectedState()
    {
        SetSelectedState(!selected);
    }

    // Set the selected state to the value passed, invoking events where appropriate
    public void SetSelectedState(bool state)
    {
        selected = state;

        if (selected)
        {
            OnTileSelected?.Invoke(this);
        }
        else
        {
            OnTileUnselected?.Invoke(this);
        }
    }

    public void ConsumeTile()
    {
        OnTileConsumed?.Invoke(this);
    }



}
