using LittleKingdom.Input;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private Loader initialLoader;
        private InGameInput inGameInput;

        [Inject]
        public void Construct(InGameInput inGameInput)
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