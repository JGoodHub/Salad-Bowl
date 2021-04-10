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

    [SerializeField] private TileLoadoutsData tileLoadoutsData;
    public TileLoadoutsData TileLoadouts
    {
        get { return tileLoadoutsData; }
    }

    [SerializeField] private LevelQuotaData levelQuotaData;
    public LevelQuotaData LevelQuota
    {
        get { return levelQuotaData; }
        set { levelQuotaData = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        BoardLayout.Init();
    }



}
