using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [RequireComponent(typeof(Selectable))]
    [AddComponentMenu("LittleKingdom/Interaction/OnSelected")]
    public class OnSelected : Interaction
    {
        [SerializeField] private Selectable selectable;

        [Inject]
        public void Construct(SelectedObjectTracker tracker)
        {
            tracker.ObjectSelected += OnObjectSelected;
        }

        private void OnObjectSelected(Selectable selectable)
        {
            print("Selected");
            if (selectable == this.selectable)
                OnInteraction();
        }
    }
}