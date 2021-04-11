using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : Singleton<LevelSelectUI>
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonContainer;

    private GameObject[] levelButtons;

    private void Start()
    {
        for (int c = 0; c < levelButtonContainer.childCount; c++)
        {
            Destroy(levelButtonContainer.GetChild(c).gameObject);
        }

        levelButtons = new GameObject[GameCoordinator.Instance.Levels.Length];
        for (int i = 0; i < GameCoordinator.Instance.Levels.Length; i++)
        {
            int iRef = i;

            GameObject levelButtonObj = Instantiate(levelButtonPrefab, levelButtonContainer);

            levelButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) < 10 ? "0" + (i + 1) : (i + 1).ToString();

            levelButtonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameCoordinator.Instance.LevelIndex = iRef;
                SceneCoordinator.Instance.LaunchPlayScene();
            });

            levelButtons[i] = levelButtonObj;

        }
    }

    public void BackToGamemodeSelection()
    {
        MainMenuPanelManager.Instance.SetActivePanel(MainMenuPanelManager.Panels.GAMEMODE_SELECT);
    }
}
