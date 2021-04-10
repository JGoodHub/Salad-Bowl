using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityTileTypeEvent : UnityEvent<TileType> { }
[System.Serializable] public class UnityTileEvent : UnityEvent<TileBehaviour> { }
[System.Serializable] public class UnityTileArrayEvent : UnityEvent<TileBehaviour[]> { }