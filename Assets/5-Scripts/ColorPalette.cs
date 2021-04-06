using System.Collections;
using UnityEngine;

public class ColorPalette : Singleton<ColorPalette>
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

    public static void ConvertTileColourToRGB(TileColor tileColor)
    {

    }



}
