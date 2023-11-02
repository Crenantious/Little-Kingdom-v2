using LittleKingdom;
using LittleKingdom.Buildings;
using LittleKingdom.CharacterTurns;
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
        // TODO: JR - put this in a settings SO. Probably link the settings to a profile.
        private const int maxValue = 999;

        [Inject] private readonly PlayModeTestHelper testHelper;
        [Inject] private readonly PlayerHUD playerHUD;

        private readonly Resources uniqueResourceAmounts = new((ResourceType.Stone, 1),
                                                               (ResourceType.Wood, 2),
                                                               (ResourceType.Brick, 3),
                                                               (ResourceType.Glass, 4),
                                                               (ResourceType.Metal, 5));

        private readonly Resources maxResourceAmounts = new((ResourceType.Stone |
                                                             ResourceType.Wood |
                                                             ResourceType.Brick |
                                                             ResourceType.Glass |
                                                             ResourceType.Metal,
                                                             maxValue));
        private Mock<ICharacterTurnTransitions> turnTransitions;

        [SetUp]
        public void CommonInstall()
        {
            turnTransitions = new();

            Container.BindInstance(turnTransitions.Object);
            PlayModeInstaller.Install(Container);
            Container.Inject(this);
        }

        [UnityTest]
        // This is to ensure each value is being set correctly.
        public IEnumerator OpenHUD_VerifyContentWithUniqueValues()
        {
            Mock<ICharacter> character = CreateCharacter(uniqueResourceAmounts, 6, 7, 8);
            testHelper.OpenDialogBox();

            playerHUD.Show(character.Object);

            yield return testHelper;
        }

        [UnityTest]
        // This is to check for resizing and overlapping.
        public IEnumerator OpenHUD_VerifyContentWithMaxValues()
        {
            Mock<ICharacter> character = CreateCharacter(maxResourceAmounts, maxValue, maxValue, maxValue);
            testHelper.OpenDialogBox();

            playerHUD.Show(character.Object);

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator OpenHUD_ChangeCharacterData_UpdateHUD_VerifyContentUpdatedCorrectly()
        {
            Mock<ICharacter> character = CreateCharacter(uniqueResourceAmounts, 6, 7, 8);
            testHelper.OpenDialogBox(("Modify character", () => ModifyCharacterAndUpdateHUD(character.Object)));

            playerHUD.Show(character.Object);

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator OpenHUD_CloseHUD_OpenHUDWithADifferentCharacter_VerifyCorrectValuesAreDisplayed()
        {
            Mock<ICharacter> characterOne = CreateCharacter(uniqueResourceAmounts, 6, 7, 8);
            Mock<ICharacter> characterTwo = CreateCharacter(maxResourceAmounts, 1, 2, 3);
            testHelper.OpenDialogBox();

            playerHUD.Show(characterOne.Object);
            playerHUD.Hide();
            playerHUD.Show(characterTwo.Object);

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator OpenHUD_OpenHUDWithADifferentCharacter_VerifyCorrectValuesAreDisplayed()
        {
            Mock<ICharacter> characterOne = CreateCharacter(uniqueResourceAmounts, 6, 7, 8);
            Mock<ICharacter> characterTwo = CreateCharacter(maxResourceAmounts, 1, 2, 3);
            testHelper.OpenDialogBox();

            playerHUD.Show(characterOne.Object);
            playerHUD.Show(characterTwo.Object);

            yield return testHelper;
        }


        [UnityTest]
        public IEnumerator OpenHUD_ClickEndTurnButton_VerifyCharacterEndTurnWasCalled()
        {
            Mock<ICharacter> character = CreateCharacter(uniqueResourceAmounts, 6, 7, 8);
            Mock<ICharacterTurn> turn = new();
            character.Setup(c => c.Turn).Returns(turn.Object);
            testHelper.OpenDialogBox(() => turn.Verify(t => t.End(), Times.Once()), true);

            playerHUD.Show(character.Object);

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

        private void ModifyCharacterAndUpdateHUD(ICharacter character)
        {
            character.Resources.Add(character.Resources);
            character.OffensiveCards.Add(CreatePowerCard());
            character.DefensiveCards.Add(CreatePowerCard());
            character.UtilityCards.Add(CreatePowerCard());
            playerHUD.UpdateValues();
        }
    }
}