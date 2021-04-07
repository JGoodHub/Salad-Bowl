using System.Collections;
using UnityEngine;

public class ColorPalette : Singleton<ColorPalette>
{
    [System.Serializable]
    public struct ColorSet
    {
        public TileData.Type type;
        public Color primary;
        public Color secondary;
        public Color effect;
    }

    //public ColorSet[] tileColorsSet;

    public Color[] tileColors = new Color[]
    {
        new Color(0.78f, 0.27f, 0.21f),
        new Color(1f, 0.54f, 0f),
        new Color(1f, 0.82f, 0f),
        new Color(0.13f, 0.64f, 0.08f),
        new Color(0.18f, 0.32f, 0.57f),
        new Color(1f, 0f, 0.52f)
    };

    private void Start()
    {
        Debug.Assert(System.Enum.GetNames(typeof(TileData.Type)).Length == tileColors.Length, "The TileColors array has to many or to few values based on the number of supported tile types");
    }

    public Color ConvertTileTypeToRGB(TileData.Type tColor, bool hueOnly)
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
