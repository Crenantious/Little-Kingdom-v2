using LittleKingdom.Attributes;
using UnityEngine;

namespace LittleKingdom.Loading
{
    public class LoaderProfiles : MonoBehaviour
    {
        [field: SerializeField, DisplayDrawer] public LoaderProfile Current;
    }
}