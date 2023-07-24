using UnityEngine;

namespace LittleKingdom.Loading
{
    [CreateAssetMenu(menuName = "Game/Loading/Configs/Game setup")]
    public class GameSetupLC : LoaderConfig
    {
        [field: SerializeField] public int PlayerCount { get; private set; }
    }
}