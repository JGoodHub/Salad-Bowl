using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileData : MonoBehaviour
{


    // Tile state
    public ColorPalette.TileType type;
    public bool selected;
    public Vector2Int gridPosition;

    public List<TileData> adjacents = new List<TileData>();


    // Moving parameters
    public float fallSpeed;

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

    public void DestroyTile()
    {
        OnTileDestroyed?.Invoke(this);

        Destroy(gameObject);
    }

    public void MoveToGridPosition(Vector2Int newPosition)
    {
        if (gridPosition.Equals(newPosition))
            return;

        StopAllCoroutines();
        StartCoroutine(MoveToGridPositionCoroutine(newPosition));
    }

    private IEnumerator MoveToGridPositionCoroutine(Vector2Int newGridPos)
    {
        Vector3 newWorldPosition = TileGrid.Instance.GridToWorldSpace(newGridPos);
        while (transform.position.Equals(newWorldPosition) == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, newWorldPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        gridPosition = newGridPos;
    }
}
