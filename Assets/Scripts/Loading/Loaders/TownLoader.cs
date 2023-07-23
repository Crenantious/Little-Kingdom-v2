using LittleKingdom.Loading;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class TownLoader : Loader<TownLC>
    {
        public override List<Loader> Dependencies { get; } = new() { };

        public override void Load(TownLC config)
        {
            throw new System.NotImplementedException();
        }

        public override void Unload()
        {
            throw new System.NotImplementedException();
        }
    }
}