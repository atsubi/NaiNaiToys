using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapList {

    public class TilemapProvider : MonoBehaviour
    {
        [Header("床のタイルマップ"), SerializeField]
        public Tilemap FloorTileMap;

        [Header("壁のタイルマップ"), SerializeField]
        public Tilemap WallTileMap;

        [Header("アイテムのタイルマップ"), SerializeField]
        public Tilemap ItemTileMap;
    }
}