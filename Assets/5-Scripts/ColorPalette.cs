using System.Collections;
using UnityEngine;

public class ColorPalette : Singleton<ColorPalette>
{


    public Color[] tileColors = new Color[]
    {
        new Color(0.78f, 0.27f, 0.21f),
        new Color(1f, 0.54f, 0f),
        new Color(1f, 0.82f, 0f),
        new Color(0.13f, 0.64f, 0.08f),
        new Color(0.18f, 0.32f, 0.57f),
        new Color(1f, 0f, 0.52f)
    };

    public Color ConvertTileTypeToRGB(Tile.Type tColor, bool hueOnly)
    {
        Debug.Assert((int)tColor >= 0 && (int)tColor < tileColors.Length, "tileColor enum is out of range");

        return hueOnly ? MaxOutSaturationAndValue(tileColors[(int)tColor]) : tileColors[(int)tColor];
    }

    public static Color MaxOutSaturationAndValue(Color color)
    {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);
        return Color.HSVToRGB(hue, 1f, 1f);
    }

}
