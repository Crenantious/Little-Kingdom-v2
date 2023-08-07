using LittleKingdom.Attributes;
using LittleKingdom.Board;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [CreateAssetMenu(menuName = "Game/Loading/Configs/Board")]
    public class BoardLC : LoaderConfig
    {
        [field: SerializeField] public int WidthInTiles { get; private set; }
        [field: SerializeField] public int HeightInTiles { get; private set; }

        // For inspector information.
        [SerializeField, ReadOnly] private float totalTilePercent = 0;
        [SerializeField] private List<TileInfo> tileInfo;

        /// <summary>
        /// Gets the resource tiles information for creating the game board.
        /// </summary>
        public ReadOnlyCollection<TileInfo> TileInfo => tileInfo.AsReadOnly();

        private void OnValidate()
        {
            totalTilePercent = 0;
            tileInfo.ForEach(t => totalTilePercent += t.PercentOfBoard);
        }
    }
}