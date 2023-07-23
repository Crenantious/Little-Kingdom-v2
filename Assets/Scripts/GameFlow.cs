using LittleKingdom.Loading;
using UnityEngine;

namespace LittleKingdom
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private Loader initialLoader;

        private void Start()
        {
            Loading.Loading.Load(initialLoader);
        }
    }
}