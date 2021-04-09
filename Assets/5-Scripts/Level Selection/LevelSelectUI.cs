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

        levelButtons = new GameObject[LevelSelectManager.Instance.levels.Length];
        for (int i = 0; i < LevelSelectManager.Instance.levels.Length; i++)
        {
            int iRef = i;

            GameObject levelButtonObj = Instantiate(levelButtonPrefab, levelButtonContainer);

            levelButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) < 10 ? "0" + (i + 1) : (i + 1).ToString();

            levelButtonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                LevelSelectManager.Instance.AssignLevelToCoordinator(iRef);
                SceneCoordinator.Instance.LaunchPlayScene();
            });

            levelButtons[i] = levelButtonObj;

        }
    }

}
