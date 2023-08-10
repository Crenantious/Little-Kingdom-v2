using LittleKingdom.Board;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using System;
using UnityEngine;

namespace LittleKingdom
{
    public class TownPlacementUtilities
    {
        #region DI

        private readonly InputUtility inputUtility;
        private readonly InGameInput inGameInput;

        #endregion

        private readonly Vector2 defaultPointerWorldPosition = new();

        public TownPlacementUtilities(InputUtility inputUtility, InGameInput inGameInput)
        {
            this.inputUtility = inputUtility;
            this.inGameInput = inGameInput;
        }

        public void MoveTownToTile(ITown town, ITile origin)
        {
            float xOffset = MathF.Ceiling((float)town.Width / 2) * (References.TileWidth / 2);
            float zOffset = -MathF.Ceiling((float)town.Height / 2) * (References.TileHeight / 2);
            town.SetPosition(new Vector3(origin.XPosition + xOffset, 0, origin.YPosition + zOffset));
        }

        public ITile GetTownOriginTile(ITown town) =>
            References.Board.GetTownOriginFromPointerPosition(town, GetWorldspacePointerPosition());

        private Vector2 GetWorldspacePointerPosition() =>
            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) ?
                new Vector2(hit.point.x + References.TileWidth / 2, hit.point.z + References.TileHeight / 2) :
                defaultPointerWorldPosition;
    }
}