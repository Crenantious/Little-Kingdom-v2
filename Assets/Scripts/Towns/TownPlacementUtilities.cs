using LittleKingdom.Board;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using UnityEngine;

namespace LittleKingdom
{
    public class TownPlacementUtilities
    {
        #region DI

        private readonly RaycastFromPointer raycastFromPointer;
        private readonly IReferences references;

        #endregion

        private readonly Vector2 defaultPointerWorldPosition = new();

        public TownPlacementUtilities(RaycastFromPointer raycastFromPointer, IReferences references)
        {
            this.raycastFromPointer = raycastFromPointer;
            this.references = references;
        }

        public void MoveTownToTile(ITown town, ITile origin)
        {
            float xOffset = -references.TileWidth / 2 + (float)town.Width / 2 * references.TileWidth;
            float yOffset = references.TileHeight / 2 - (float)town.Height / 2 * references.TileHeight;
            town.SetPosition(new Vector2(origin.XPosition + xOffset, origin.YPosition + yOffset));
        }

        public ITile GetTownOriginTile(ITown town) =>
            references.Board.GetTownOriginFromPointerPosition(town, GetWorldspacePointerPosition());

        private Vector2 GetWorldspacePointerPosition() =>
            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            raycastFromPointer.CastTo3D(out RaycastHit hit) ?
                new Vector2(hit.point.x + references.TileWidth / 2, hit.point.z + references.TileHeight / 2) :
                defaultPointerWorldPosition;
    }
}