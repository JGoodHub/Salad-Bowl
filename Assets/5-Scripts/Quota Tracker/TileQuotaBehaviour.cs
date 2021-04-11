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

    private void OnTileChainDestroyed()
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
            TileGridManager.Instance.gridLocked = true;
        }
    }
}
