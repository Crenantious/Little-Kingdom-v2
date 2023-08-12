using UnityEngine;

namespace LittleKingdom
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Town town;
        public ITown Town => town;
    }
}