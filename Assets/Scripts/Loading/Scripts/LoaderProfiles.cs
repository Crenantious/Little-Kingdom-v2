using LittleKingdom.Attributes;
using UnityEngine;

namespace LittleKingdom.Loading
{
    public class LoaderProfiles : MonoBehaviour
    {
        // Cannot use field: SerializeField as it is used in a SerializedProperty.
        [SerializeField, DisplayDrawer] private LoaderProfile current;

        public LoaderProfile Current => current;
    }
}