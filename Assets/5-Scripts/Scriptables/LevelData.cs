using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelQuota", menuName = "ScriptableObjects/Create Level Quota")]
public class LevelData : ScriptableObject
{

    [System.Serializable]
    public struct TileQuota
    {
        public TileType type;
        public int target;
    }

    [Header("Active Tiles")]
    public TileType[] activeTiles;

    [Header("Tile Quotas")]
    public TileQuota[] tileQuotas;

    [Header("Board Layout")]
    public BoardData boardLayout;

    [Header("Limits & Bonuses")]
    public int moveLimit;
    public int completionBonus;

    public int GetTargetForType(TileType type)
    {
        for (int i = 0; i < tileQuotas.Length; i++)
        {
            if (tileQuotas[i].type == type)
            {
                return tileQuotas[i].target;
            }
        }

        return 0;
    }
}
