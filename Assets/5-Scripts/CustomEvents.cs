using System.Collections;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable] public class UnityTileTypeEvent : UnityEvent<TileType> { }
[System.Serializable] public class UnityTileDataEvent : UnityEvent<TileSelectionBehaviour> { }
[System.Serializable] public class UnityTileArrayEvent : UnityEvent<TileSelectionBehaviour[]> { }