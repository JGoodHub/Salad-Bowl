using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TileSelectionBehaviour : MonoBehaviour
{

    // Tile state
    public TileType type;
    private bool selected;
    public List<TileSelectionBehaviour> adjacents = new List<TileSelectionBehaviour>();

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
        if (TileGridManager.Instance.GridLocked == false)
            TileChainManager.Instance.StartNewChainFromTile(this);
    }

    private void OnMouseUp()
    {
        if (TileGridManager.Instance.GridLocked == false)
            TileChainManager.Instance.ConsumeChain();
    }

    // Add or remove this tile based on its selected status
    private void OnMouseEnter()
    {
        if (TileGridManager.Instance.GridLocked == false && Input.GetMouseButton(0))
        {
            if (selected)
            {
                TileChainManager.Instance.TrimChainToTile(this);
            }
            else
            {
                TileChainManager.Instance.AddTileToChain(this);
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

    public void DestroyTile()
    {
        OnTileDestroyed?.Invoke(this);

        Destroy(gameObject);
    }
}
