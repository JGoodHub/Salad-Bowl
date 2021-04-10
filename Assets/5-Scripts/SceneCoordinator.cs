using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCoordinator : Singleton<SceneCoordinator>
{

    public int menuSceneIndex;
    public int playSceneIndex;

    protected override void Awake()
    {
        base.Awake();

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    public void LaunchMenuScene()
    {
        LaunchSceneWithIndex(menuSceneIndex);
    }

    public void LaunchPlayScene()
    {
        LaunchSceneWithIndex(playSceneIndex);
    }

    public void ReloadCurrentScene()
    {
        LaunchSceneWithIndex(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Launches the scene with the specific index in the build settings
    /// </summary>
    /// <param name="sceneIndex">Scene index to launch</param>
    public void LaunchSceneWithIndex(int sceneIndex)
    {
        Debug.Assert(sceneIndex >= 0);
        Debug.Assert(sceneIndex < SceneManager.sceneCountInBuildSettings);

        SceneManager.LoadScene(sceneIndex);
    }

}
