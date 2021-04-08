﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{

    [System.Serializable]
    public enum Type
    {
        RED,
        ORANGE,
        YELLOW,
        GREEN,
        BLUE,
        PINK
    }


    // Tile state
    public Type type;
    public bool selected;
    public Vector2Int gridRef;

    public List<Tile> adjacents = new List<Tile>();

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
        TileChainManager.Instance.StartNewChainFromTile(this);
    }

    private void OnMouseUp()
    {
        TileChainManager.Instance.ConsumeChain();
    }

    // Add or remove this tile based on its selected status
    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
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

    public void MoveToGridRef(int x, int y)
    {
        MoveToGridRef(new Vector2Int(x, y));
    }

    public void MoveToGridRef(Vector2Int newPosition)
    {
        if (gridRef.Equals(newPosition))
            return;

        StopAllCoroutines();
        StartCoroutine(MoveToGridPositionCoroutine(newPosition));
    }

    private IEnumerator MoveToGridPositionCoroutine(Vector2Int newGridPos)
    {
        Vector3 newWorldPosition = TileGridController.Instance.GridToWorldSpace(newGridPos);
        while (transform.position.Equals(newWorldPosition) == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, newWorldPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        gridRef = newGridPos;
    }
}