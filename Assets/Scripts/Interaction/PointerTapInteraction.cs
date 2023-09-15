using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [AddComponentMenu("LittleKingdom/Interaction/PointerTapInteraction")]
    public class PointerPressAndReleaseInteraction : Interaction
    {
        [Inject]
        public void Construct(StandardInput input)
        {
            input.PointerPressAndRelease += OnInteraction;
        }
    }
}