using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : Singleton<LevelSelectManager>
{

    public LevelQuotaData[] levels;


    public void AssignLevelToCoordinator(int levelIndex)
    {
        GameCoordinator.Instance.LevelQuota = levels[levelIndex];
    }

}
