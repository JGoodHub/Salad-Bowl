using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TileBehaviour))]
public class TileSelectionBehaviour : MonoBehaviour
{
    public TileBehaviour ParentBehaviour { get => GetComponent<TileBehaviour>(); }

    // Tile state
    private bool selected;

    // Tile events
    public UnityTileEvent OnTileSelected;
    public UnityTileEvent OnTileUnselected;

    // Start a new chain on this tile
    private void OnMouseDown()
    {
        if (TileGridManager.Instance.gridLocked == false)
            TileChainManager.Instance.StartNewChainFromTile(ParentBehaviour);
    }

    // Consume the current chain we've just dragged
    private void OnMouseUp()
    {
        if (TileGridManager.Instance.gridLocked == false)
            TileChainManager.Instance.ConsumeChain();
    }

    // Add or remove this tile based on its selected status
    private void OnMouseEnter()
    {
        if (TileGridManager.Instance.gridLocked == false && Input.GetMouseButton(0))
        {
            if (selected)
            {
                TileChainManager.Instance.TrimChainToTile(ParentBehaviour);
            }
            else
            {
                TileChainManager.Instance.AddTileToChain(ParentBehaviour);
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
            OnTileSelected?.Invoke(ParentBehaviour);
        }
        else
        {
            OnTileUnselected?.Invoke(ParentBehaviour);
        }
    }
}