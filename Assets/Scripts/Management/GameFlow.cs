using LittleKingdom.Input;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private Loader initialLoader;

        [Inject]
        public void Construct(StandardInput inGameInput)
        {
            ActiveInputScheme.Set(inGameInput);
        }

        private void Start()
        {
            Loading.Loading.Load(initialLoader);
        }
    }
}