using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovesTrackerUI : Singleton<MovesTrackerUI>
{

    public TextMeshProUGUI movesRemainingText;

    public void SetMovesRemaining(int remaining)
    {
        remaining = Mathf.Clamp(remaining, 0, int.MaxValue);

        movesRemainingText.text = remaining.ToString();
    }
}
