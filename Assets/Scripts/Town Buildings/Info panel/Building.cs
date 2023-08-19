using System;
using UnityEngine;

namespace LittleKingdom.Buildings
{
    [AddComponentMenu("LittleKingdom/Building")]
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public int BuildingLevel { get; set; }
        [field: SerializeField] public string Description { get; set; }
        [field: SerializeField] public Action UpgradeCallback { get; set; }
    }
}