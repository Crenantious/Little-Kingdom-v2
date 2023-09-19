using LittleKingdom;
using LittleKingdom.Buildings;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace PlayModeTests
{
    public class PlayerHUDTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly PlayModeTestHelper testHelper;
        [Inject] private readonly PlayerHUD playerHUD;

        [SetUp]
        public void CommonInstall()
        {
            DefaultInstaller defaultInstaller = new(Container);
            defaultInstaller.InstallBindings();

            Container.Bind<PlayerHUD>()
                .FromComponentInNewPrefab(TestUtilities.LoadPrefab("Player HUD"))
                .AsSingle();
            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator OpenHUD_VerifyContent()
        {
            testHelper.Initialise();

            ShowHUD();

            yield return testHelper;
        }

        private void ShowHUD() =>
            playerHUD.Show(CreatePlayer());

        private IPlayer CreatePlayer()
        {
            Mock<IPlayer> player = new();
            player.Setup(p => p.Resources).Returns(
                new Resources((ResourceType.Stone, 1),
                              (ResourceType.Wood, 2),
                              (ResourceType.Glass, 3),
                              (ResourceType.Brick, 4),
                              (ResourceType.Metal, 5)));
            player.Setup(p => p.OffensiveCards).Returns(CreatePowerCards(1));
            player.Setup(p => p.DefensiveCards).Returns(CreatePowerCards(2));
            player.Setup(p => p.UtilityCards).Returns(CreatePowerCards(3));
            return player.Object;
        }

        private IPowerCard CreatePowerCard() =>
            new Mock<IPowerCard>().Object;

        private List<IPowerCard> CreatePowerCards(int count) =>
            Enumerable.Repeat(CreatePowerCard(), count).ToList();
    }
}