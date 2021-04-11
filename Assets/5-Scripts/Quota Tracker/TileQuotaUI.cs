using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileQuotaUI : Singleton<TileQuotaUI>
{

    public GameObject tileCounterPrefab;
    public Transform counterEntriesParent;

    private Dictionary<TileType, TileQuotaEntry> counterEntries;

    /// <summary>
    /// Create and initialise entries for each of the levels tile quotas
    /// </summary>
    /// <param name="quotas"></param>
    public void CreateAndPopulateQuotaEntries(LevelData.TileQuota[] quotas)
    {
        counterEntries = new Dictionary<TileType, TileQuotaEntry>();

        for (int i = 0; i < quotas.Length; i++)
        {
            if (quotas[i].target == 0)
                continue;

            TileQuotaEntry counterEntry = Instantiate(tileCounterPrefab, counterEntriesParent).GetComponent<TileQuotaEntry>();

            counterEntry.SetImage(GameCoordinator.Instance.TileData.GetLoadoutByType(quotas[i].type).image);
            counterEntry.SetCounter(0);
            counterEntry.SetTotal(quotas[i].target);
            counterEntry.SetCompleted(false);

            counterEntries.Add(quotas[i].type, counterEntry);
        }
    }

    /// <summary>
    /// Update the counter text for the quota counter
    /// </summary>
    /// <param name="type">The quota type to update</param>
    /// <param name="newCount">The new value to use</param>
    public void SetCounterForType(TileType type, int newCount)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCounter(newCount);
    }

    /// <summary>
    /// Set the completed state of the quota
    /// </summary>
    /// <param name="type">The quota type to update</param>
    /// <param name="complete">The completion state</param>
    public void SetCompletetionForType(TileType type, bool complete)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCompleted(complete);
    }

}