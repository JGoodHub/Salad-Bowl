using System.Collections;
using UnityEngine;

public class ColorPalette
{
    [System.Serializable]
    public enum TileColor
    {
        RED,
        ORANGE,
        YELLOW,
        GREEN,
        BLUE,
        PINK
    }

    public static Color ConvertTileColourToRGB(TileColor tileColor, bool maxSatAndVal)
    {
        Color colorRGB;

        switch (tileColor)
        {
            case TileColor.RED:
                colorRGB = new Color(0.78f, 0.27f, 0.21f);
                break;
            case TileColor.ORANGE:
                colorRGB = new Color(1f, 0.54f, 0f);
                break;
            case TileColor.YELLOW:
                colorRGB = new Color(1f, 0.82f, 0f);
                break;
            case TileColor.GREEN:
                colorRGB = new Color(0.13f, 0.64f, 0.08f);
                break;
            case TileColor.BLUE:
                colorRGB = new Color(0.18f, 0.32f, 0.57f);
                break;
            case TileColor.PINK:
                colorRGB = new Color(1f, 0f, 0.52f);
                break;
            default:
                return Color.white;
        }

        return maxSatAndVal ? MaxOutSaturationAndValue(colorRGB) : colorRGB;
    }

    public static Color MaxOutSaturationAndValue(Color color)
    {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);
        return Color.HSVToRGB(hue, 1f, 1f);
    }

}
