using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    public class LoaderProfiles : MonoBehaviour
    {
        [SerializeField] private List<LoaderProfile> profiles = new();
        [SerializeField] private LoaderProfile current;
        private static LoaderProfiles instance;

        public static LoaderProfile Current => instance.current;

        private void Awake()
        {
            instance = FindObjectOfType<LoaderProfiles>();
        }
    }
}