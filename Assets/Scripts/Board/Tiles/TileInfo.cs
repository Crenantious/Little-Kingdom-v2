using System;
using UnityEngine;

namespace LittleKingdom.Board
{
    [Serializable]
    public class TileInfo
    {
        [SerializeField] private Texture texture;
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private float percentOfBoard;

        /// <summary>
        /// The texture to be used for the tile's material.
        /// </summary>
        public Texture Texture => texture;

        /// <summary>
        /// The name of the resource the tile produces.
        /// </summary>
        public ResourceType ResourceType => resourceType;

        /// <summary>
        /// The percent of the board that should be filled with type of tile this class is for.
        /// </summary>
        public float PercentOfBoard => percentOfBoard;

        public TileInfo(Texture texture, ResourceType resourceType, float percentOfBoard)
        {
            this.texture = texture;
            this.resourceType = resourceType;
            this.percentOfBoard = percentOfBoard;
        }
    }
}