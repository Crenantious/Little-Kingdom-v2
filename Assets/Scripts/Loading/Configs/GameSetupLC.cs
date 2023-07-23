using LittleKingdom.Loading;
using UnityEngine;

namespace LittleKingdom
{
    public class GameSetupLC : LoaderConfig
    {
        [field: SerializeField] public int PlayerCount { get; private set; }
    }
}