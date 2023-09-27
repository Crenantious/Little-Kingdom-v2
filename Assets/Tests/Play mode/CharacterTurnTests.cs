using LittleKingdom;
using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TestTools;
using Zenject;

namespace IntegrationTests
{
    public class CharacterTurnTests : ZenjectIntegrationTestFixture
    {
        [Inject] private readonly PlayModeTestHelper testHelper;
        [Inject] private readonly ICharacterTurnTransitions turnTransitions;
        [Inject] private readonly PlayerFactory playerFactory;

        private readonly Resources uniqueResourceAmounts = new((ResourceType.Stone, 1),
                                                                                       (ResourceType.Wood, 2),
                                                                                       (ResourceType.Brick, 3),
                                                                                       (ResourceType.Glass, 4),
                                                                                       (ResourceType.Metal, 5));

        private ICharacter characterOne;
        private ICharacter characterTwo;

        [SetUp]
        public void CommonInstall()
        {
            PreInstall();
            CharacterTurnInstaller.Install(Container);
            PlayModeInstaller.Install(Container);
            PlayerInstaller.Player = new UnityEngine.GameObject().AddComponent<Player>();
            PlayerInstaller.Install(Container);
            PostInstall();

            Setup();
        }

        private void Setup()
        {
            characterOne = playerFactory.Create("Player one");
            characterTwo = playerFactory.Create("Player two");

            characterOne.Resources.Add(uniqueResourceAmounts);
            characterTwo.Resources.Add(Resources.Add(uniqueResourceAmounts, uniqueResourceAmounts));

            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);
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