using LittleKingdom;
using LittleKingdom.Buildings;
using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject;

namespace IntegrationTests
{
    public class CharacterTurnTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly PlayModeTestHelper testHelper;
        [Inject] private readonly CharacterTurnTransitions turnTransitions;
        [Inject] private readonly PlayerHUD playerHUD;

        private readonly Resources uniqueResourceAmounts = new((ResourceType.Stone, 1),
                                                               (ResourceType.Wood, 2),
                                                               (ResourceType.Brick, 3),
                                                               (ResourceType.Glass, 4),
                                                               (ResourceType.Metal, 5));

        private Mock<ICharacter> characterOne;
        private Mock<ICharacter> characterTwo;

        [SetUp]
        public void CommonInstall()
        {
            DefaultInstaller defaultInstaller = new(Container);
            defaultInstaller.InstallBindings();

            Container.Bind<PlayerHUD>()
                .FromComponentInNewPrefab(TestUtilities.LoadPrefab("Player HUD"))
                .AsSingle();
            Container.Bind<Player>()
                .FromComponentInNewPrefab(TestUtilities.LoadPrefab("Player"))
                .AsSingle();
            Container.Bind<CharacterTurnOrder>().AsSingle();
            Container.Bind<CharacterTurnTransitions>().AsSingle();
            Container.BindFactory<ICharacter, ICharacterTurn, CharacterTurnFactory>().FromFactory<CustomCharacterTurnFactory>();
            Container.Inject(this);

            characterOne = new();
            characterTwo = new();

            characterOne.Setup(c=>c.Resources).Returns(uniqueResourceAmounts);
            characterOne.Setup(c=>c.Resources).Returns(Resources.Add(uniqueResourceAmounts, uniqueResourceAmounts));

            CharacterTurnOrder.AddCharacter(characterOne.Object);
            CharacterTurnOrder.AddCharacter(characterTwo.Object);
        }

        [UnityTest]
        public IEnumerator BeginFirstTurn_VerifyHUDContentsForPlayerOne()
        {
            testHelper.Initialise();

            turnTransitions.BeginFirstTurn();

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator BeginFirstTurn_EndTurn_VerifyHUDContentsForPlayerTwo()
        {
            testHelper.Initialise();

            turnTransitions.BeginFirstTurn();

            yield return testHelper;
        }

        private Mock<ICharacter> CreateCharacter(Resources resources, int offensiveCardsCount,
            int defensiveCardsCount, int utilityCardsCount)
        {
            Mock<ICharacter> character = new();
            character.Setup(p => p.Resources).Returns(resources);
            character.Setup(p => p.OffensiveCards).Returns(CreatePowerCards(offensiveCardsCount));
            character.Setup(p => p.DefensiveCards).Returns(CreatePowerCards(defensiveCardsCount));
            character.Setup(p => p.UtilityCards).Returns(CreatePowerCards(utilityCardsCount));
            return character;
        }

        private IPowerCard CreatePowerCard() =>
            new Mock<IPowerCard>().Object;

        private List<IPowerCard> CreatePowerCards(int count) =>
            Enumerable.Repeat(CreatePowerCard(), count).ToList();
    }
}