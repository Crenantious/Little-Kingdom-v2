using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [RequireComponent(typeof(Selectable))]
    [AddComponentMenu("LittleKingdom/Interaction/OnDeselected")]
    public class OnDeselected : Interaction
    {
        [SerializeField] private Selectable selectable;

        [Inject]
        public void Construct(SelectedObjectTracker tracker)
        {
            tracker.ObjectDeselected += OnObjectDeselected;
        }

        private void OnObjectDeselected(Selectable selectable)
        {
            if (selectable == this.selectable)
                OnInteraction();
        }
    }
}