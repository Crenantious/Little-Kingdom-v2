using LittleKingdom.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class Tile : ITile
    {
        private readonly List<IPlaceableInTile> placeables = new();
        private readonly List<IHoldResources> holders = new();
        private readonly IReferences references;

        public IReadOnlyList<IHoldResources> Holders { get; }

        public ResourceType ResourceType { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }
        public ITown Town { get; set; }

        public Tile(IReferences references)
        {
            this.references = references;
            Holders = holders.AsReadOnly();
        }

        public void Initialise(ResourceType resourceType) =>
            ResourceType = resourceType;

        public void SetPos(Vector2 position)
        {
            XPosition = position.x;
            YPosition = position.y;
        }

        public bool Add(IPlaceableInTile placeable)
        {
            if (placeables.Count == references.MaxPlaceablesOnATile)
                return false;

            placeables.Add(placeable);
            if (Inherits<IHoldResources>(placeable))
                holders.Add((IHoldResources)placeable);

            return true;
        }

        public bool Remove(IPlaceableInTile placeable)
        {
            if (placeables.Count == 0 || placeables.Contains(placeable) is false)
                return false;

            placeables.Remove(placeable);
            if (Inherits<IHoldResources>(placeable))
                holders.Add((IHoldResources)placeable);

            return true;
        }

        private static bool Inherits<T>(IPlaceableInTile placeable) =>
            typeof(T).IsAssignableFrom(placeable.GetType());
    }
}