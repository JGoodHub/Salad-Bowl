using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileLoadouts", menuName = "ScriptableObjects/Create Tile Loadouts")]
public class TileLoadoutsData : ScriptableObject
{
    [System.Serializable]
    public struct TileLoadout
    {
        public string name;
        public TileSelectionBehaviour.Type type;
        public int score;
        public Sprite image;
        public Color primaryColor;

        public TileLoadout(string name, TileSelectionBehaviour.Type type, int score, Sprite image, Color primaryColor)
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

        loadouts[0] = new TileLoadout("Red", TileSelectionBehaviour.Type.RED, 5, null, new Color(0.78f, 0.27f, 0.21f));
        loadouts[1] = new TileLoadout("Orange", TileSelectionBehaviour.Type.ORANGE, 5, null, new Color(1f, 0.54f, 0f));
        loadouts[2] = new TileLoadout("Yellow", TileSelectionBehaviour.Type.YELLOW, 5, null, new Color(1f, 0.82f, 0f));
        loadouts[3] = new TileLoadout("Green", TileSelectionBehaviour.Type.GREEN, 5, null, new Color(0.13f, 0.64f, 0.08f));
        loadouts[4] = new TileLoadout("Blue", TileSelectionBehaviour.Type.BLUE, 5, null, new Color(0.18f, 0.32f, 0.57f));
        loadouts[5] = new TileLoadout("Pink", TileSelectionBehaviour.Type.PINK, 5, null, new Color(1f, 0f, 0.52f));
    }

    private void OnValidate()
    {
        Debug.Assert(System.Enum.GetNames(typeof(TileSelectionBehaviour.Type)).Length == loadouts.Length, "The properties array has to many or to few values based on the number of supported tile types");
    }

    public TileLoadout GetLoadoutByType(TileSelectionBehaviour.Type type)
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

    public Color ConvertTileTypeToRGB(TileSelectionBehaviour.Type type, bool hueOnly)
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
