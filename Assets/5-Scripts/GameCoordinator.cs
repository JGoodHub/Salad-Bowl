using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : Singleton<GameCoordinator>
{

    public TileData TileData;
    public LevelData LevelData;

    protected override void Awake()
    {
        base.Awake();

        if (dying)
            return;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        LevelData.boardLayout.Init();
    }

}
