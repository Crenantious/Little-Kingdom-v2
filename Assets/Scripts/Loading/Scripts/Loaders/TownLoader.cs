using LittleKingdom.Loading;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LittleKingdom
{
    public class TownLoader : Loader<TownLC>
    {
        [SerializeField] private BoardLoader boardLoader;

        private void Awake() =>
            Dependencies.Add(boardLoader);

        public override void Load(TownLC config)
        {
            if (config.AutoPlace)
                throw new NotImplementedException();

            foreach(Player player in TurnManager.Players)
                TownPlacement.BeginPlacement(player.Town);
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}