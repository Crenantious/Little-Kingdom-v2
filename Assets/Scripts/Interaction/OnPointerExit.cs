using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [AddComponentMenu("LittleKingdom/Interaction/OnPointerExit")]
    public class OnPointerExit : Interaction
    {
        [Inject]
        public void Construct(PointerObjectHoverTracker tracker)
        {
            tracker.ObjectExited += OnObjectExit;
        }

        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
                OnInteraction();
        }
    }
}