using LittleKingdom.CharacterTurns;
using LittleKingdom.Resources;
using LittleKingdom.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace LittleKingdom.Buildings
{
    [AddComponentMenu("LittleKingdom/Player HUD")]
    [RequireComponent(typeof(UIContainer))]
    public class PlayerHUD : MonoBehaviour, IUIContainter<ICharacter>
    {
        private readonly IEnumerable<ResourceType> resourceTypes = GetResourceTypes();

        [SerializeField] private UIContainer container;
        [SerializeField] private VisualTreeAsset hud;
        [SerializeField] private VisualTreeAsset resource;

        private ICharacterTurnTransitions transitions;
        private ICharacter character;

        [Inject]
        public void Construct(ICharacterTurnTransitions transitions) =>
            this.transitions = transitions;

        // TODO: JR - figure out why this doesn't create extra resource labels when called multiple times.
        // (Works as intended so it's unimportant for now.)
        /// <summary>
        /// Show the HUD and call <see cref="UpdateValues"/>.
        /// </summary>
        public void Show(ICharacter character)
        {
            this.character = character;

            container.Show(hud, OnPanelHide);
            CreateResourcesLabels();
            GetRootVisualElement().Q<Label>("PlayerName").text = character.Name;
            GetRootVisualElement().Q<Button>("EndTurnButton").clicked += transitions.EndCurrentTurn;

            UpdateValues();
        }

        /// <summary>
        /// Updates the HUD values based on the <see cref="ICharacter"/> passed into <see cref="Show(ICharacter)"/>.<br/>
        /// Logs an error if <see cref="Show(ICharacter)"/> was not called beforehand.
        /// </summary>
        public void UpdateValues()
        {
            if (character is null)
            {
                Debug.LogError($"Cannot update HUD values as no character is set. Must call {nameof(Show)} first.", this);
                return;
            }

            UpdateResourcesValues();
            UpdatePowerCardValues();
        }

        private void CreateResourcesLabels()
        {
            VisualElement resources = GetRootVisualElement().Q("Resources");
            foreach (ResourceType resourceType in resourceTypes)
            {
                VisualElement resourceClone = resource.Instantiate();
                resourceClone.name = GetResourceContainerName(resourceType);
                resourceClone.Q<Label>("Name").text = resourceType.ToString() + ":";
                resourceClone.Q<Label>("Amount").text = "0";
                resources.Add(resourceClone);
            }
        }

        private void UpdateResourcesValues()
        {
            foreach (ResourceType resourceType in resourceTypes)
            {
                VisualElement container = GetRootVisualElement().Q(GetResourceContainerName(resourceType));
                container.Q<Label>("Amount").text = character.Resources.Get(resourceType).ToString();
            }
        }

        private void UpdatePowerCardValues()
        {
            VisualElement powerCards = GetRootVisualElement().Q("PowerCards");
            powerCards.Q("Offensive").Q<Label>("Amount").text = character.OffensiveCards.Count.ToString();
            powerCards.Q("Defensive").Q<Label>("Amount").text = character.DefensiveCards.Count.ToString();
            powerCards.Q("Utility").Q<Label>("Amount").text = character.UtilityCards.Count.ToString();
        }

        public void Hide() => container.Hide();

        private void OnPanelHide()
        {
            // Unregister end turn buttion.
        }

        private static IEnumerable<ResourceType> GetResourceTypes()
        {
            Array values = Enum.GetValues(typeof(ResourceType));
            return ((IEnumerable<ResourceType>)values).Where(r => r != ResourceType.None);
        }
        private static string GetResourceContainerName(ResourceType resourceType) =>
            "Resource" + resourceType.ToString();

        private VisualElement GetRootVisualElement() =>
            container.Document.rootVisualElement;
    }
}