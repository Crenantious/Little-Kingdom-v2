using LittleKingdom.Input;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private Loader initialLoader;
        private StandardInput inGameInput;

        [Inject]
        public void Construct(StandardInput inGameInput)
        {
            this.inGameInput = inGameInput;
        }

        private void Awake()
        {
            ActiveInputScheme.Set(inGameInput);
        }

        private void Start()
        {
            Loading.Loading.Load(initialLoader);
        }
    }
}