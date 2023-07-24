using UnityEngine;

namespace LittleKingdom.Loading
{
    [CreateAssetMenu(menuName = "Game/Loading/Configs/Town")]
    public class TownLC : LoaderConfig
    {
        [field: SerializeField] public bool AutoPlace { get; private set; }
    }
}