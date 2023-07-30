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
            foreach (Player player in TurnManager.Players)
            {
                if (config.AutoPlace)
                    TownPlacement.PlaceAutomatically(player.Town);
                else
                    TownPlacement.PlaceManually(player.Town);
            }
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}