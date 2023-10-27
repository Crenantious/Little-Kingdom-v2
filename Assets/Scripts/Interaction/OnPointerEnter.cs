using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [AddComponentMenu("LittleKingdom/Interaction/OnPointerEnter")]
    public class OnPointerEnter : Interaction
    {
        [Inject]
        public void Construct(PointerOverObjectTracker tracker)
        {
            tracker.ObjectEntered += OnObjectEnter;
        }

        private void OnObjectEnter(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
                OnInteraction();
        }
    }
}