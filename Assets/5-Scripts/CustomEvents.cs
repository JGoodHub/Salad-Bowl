using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class UnityTileTypeEvent : UnityEvent<Tile.Type> { }
[System.Serializable] public class UnityTileDataEvent : UnityEvent<Tile> { }
[System.Serializable] public class UnityTileChainEvent : UnityEvent<Tile[]> { }
