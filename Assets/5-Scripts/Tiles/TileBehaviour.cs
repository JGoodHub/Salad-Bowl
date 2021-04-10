using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{

    public TileSelectionBehaviour SelectionBehaviour { get; private set; }
    public TileMovementBehaviour MovementBehaviour { get; private set; }

    private void Start()
    {
        SelectionBehaviour = GetComponent<TileSelectionBehaviour>();
        MovementBehaviour = GetComponent<TileMovementBehaviour>();
    }

}
