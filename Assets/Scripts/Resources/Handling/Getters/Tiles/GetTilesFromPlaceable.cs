using LittleKingdom.Board;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Resources
{
    public class GetTilesFromPlaceable : IGetTiles
    {
        private static readonly string InvalidTileOptionError = $"{nameof(Option)} must be a valid value.";

        [field: SerializeField] public IPlaceable Placeable { get; private set; }
        [field: SerializeField] public TileOption Option { get; private set; }

        public enum TileOption
        {
            OriginTile,
            AllTiles
        }

        /// <returns>The tiles from <see cref="Placeable"/> based on <see cref="Option"/>.</returns>
        /// <exception cref="NullReferenceException">If <see cref="Placeable"/> is not set.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If given an invalid <see cref="TileOption"/></exception>
        public IEnumerable<ITile> Get() =>
            Option switch
            {
                TileOption.OriginTile => new ITile[] { Placeable.OriginTile },
                TileOption.AllTiles => Placeable.Tiles.GetEnumerable(),
                _ => throw new ArgumentOutOfRangeException(nameof(Option), InvalidTileOptionError),
            };
    }
}