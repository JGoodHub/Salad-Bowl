using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [Header("Win Panel Elements")]
    public GameObject winScreen;
    public GameObject nextLevelButton;

    [Header("Lose Panel Elements")]
    public GameObject loseScreen;

    /// <summary>
    /// Hide both win and lose screens
    /// </summary>
    private void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    /// <summary>
    /// DIsplay the win screen if the lose screen isn't already active
    /// </summary>
    public void DisplayWinScreen()
    {
        if (loseScreen.activeSelf == true)
            return;

        winScreen.SetActive(true);

        if (GameCoordinator.Instance.LevelIndex == GameCoordinator.Instance.Levels.Length - 1)
        {
            nextLevelButton.SetActive(false);
        }
    }

    /// <summary>
    /// Display the lose screen if the win screen isn't already active
    /// </summary>
    public void DisplayLoseScreen()
    {
        if (winScreen.activeSelf == true)
            return;

        loseScreen.SetActive(true);
    }

    public void LoadMenuScene()
    {
        SceneCoordinator.Instance.LaunchMenuScene();
    }

    public void ReloadLevel()
    {
        SceneCoordinator.Instance.ReloadCurrentScene();
    }

    public void LoadNextLevel()
    {
        GameCoordinator.Instance.LevelIndex++;
        SceneCoordinator.Instance.ReloadCurrentScene();
    }

}