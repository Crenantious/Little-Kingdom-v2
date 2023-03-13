using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace LittleKingdom.Tiles
{
    [CreateAssetMenu(fileName = "TilesInfo", menuName = "Game/Board and tiles/Tiles info")]
    public class TilesInfo : ScriptableObject
    {
        [SerializeField]
        private List<TileInfo> tiles;

        public ReadOnlyCollection<TileInfo> Tiles => tiles.AsReadOnly();
    }
}