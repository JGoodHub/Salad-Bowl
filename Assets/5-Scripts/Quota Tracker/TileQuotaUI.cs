using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileQuotaUI : Singleton<TileQuotaUI>
{

    public GameObject tileCounterPrefab;
    public Transform counterEntriesParent;

    private Dictionary<TileType, TileQuotaEntry> counterEntries;

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

    public void SetCounterForType(TileType type, int newCount)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCounter(newCount);
    }

    public void SetCompletetionForType(TileType type, bool complete)
    {
        if (counterEntries.ContainsKey(type) == false)
            return;

        counterEntries[type].SetCompleted(complete);
    }


}
