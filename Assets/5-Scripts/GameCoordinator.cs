using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : Singleton<GameCoordinator>
{

    public TileData TileData;

    public int LevelIndex;
    public LevelData[] Levels;

    public LevelData ActiveLevel
    {
        get
        {
            return Levels[LevelIndex];
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (dying)
            return;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        foreach (LevelData level in Levels)
        {
            level.boardLayout.Init();

        }
    }

}
