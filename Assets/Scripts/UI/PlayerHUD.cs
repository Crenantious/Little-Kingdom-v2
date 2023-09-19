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
    public class PlayerHUD : MonoBehaviour, IUIContainter<IPlayer>
    {
        [SerializeField] private UIContainer container;
        [SerializeField] private VisualTreeAsset hud;
        [SerializeField] private VisualTreeAsset resource;

        public void Show(IPlayer player)
        {
            container.Show(hud, () => OnPanelHide(player));
            SetPowerCards(player);
            SetResources(player);
        }

        private void SetPowerCards(IPlayer player)
        {
            VisualElement powerCards = GetRootVisualElement().Q("PowerCards");
            powerCards.Q("Offensive").Q<Label>("Amount").text = player.OffensiveCards.Count.ToString();
            powerCards.Q("Defensive").Q<Label>("Amount").text = player.DefensiveCards.Count.ToString();
            powerCards.Q("Utility").Q<Label>("Amount").text = player.UtilityCards.Count.ToString();
        }

        private void SetResources(IPlayer player)
        {
            VisualElement resources = GetRootVisualElement().Q("Resources");
            foreach (ResourceType resourceType in GetResourceTypes())
            {
                var resourceClone = resource.Instantiate();
                resourceClone.Q<Label>("Name").text = resourceType.ToString();
                resourceClone.Q<Label>("Amount").text = player.Resources.Get(resourceType).ToString();
                resources.Add(resourceClone);
            }
        }

        private IEnumerable<ResourceType> GetResourceTypes() =>
            new Resources.Resources()
            .GetAll()
            .Select(x => x.Key);

        public void Hide() => container.Hide();

        private void OnPanelHide(IPlayer data)
        {
            // Unregister end turn buttion.
        }

        private VisualElement GetRootVisualElement() =>
            container.Document.rootVisualElement;
    }
}