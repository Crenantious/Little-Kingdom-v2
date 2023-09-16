using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [CreateAssetMenu(menuName = "Game/Loading/Configs/Town")]
    public class TownLC : LoaderConfig
    {
        [field: SerializeField] public bool AutoPlace { get; private set; }

        [field: SerializeField]
        [Tooltip("Towns are automatically placed on these tiles up to the number of players." +
                 "Ensure there are at least as many listed here as there are players.")]
        public List<Vector2Int> TownOriginTiles { get; private set; }
    }
}