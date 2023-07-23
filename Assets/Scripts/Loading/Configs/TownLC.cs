using LittleKingdom.Loading;
using UnityEngine;

namespace LittleKingdom
{
    public class TownLC : LoaderConfig
    {
        [field: SerializeField] public bool AutoPlace { get; private set; }
    }
}