using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [CreateAssetMenu(menuName = "Game/Loading/Configs/Game setup")]
    public class GameSetupLC : LoaderConfig
    {
        [field: SerializeField] public List<string> PlayerNames { get; private set; }
    }
}