using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamemodeSelectUI : Singleton<GamemodeSelectUI>
{

    [Header("UI Elements")]

    public TextMeshProUGUI gamemodeText;
    public RectTransform previewImageTransform;

    public GameObject leftButton;
    public GameObject rightButton;

    [Header("Availability Elements")]

    public Color availableColor;
    public Color comingSoonColor;

    public GameObject comingSoonBannerObject;
    public Button playButton;

    [Header("Animation Parameters")]

    public float exitSpeed;
    public AnimationCurve exitCurve;

    public float waitTime;

    public float enterSpeed;
    public AnimationCurve enterCurve;

    private int gamemodeIndex;
    private Vector2 sourcePosition;

    private void Start()
    {
        sourcePosition = previewImageTransform.anchoredPosition;
    }

    /// <summary>
    /// Decrement the game mode index and play the UI animation
    /// </summary>
    public void ShowPrevGamemode()
    {
        gamemodeIndex = Mathf.Clamp(gamemodeIndex - 1, 0, GamemodeSelectManager.Instance.gamemodes.Length - 1);
        ShowGamemode(GamemodeSelectManager.Instance.gamemodes[gamemodeIndex], false);
    }

    /// <summary>
    /// Increment the game mode index and play the UI animation
    /// </summary>
    public void ShowNextGamemode()
    {
        gamemodeIndex = Mathf.Clamp(gamemodeIndex + 1, 0, GamemodeSelectManager.Instance.gamemodes.Length - 1);
        ShowGamemode(GamemodeSelectManager.Instance.gamemodes[gamemodeIndex], true);
    }

    /// <summary>
    /// Show the level select UI panel
    /// </summary>
    public void SelectLevel()
    {
        MainMenuPanelManager.Instance.SetActivePanel(MainMenuPanelManager.Panels.LEVEL_SELECT);
    }

    /// <summary>
    /// Play the UI to swap out the current gamemode
    /// </summary>
    /// <param name="gamemode">THe gamemode to switch to</param>
    /// <param name="exitLeft">The direction of the swipe animation</param>
    public void ShowGamemode(GamemodeSelectManager.Gamemode gamemode, bool exitLeft)
    {
        StopAllCoroutines();
        StartCoroutine(SelectGamemodeCoroutine(gamemode, exitLeft));
    }


    /// <summary>
    /// Coroutine animation to set the new gamemode UI by swipe the current gamemode off screen and swipe a new gamemode into screen
    /// </summary>
    /// <param name="gamemode">THe gamemode to switch to</param>
    /// <param name="exitLeft">The direction of the swipe animation</param>
    /// <returns></returns>
    private IEnumerator SelectGamemodeCoroutine(GamemodeSelectManager.Gamemode gamemode, bool exitLeft)
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);

        Vector2 sourceExitOffset = new Vector2(exitLeft ? -Screen.width : Screen.width, 0);
        Vector2 sourceEnterOffset = new Vector2(exitLeft ? Screen.width : -Screen.width, 0);

        // Swipe the current gamemode graphic off screen

        float distance = Screen.width;
        float traversalTime = distance / exitSpeed;
        float t = 0;

        while (t < traversalTime)
        {
            previewImageTransform.anchoredPosition = LerpUnclamped(sourcePosition, sourcePosition + sourceExitOffset, exitCurve.Evaluate(t / traversalTime));
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        // Update the static UI elements, e.g. name, sprite, etc.

        previewImageTransform.anchoredPosition = sourcePosition + sourceEnterOffset;

        previewImageTransform.GetComponent<Image>().sprite = gamemode.uiImage;
        previewImageTransform.GetComponent<Image>().color = gamemode.available ? availableColor : comingSoonColor;

        gamemodeText.text = gamemode.name;

        comingSoonBannerObject.SetActive(!gamemode.available);
        playButton.interactable = gamemode.available;

        // Swipe the new gamemode onto screen

        traversalTime = distance / enterSpeed;
        t = 0;

        while (t < traversalTime)
        {
            previewImageTransform.anchoredPosition = LerpUnclamped(sourcePosition + sourceEnterOffset, sourcePosition, enterCurve.Evaluate(t / traversalTime));
            t += Time.deltaTime;
            yield return null;
        }
        previewImageTransform.anchoredPosition = sourcePosition;

        leftButton.SetActive(true);
        rightButton.SetActive(true);
    }

    /// <summary>
    /// Unclamped version of Vector2.Lerp
    /// </summary>
    /// <param name="a">The source vector</param>
    /// <param name="b">The target vector</param>
    /// <param name="t">Interpolation value</param>
    /// <returns>A vector proportionally located between source and target using t</returns>
    public Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
    {
        return a + ((b - a) * t);
    }

}
