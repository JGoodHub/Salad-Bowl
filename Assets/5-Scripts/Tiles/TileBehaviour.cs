using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public TileSelectionBehaviour SelectionBehaviour { get; private set; }
    public TileMovementBehaviour MovementBehaviour { get; private set; }

    public TileType type;
    public List<TileBehaviour> adjacents = new List<TileBehaviour>();

    public UnityTileEvent OnTileInitalise;

    public UnityTileEvent OnTileConsumed;
    public UnityTileEvent OnTileDestroyed;

    private void Awake()
    {
        SelectionBehaviour = GetComponent<TileSelectionBehaviour>();
        MovementBehaviour = GetComponent<TileMovementBehaviour>();

        OnTileInitalise?.Invoke(this);
    }

    public void ConsumeTile()
    {
        OnTileConsumed?.Invoke(this);
    }

    public void DestroyTile()
    {
        OnTileDestroyed?.Invoke(this);

        TileGridManager.Instance.SetTile(MovementBehaviour.gridRef, null);

        Destroy(gameObject);
    }
}
