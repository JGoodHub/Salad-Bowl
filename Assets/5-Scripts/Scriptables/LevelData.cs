using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelQuota", menuName = "ScriptableObjects/Create Level Quota")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct TileState
    {
        public TileType type;
        public bool isActive;
    }

    [System.Serializable]
    public struct TileQuota
    {
        public TileType type;
        public int target;
    }

    [Header("Active Tile Toggles")]
    public TileState[] tileStates;

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

    public void Reset()
    {
        string[] tileNames = System.Enum.GetNames(typeof(TileType));

        tileStates = new TileState[tileNames.Length];

        for (int i = 0; i < tileStates.Length; i++)
        {
            tileStates[i].type = (TileType)i;
            tileStates[i].isActive = true;
        }

        tileQuotas = new TileQuota[tileNames.Length];

        for (int i = 0; i < tileQuotas.Length; i++)
        {
            tileQuotas[i].type = (TileType)i;
            tileQuotas[i].target = 0;
        }

        moveLimit = 25;
        completionBonus = 0;
    }

    public TileType[] GetActiveTypes()
    {
        List<TileType> activeTiles = new List<TileType>();

        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tileStates[i].isActive == true)
            {
                activeTiles.Add(tileStates[i].type);
            }
        }

        return activeTiles.ToArray();
    }
}
