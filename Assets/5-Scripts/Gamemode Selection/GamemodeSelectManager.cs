using System.Collections;
using UnityEngine;

public class GamemodeSelectManager : Singleton<GamemodeSelectManager>
{
    [System.Serializable]
    public struct Gamemode
    {
        public string name;
        public int buildIndex;
        public Sprite uiImage;
        public bool available;
    }

    public Gamemode[] gamemodes;

}