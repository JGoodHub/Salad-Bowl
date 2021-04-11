using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelManager : Singleton<MainMenuPanelManager>
{
    public enum Panels
    {
        GAMEMODE_SELECT,
        LEVEL_SELECT
    }

    public Panels activePanel;

    public GameObject gamemodePanel;
    public GameObject levelSelectPanel;

    private void Start()
    {
        SetActivePanel(activePanel);
    }

    public void SetActivePanel(Panels panel)
    {
        gamemodePanel.SetActive(panel == Panels.GAMEMODE_SELECT);
        levelSelectPanel.SetActive(panel == Panels.LEVEL_SELECT);
    }

}
