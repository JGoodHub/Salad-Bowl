using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    PINK
}

[CreateAssetMenu(fileName = "TileLoadouts", menuName = "ScriptableObjects/Create Tile Loadouts")]
public class TileLoadoutsData : ScriptableObject
{
    [System.Serializable]
    public struct TileLoadout
    {
        public string name;
        public TileType type;
        public int score;
        public Sprite image;
        public Color primaryColor;

        public TileLoadout(string name, TileType type, int score, Sprite image, Color primaryColor)
        {
            this.name = name;
            this.type = type;
            this.score = score;
            this.image = image;
            this.primaryColor = primaryColor;
        }
    }

    public TileLoadout[] loadouts;

    private void Reset()
    {
        loadouts = new TileLoadout[6];

        loadouts[0] = new TileLoadout("Red", TileType.RED, 5, null, new Color(0.78f, 0.27f, 0.21f));
        loadouts[1] = new TileLoadout("Orange", TileType.ORANGE, 5, null, new Color(1f, 0.54f, 0f));
        loadouts[2] = new TileLoadout("Yellow", TileType.YELLOW, 5, null, new Color(1f, 0.82f, 0f));
        loadouts[3] = new TileLoadout("Green", TileType.GREEN, 5, null, new Color(0.13f, 0.64f, 0.08f));
        loadouts[4] = new TileLoadout("Blue", TileType.BLUE, 5, null, new Color(0.18f, 0.32f, 0.57f));
        loadouts[5] = new TileLoadout("Pink", TileType.PINK, 5, null, new Color(1f, 0f, 0.52f));
    }

    private void OnValidate()
    {
        Debug.Assert(System.Enum.GetNames(typeof(TileType)).Length == loadouts.Length, "The properties array has to many or to few values based on the number of supported tile types");
    }

    public TileLoadout GetLoadoutByType(TileType type)
    {
        for (int i = 0; i < loadouts.Length; i++)
        {
            if (loadouts[i].type == type)
            {
                return loadouts[i];
            }
        }

        throw new System.InvalidOperationException();
    }

    public Color ConvertTileTypeToRGB(TileType type, bool hueOnly)
    {
        return hueOnly ? MaxOutSaturationAndValue(GetLoadoutByType(type).primaryColor) : GetLoadoutByType(type).primaryColor;
    }

    public static Color MaxOutSaturationAndValue(Color color)
    {
        float hue, sat, val;
        Color.RGBToHSV(color, out hue, out sat, out val);
        return Color.HSVToRGB(hue, 1f, 1f);
    }
}
