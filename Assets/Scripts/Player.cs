using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public Town Town { get; private set; }
    }
}