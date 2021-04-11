using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : Singleton<LevelSelectManager>
{

    public LevelData[] levels;


    public void AssignLevelToCoordinator(int levelIndex)
    {
        GameCoordinator.Instance.LevelIndex = levelIndex;
    }

}
