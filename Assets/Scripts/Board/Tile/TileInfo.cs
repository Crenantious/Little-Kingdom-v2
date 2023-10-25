using LittleKingdom.Resources;
using System;
using UnityEngine;

namespace LittleKingdom.Board
{
    [Serializable]
    public class TileInfo : ITileInfo
    {
        [SerializeField] private Texture texture;
        [SerializeField] private Resources.Resources resources;
        [SerializeField] private float percentOfBoard;

        public Texture Texture => texture;
        public Resources.Resources Resources => resources;
        public float PercentOfBoard => percentOfBoard;

        public TileInfo(Texture texture, ResourceType resourceType, float percentOfBoard)
        {
            this.texture = texture;
            this.percentOfBoard = percentOfBoard;

            // TODO: JR - make the resource amount configurable.
            resources = new(resourceType, 1);
        }
    }
}