// Ignore Spelling: Interactable

using System.Collections.Generic;
using UnityEngine;
using static LittleKingdom.Interaction.Interaction;

namespace LittleKingdom.Interaction
{
    public class Interactable : MonoBehaviour
    {
        private readonly List<Interaction> mouseDownInteractions = new();
        private readonly List<Interaction> mouseUpInteractions = new();
        private readonly List<Interaction> mouseEnterInteractions = new();
        private readonly List<Interaction> mouseExitInteractions = new();
        private readonly List<Interaction> mouseOverInteractions = new();
        private readonly List<Interaction> mouseDragInteractions = new();
        private Dictionary<InteractionType, List<Interaction>> typeToInteractions;

        public List<Interaction> Interactions;

        private void Awake()
        {
            typeToInteractions = new()
            {
                { InteractionType.MouseDown, mouseDownInteractions },
                { InteractionType.MouseUp, mouseUpInteractions },
                { InteractionType.MouseEnter, mouseEnterInteractions },
                { InteractionType.MouseExit, mouseExitInteractions },
                { InteractionType.MouseOver, mouseOverInteractions },
                { InteractionType.MouseDrag, mouseDragInteractions }
            };
            print(Interactions.Count);

            foreach (Interaction interaction in Interactions)
            {
                typeToInteractions[interaction.Type].Add(interaction);
                print(interaction.Type);
            }
        }

        private void OnMouseDown() =>
            HandleInteractions(mouseDownInteractions);

        private void OnMouseUp() =>
            HandleInteractions(mouseUpInteractions);

        private void OnMouseEnter() =>
            HandleInteractions(mouseEnterInteractions);

        private void OnMouseExit() =>
            HandleInteractions(mouseExitInteractions);

        private void OnMouseOver() =>
            HandleInteractions(mouseOverInteractions);

        private void OnMouseDrag() =>
            HandleInteractions(mouseDragInteractions);

        private void HandleInteractions(List<Interaction> interactions)
        {
            foreach (Interaction interaction in interactions)
            {
                print(interaction.Type);
                interaction.OnInteraction();
            }
        }
    }
}