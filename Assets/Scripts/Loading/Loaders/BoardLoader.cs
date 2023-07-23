using LittleKingdom.Loading;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class BoardLoader : Loader<BoardLC>
    {
        public override List<Loader> Dependencies { get; } = new();

        public override void Load(BoardLC config)
        {
            throw new System.NotImplementedException();
        }

        public override void Unload()
        {
            throw new System.NotImplementedException();
        }
    }
}