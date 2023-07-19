using LittleKingdom.Attributes;
using UnityEngine;

namespace LittleKingdom.Loading
{
    public class LoaderProfiles : MonoBehaviour
    {
        private static LoaderProfiles instance;

        [SerializeField, DisplayDrawer] private LoaderProfile current;

        public static LoaderProfile Current => instance.current;

        private void Awake()
        {
            instance = FindObjectOfType<LoaderProfiles>();
        }
    }
}