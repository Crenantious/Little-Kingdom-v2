using UnityEngine;
using LittleKingdom.Loading;

namespace LittleKingdom
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        [SerializeField] private Player player;

        public override void Load(GameSetupLC config)
        {
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