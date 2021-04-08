using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : Singleton<GameCoordinator>
{

    [SerializeField] private BoardLayoutData boardLayoutData;
    public BoardLayoutData BoardLayout
    {
        get { return boardLayoutData; }
    }

    [SerializeField] private LevelQuotaData levelQuotaData;
    public LevelQuotaData LevelQuota
    {
        get { return levelQuotaData; }
    }

    [SerializeField] private TileLoadoutsData tileLoadoutsData;
    public TileLoadoutsData TileLoadouts
    {
        get { return tileLoadoutsData; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        BoardLayout.Init();
    }

}
