using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterUI : MonoBehaviour
{

    public bool musicOn = true;
    public AudioSource musicSource;

    public GameObject musicOnImage;
    public GameObject musicOffImage;

    /// <summary>
    /// Initialise the music muted state
    /// </summary>
    private void Start()
    {
        musicSource.mute = !musicOn;
        musicOnImage.SetActive(musicOn);
        musicOffImage.SetActive(!musicOn);
    }

    public void ReturnToMenu()
    {
        SceneCoordinator.Instance.LaunchMenuScene();
    }

    /// <summary>
    /// Toggle the game music
    /// </summary>
    public void ToggleMusicMuted()
    {
        musicOn = !musicOn;

        musicSource.mute = !musicOn;
        musicOnImage.SetActive(musicOn);
        musicOffImage.SetActive(!musicOn);
    }

}