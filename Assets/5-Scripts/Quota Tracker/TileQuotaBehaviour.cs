using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileQuotaBehaviour : Singleton<TileQuotaBehaviour>
{
    private LevelData level;
    private Dictionary<TileType, int> counters;

    [Header("Event Triggers")]
    [Space]

    public UnityTileTypeEvent OnQuotaCompleted;
    public UnityEvent OnAllQuotasCompleted;

    /// <summary>
    /// Subscribe to tile chain events and create a counter for each of the tile types
    /// </summary>
    private void Start()
    {
        level = GameCoordinator.Instance.ActiveLevel;

        TileQuotaUI.Instance.CreateAndPopulateQuotaEntries(level.tileQuotas);

        TileChainManager.Instance.OnTileChainConsumed.AddListener(OnTileChainCompleted);
        TileChainManager.Instance.OnTileChainDestroyed.AddListener(OnTileChainDestroyed);

        counters = new Dictionary<TileType, int>();
        for (int i = 0; i < level.tileQuotas.Length; i++)
        {
            counters.Add(level.tileQuotas[i].type, 0);
        }
    }

    /// <summary>
    /// When a tile chain is completed add those tiles to the counter and check if the quota is fulfilled and fire the associated event
    /// </summary>
    /// <param name="tileChain"></param>
    private void OnTileChainCompleted(TileBehaviour[] tileChain)
    {
        if (tileChain == null || tileChain.Length == 0 || counters.ContainsKey(tileChain[0].type) == false)
            return;

        TileType chainType = tileChain[0].type;
        int typeTarget = level.GetTargetForType(chainType);

        counters[chainType] += tileChain.Length;

        TileQuotaUI.Instance.SetCounterForType(chainType, Mathf.Clamp(counters[chainType], 0, typeTarget));
        TileQuotaUI.Instance.SetCompletetionForType(chainType, counters[chainType] >= typeTarget);

        if (counters[chainType] >= typeTarget && counters[chainType] - tileChain.Length < typeTarget)
        {
            OnQuotaCompleted?.Invoke(chainType);
        }
    }

    /// <summary>
    /// Check if a quotas have been fulfilled and fire the associated event
    /// </summary>
    public bool CheckAllQuotasComplete()
    {
        bool allComplete = true;
        foreach (TileType type in counters.Keys)
        {
            if (counters[type] < level.GetTargetForType(type))
            {
                allComplete = false;
            }
        }

        if (allComplete)
        {
            OnAllQuotasCompleted?.Invoke();
        }

        return allComplete;
    }


    private void OnTileChainDestroyed()
    {
        CheckAllQuotasComplete();
    }


}
