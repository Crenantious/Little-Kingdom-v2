using LittleKingdom.Board;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Resources
{
    public class AllOnTileGRH : IGetResourceHolders
    {
        private static readonly string NullPlaceableError =
            $"{nameof(placeable)} cannot be null with the {nameof(Tile.OriginTile)} option selected.";
        private static readonly string InvalidTileError = $"Must set to a valid value of {nameof(Tile)}.";
        private static readonly string TileNotSetError = $"Value must be set; cannot be {nameof(Tile.None)}.";

        [SerializeField]
        [Tooltip("Used to get the tile it is on if the " + nameof(Tile.OriginTile) + " option is selected.")]
        private IPlaceableInTile placeable;

        [SerializeField] private Tile tile;

        public enum Tile
        {
            None,

            // Only for entities that inherit IPlaceableInTile. Uses the origin tile the entity is on.
            OriginTile
        }

        public IEnumerable<IHoldResources> Get() =>
            tile switch
            {
                Tile.OriginTile => GetForCurrentTile(),
                Tile.None => throw new ArgumentOutOfRangeException(nameof(tile), TileNotSetError),
                _ => throw new ArgumentOutOfRangeException(nameof(tile), InvalidTileError),
            };

        public IEnumerable<IHoldResources> GetForCurrentTile()
        {
            if (placeable is null)
                throw new NullReferenceException(NullPlaceableError);

            return placeable.OriginTile.Holders;
        }
    }
}