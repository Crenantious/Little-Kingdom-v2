using UnityEngine;
using LittleKingdom.Loading;
using System.Collections.Generic;

namespace LittleKingdom
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        private readonly IReferences references;

        [SerializeField] private Player player;

        public GameSetupLoader(IReferences references) =>
            this.references = references;

        public override void Load(GameSetupLC config)
        {
            references.GameState = GameState.StandardInGame;

            for (int i = 0; i < config.PlayerCount; i++)
            {
                TurnManager.AddPlayer(Instantiate(player));
            }
        }

        public override void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}