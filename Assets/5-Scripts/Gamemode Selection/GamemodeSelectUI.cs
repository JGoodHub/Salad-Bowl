using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamemodeSelectUI : Singleton<GamemodeSelectUI>
{

    public TextMeshProUGUI gamemodeText;
    public RectTransform previewImageTransform;
    public GameObject comingSoonBannerObject;

    public GameObject leftButton;
    public GameObject rightButton;

    public Color availableColor;
    public Color comingSoonColor;

    private Vector2 sourcePosition;

    public float exitSpeed;
    public AnimationCurve exitCurve;

    public float waitTime;

    public float enterSpeed;
    public AnimationCurve enterCurve;

    private int gamemodeIndex;

    private void Start()
    {
        LevelSelectUI.Instance.gameObject.SetActive(false);

        sourcePosition = previewImageTransform.anchoredPosition;
    }

    public void ShowPrevGamemode()
    {
        gamemodeIndex = Mathf.Clamp(gamemodeIndex - 1, 0, GamemodeSelectManager.Instance.gamemodes.Length - 1);
        SelectGamemode(GamemodeSelectManager.Instance.gamemodes[gamemodeIndex], false);
    }

    public void ShowNextGamemode()
    {
        gamemodeIndex = Mathf.Clamp(gamemodeIndex + 1, 0, GamemodeSelectManager.Instance.gamemodes.Length - 1);
        SelectGamemode(GamemodeSelectManager.Instance.gamemodes[gamemodeIndex], true);
    }

    public void SelectLevel()
    {
        MainMenuPanelManager.Instance.SetActivePanel(MainMenuPanelManager.Panels.LEVEL_SELECT);
    }

    public void SelectGamemode(GamemodeSelectManager.Gamemode gamemode, bool exitLeft)
    {
        StopAllCoroutines();
        StartCoroutine(SelectGamemodeCoroutine(gamemode, exitLeft));
    }

    private IEnumerator SelectGamemodeCoroutine(GamemodeSelectManager.Gamemode gamemode, bool exitLeft)
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);

        Vector2 sourceExitOffset = new Vector2(exitLeft ? -Screen.width : Screen.width, 0);
        Vector2 sourceEnterOffset = new Vector2(exitLeft ? Screen.width : -Screen.width, 0);

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

        previewImageTransform.anchoredPosition = sourcePosition + sourceEnterOffset;

        previewImageTransform.GetComponent<Image>().sprite = gamemode.uiImage;
        previewImageTransform.GetComponent<Image>().color = gamemode.available ? availableColor : comingSoonColor;

        gamemodeText.text = gamemode.name;

        comingSoonBannerObject.SetActive(!gamemode.available);

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

    public Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
    {
        return a + ((b - a) * t);
    }

}
