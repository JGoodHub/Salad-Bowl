using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityTileTypeEvent : UnityEvent<TileSelectionBehaviour.Type> { }
[System.Serializable] public class UnityTileDataEvent : UnityEvent<TileSelectionBehaviour> { }
[System.Serializable] public class UnityTileArrayEvent : UnityEvent<TileSelectionBehaviour[]> { }