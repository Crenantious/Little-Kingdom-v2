using LittleKingdom.Loading;
using UnityEngine;
using System;
using Zenject;

namespace LittleKingdom
{
    public class TownLoader : Loader<TownLC>
    {
        [SerializeField] private BoardLoader boardLoader;
        private TownPlacement townPlacement;

        [Inject]
        public void Construct(TownPlacement townPlacement) =>
            this.townPlacement = townPlacement;

        private void Awake() =>
            Dependencies.Add(boardLoader);

        public override void Load(TownLC config)
        {
            //TODO: JR - place one town after another.
            foreach (Player player in TurnManager.Players)
            {
                if (config.AutoPlace)
                    townPlacement.PlaceAutomatically(player.Town);
                else
                    townPlacement.PlaceManually(player.Town);
            }
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}