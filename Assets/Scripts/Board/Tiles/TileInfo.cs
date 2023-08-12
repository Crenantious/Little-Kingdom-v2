using System;
using UnityEngine;

namespace LittleKingdom.Board
{
    [Serializable]
    public class TileInfo : ITileInfo
    {
        [SerializeField] private Texture texture;
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private float percentOfBoard;

        public Texture Texture => texture;
        public ResourceType ResourceType => resourceType;
        public float PercentOfBoard => percentOfBoard;

        public TileInfo(Texture texture, ResourceType resourceType, float percentOfBoard)
        {
            this.texture = texture;
            this.resourceType = resourceType;
            this.percentOfBoard = percentOfBoard;
        }
    }
}