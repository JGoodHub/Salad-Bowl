using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileData : MonoBehaviour
{


    // Tile state
    public ColorPalette.TileColor color;
    public bool selected;

    public List<TileData> adjacents = new List<TileData>();


    // Tile events
    public UnityEvent OnTileInitalise;

    public UnityEvent OnTileSelected;
    public UnityEvent OnTileUnselected;


    private void Start()
    {
        OnTileInitalise?.Invoke();
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
                Debug.Log("ADD");
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
            Debug.Log(gameObject.name + " Selected");
            OnTileSelected?.Invoke();
        }
        else
        {
            OnTileUnselected?.Invoke();
        }
    }

}
