using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{

    public GameObject winScreen;
    public GameObject nextLevelButton;

    public GameObject loseScreen;

    private void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

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

    public void DisplayLoseScreen()
    {
        if (winScreen.activeSelf == true)
            return;

        loseScreen.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneCoordinator.Instance.LaunchMenuScene();
    }

    public void RetryLevel()
    {
        SceneCoordinator.Instance.ReloadCurrentScene();
    }

    public void NextNevel()
    {
        GameCoordinator.Instance.LevelIndex++;
        SceneCoordinator.Instance.ReloadCurrentScene();
    }

}