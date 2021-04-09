using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileCounterEntry : MonoBehaviour
{
    [SerializeField] private Image tileImage;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private Image tickImage;

    private void Start()
    {
        Debug.Assert(tileImage != null, "Tile image is null");
        Debug.Assert(counterText != null, "Counter text is null");
        Debug.Assert(totalText != null, "Total text is null");
        Debug.Assert(tickImage != null, "Tick image is null");
    }

    public void SetImage(Sprite sprite)
    {
        tileImage.sprite = sprite;
    }

    public void SetCounter(int total)
    {
        counterText.text = total.ToString();
    }

    public void SetTotal(int total)
    {
        totalText.text = total.ToString();
    }

    public void SetCompleted(bool completed)
    {
        tickImage.enabled = completed;
    }

}
