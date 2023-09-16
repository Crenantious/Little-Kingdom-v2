using LittleKingdom.Board;
using LittleKingdom.Loading;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class AutomaticTownPlacement : ITownPlacement
    {
        #region DI

        private readonly TileEntityAssignment tileEntityAssignment;
        private readonly TownPlacementUtilities townPlacementUtilities;
        private readonly List<Vector2Int> originTiles;
        private readonly IBoard board;

        #endregion

        public event SimpleEventHandler<ITown> TownPlaced;

        public AutomaticTownPlacement(TileEntityAssignment tileEntityAssignment,
            TownPlacementUtilities townPlacementUtilities, LoaderProfile loaderProfile,
            IReferences references)
        {
            this.tileEntityAssignment = tileEntityAssignment;
            this.townPlacementUtilities = townPlacementUtilities;
            originTiles = loaderProfile.GetConfig<TownLC>().TownOriginTiles;
            board = references.Board;
        }

        public void Place(ITown town)
        {
            if (town.Player.Number >= originTiles.Count)
            {
                Debug.LogError($"Not enough origin tiles specified in {nameof(TownLC)} for automatic placement.");
                return;
            }

            ITile originTile = board.Tiles.Get(originTiles[town.Player.Number]);
            townPlacementUtilities.MoveTownToTile(town, originTile);
            tileEntityAssignment.AssignTown(town, originTile);
            TownPlaced.Invoke(town);
        }
    }
}