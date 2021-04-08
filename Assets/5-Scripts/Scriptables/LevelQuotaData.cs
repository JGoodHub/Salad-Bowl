using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelQuota", menuName = "ScriptableObjects/Create Level Quota")]
public class LevelQuotaData : ScriptableObject
{
    [System.Serializable]
    public struct Quota
    {
        public string name;
        public Tile.Type type;
        public int target;
    }

    [Header("Colour Quotas")]
    public Quota[] quotas;

    [Header("Limits & Bonuses")]
    public int moveLimit;
    public int completionBonus;


    public int GetTargetForType(Tile.Type type)
    {
        for (int i = 0; i < quotas.Length; i++)
        {
            if (quotas[i].type == type)
            {
                return quotas[i].target;
            }
        }

        return 0;
    }
}
