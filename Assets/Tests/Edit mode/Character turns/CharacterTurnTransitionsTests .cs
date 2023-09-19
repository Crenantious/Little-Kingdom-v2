using Assets.Scripts.Exceptions;
using LittleKingdom;
using LittleKingdom.CharacterTurns;
using Moq;
using NUnit.Framework;
using Zenject;

namespace CharacterTurnTests
{
    public class CharacterTurnTransitionsTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly CharacterTurnTransitions transitions;

        private Mock<ICharacter> characterOne;
        private Mock<ICharacter> characterTwo;
        private Mock<ICharacterTurn> characterOneTurn;
        private Mock<ICharacterTurn> characterTwoTurn;

        [SetUp]
        public void CommonInstall()
        {
            characterOne = new();
            characterTwo = new();
            characterOneTurn = new();
            characterTwoTurn = new();

            characterOne.Setup(c => c.Turn).Returns(characterOneTurn.Object);
            characterTwo.Setup(c => c.Turn).Returns(characterTwoTurn.Object);

            Container.Bind<CharacterTurnOrder>().AsTransient();
            Container.Bind<CharacterTurnTransitions>().AsSingle();
            Container.Inject(this);
        }

        [TearDown]
        public override void Teardown() =>
            CharacterTurnOrder.RemoveAllCharacters();

        [Test]
        public void AddNoPlayers_BeginFirstTurn_GetException()
        {
            Assert.Throws<NoCharactersRegisteredException>(() => transitions.BeginFirstTurn());
        }

        [Test]
        public void AddNoPlayers_EndCurrentTurn_GetException()
        {
            Assert.Throws<NoCharacterTurnStartedException>(() => transitions.EndCurrentTurn());
        }

        [Test]
        public void AddPlayerOne_EndCurrentTurn_GetException()
        {
            AddCharacter(characterOne);

            Assert.Throws<NoCharacterTurnStartedException>(() => transitions.EndCurrentTurn());
        }

        [Test]
        public void AddPlayerOne_BeginFirstTurn_PlayerOneTurnBeganAndDidNotEnd()
        {
            AddCharacter(characterOne);

            transitions.BeginFirstTurn();

            characterOneTurn.Verify(c => c.Begin(), Times.Once());
            characterOneTurn.Verify(c => c.End(), Times.Never());
        }

        [Test]
        public void AddPlayerOne_BeginFirstTurnThenEndTurn_PlayerOneTurnBeganTwiceAndEndedOnce()
        {
            AddCharacter(characterOne);

            transitions.BeginFirstTurn();
            transitions.EndCurrentTurn();

            characterOneTurn.Verify(c => c.Begin(), Times.Exactly(2));
            characterOneTurn.Verify(c => c.End(), Times.Once());
        }

        [Test]
        public void AddPlayerOneAndPlayerTwo_BeginFirstTurn_OnlyPlayerOneTurnBeganAndNeitherTurnEnded()
        {
            AddCharacter(characterOne);
            AddCharacter(characterTwo);

            transitions.BeginFirstTurn();

            characterOneTurn.Verify(c => c.Begin(), Times.Once());
            characterOneTurn.Verify(c => c.End(), Times.Never());
            characterTwoTurn.Verify(c => c.Begin(), Times.Never());
            characterTwoTurn.Verify(c => c.End(), Times.Never());
        }

        [Test]
        public void AddPlayerOneAndPlayerTwo_BeginFirstTurnThenEndTurn_PlayerOneAndPlayerTwoTurnsBeganAndPlayerOneTurnEnded()
        {
            AddCharacter(characterOne);
            AddCharacter(characterTwo);

            transitions.BeginFirstTurn();
            transitions.EndCurrentTurn();

            characterOneTurn.Verify(c => c.Begin(), Times.Once());
            characterOneTurn.Verify(c => c.End(), Times.Once());
            characterTwoTurn.Verify(c => c.Begin(), Times.Once());
            characterTwoTurn.Verify(c => c.End(), Times.Never());
        }

        [Test]
        public void AddPlayerOneAndPlayerTwo_BeginFirstTurnThenEndTurnTwice_PlayerOneTurnBeganTwiceAndPlayerTwoTurnBeganOnceAndBothPlayerTurnsEndedOnce()
        {
            AddCharacter(characterOne);
            AddCharacter(characterTwo);

            transitions.BeginFirstTurn();
            transitions.EndCurrentTurn();
            transitions.EndCurrentTurn();

            characterOneTurn.Verify(c => c.Begin(), Times.Exactly(2));
            characterOneTurn.Verify(c => c.End(), Times.Once());
            characterTwoTurn.Verify(c => c.Begin(), Times.Once());
            characterTwoTurn.Verify(c => c.End(), Times.Once());
        }

        private void AddCharacter(Mock<ICharacter> character) =>
            CharacterTurnOrder.AddCharacter(character.Object);
    }
}