using LittleKingdom.Editor.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace LittleKingdom.Board
{
    [CreateAssetMenu(fileName = "TilesInfo", menuName = "Game/Board and tiles/Tiles info")]
    public class TilesInfo : ScriptableObject
    {
        [SerializeField] private List<TileInfo> tiles;
        [SerializeField, ReadOnly] private float totalTilePercent = 0;

        /// <summary>
        /// Gets the resource tiles information for creating the game board.
        /// </summary>
        public ReadOnlyCollection<TileInfo> Tiles => tiles.AsReadOnly();

        private void OnValidate()
        {
            totalTilePercent = 0;
            tiles.ForEach(t => totalTilePercent += t.PercentOfBoard);
        }
    }
}