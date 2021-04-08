﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovesTrackerBehaviour : MonoBehaviour
{
    private int movesRemaining;

    [Header("Event Triggers")]
    [Space]

    public UnityEvent OnMovesExhausted;

    private void Start()
    {
        TileChainManager.Instance.OnTileChainConsumed.AddListener(DecrementMovesCounter);

        movesRemaining = GameCoordinator.Instance.LevelQuota.moveLimit;
        MovesTrackerUI.Instance.SetMovesRemaining(movesRemaining);
    }

    private void DecrementMovesCounter(Tile[] tilechain)
    {
        movesRemaining--;

        MovesTrackerUI.Instance.SetMovesRemaining(movesRemaining);

        if (movesRemaining <= 0)
        {
            OnMovesExhausted?.Invoke();
        }
    }
}