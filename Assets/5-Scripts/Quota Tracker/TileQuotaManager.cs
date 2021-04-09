using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileQuotaManager : Singleton<TileQuotaManager>
{
    [Header("Level Profile")]

    public LevelQuotaData levelQuota;

    private Dictionary<TileType, int> counters;

    [Header("Event Triggers")]
    [Space]

    public UnityTileTypeEvent OnQuotaCompleted;
    public UnityEvent OnAllQuotasCompleted;

    private void Start()
    {
        TileCounterUI.Instance.CreateAndPopulateQuotaEntries(levelQuota.quotas);

        TileChainManager.Instance.OnTileChainConsumed.AddListener(OnTileChainCompleted);

        counters = new Dictionary<TileType, int>();
        for (int i = 0; i < levelQuota.quotas.Length; i++)
            counters.Add(levelQuota.quotas[i].type, 0);
    }


    private void OnTileChainCompleted(TileSelectionBehaviour[] tileChain)
    {
        if (tileChain == null || tileChain.Length == 0 || counters.ContainsKey(tileChain[0].type) == false)
            return;

        TileType chainType = tileChain[0].type;
        int typeTarget = levelQuota.GetTargetForType(chainType);

        counters[chainType] += tileChain.Length;

        TileCounterUI.Instance.SetCounterForType(chainType, Mathf.Clamp(counters[chainType], 0, typeTarget));
        TileCounterUI.Instance.SetCompletetionForType(chainType, counters[chainType] >= typeTarget);

        if (counters[chainType] >= typeTarget && counters[chainType] - tileChain.Length < typeTarget)
        {
            OnQuotaCompleted?.Invoke(chainType);
            CheckAllQuotasComplete();
        }
    }

    private void CheckAllQuotasComplete()
    {
        bool allComplete = true;
        foreach (TileType type in counters.Keys)
        {
            if (counters[type] < levelQuota.GetTargetForType(type))
            {
                allComplete = false;
            }
        }

        if (allComplete)
        {
            OnAllQuotasCompleted?.Invoke();
        }
    }

}
