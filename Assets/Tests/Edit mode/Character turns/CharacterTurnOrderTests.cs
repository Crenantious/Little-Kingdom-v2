using LittleKingdom;
using LittleKingdom.CharacterTurns;
using Moq;
using NUnit.Framework;
using Zenject;

namespace CharacterTurnTests
{
    public class CharacterTurnOrderTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly CharacterTurnOrder turnOrder;

        private ICharacter characterOne;
        private ICharacter characterTwo;

        [SetUp]
        public void CommonInstall()
        {
            characterOne = new Mock<ICharacter>().Object;
            characterTwo = new Mock<ICharacter>().Object;

            Container.Bind<CharacterTurnOrder>().AsTransient();
            Container.Inject(this);
        }

        [TearDown]
        public override void Teardown() =>
            CharacterTurnOrder.RemoveAllCharacters();

        [Test]
        public void AddNoCharacters_CurrentIsNull()
        {
            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddNoCharacters_MoveNextIsFalse()
        {
            bool result = turnOrder.MoveNext();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddNoCharacters_MoveNext_CurrentIsNull()
        {
            turnOrder.MoveNext();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddNoCharacters_CountIsZero()
        {
            Assert.AreEqual(0, turnOrder.Count);
        }

        [Test]
        public void AddACharacter_CountIsOne()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            Assert.AreEqual(1, turnOrder.Count);
        }

        [Test]
        public void AddTwoCharacters_CountIsTwo()
        {
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            Assert.AreEqual(2, turnOrder.Count);
        }

        [Test]
        public void AddACharacter_CurrentIsNull()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddACharacter_MoveNext_CurrentIsTheCharacter()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }

        [Test]
        public void AddACharacter_MoveNextTwice_CurrentIsNull()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddACharacter_MoveNext_Reset_CurrentIsNull()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();
            turnOrder.Reset();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddACharacter_MoveNextTwice_Reset_CurrentIsNull()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();
            turnOrder.MoveNext();
            turnOrder.Reset();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddACharacter_MoveNext_Reset_MoveNext_CurrentIsTheCharacter()
        {
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();
            turnOrder.Reset();
            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }

        [Test]
        public void AddTwoCharacters_MoveNext_CurrentIsCharacterOne()
        {
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }

        [Test]
        public void AddTwoCharacters_MoveNextTwice_CurrentIsCharacterTwo()
        {
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(characterTwo, turnOrder.Current);
        }

        [Test]
        public void AddTwoCharacters_MoveNextTwice_Reset_MoveNext_CurrentIsCharacterOne()
        {
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();
            turnOrder.Reset();
            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }

        [Test]
        public void AddACharacterAndEnableWrapping_MoveNextTwice_CurrentIsTheCharacter()
        {
            turnOrder.ShouldWrap = true;
            CharacterTurnOrder.AddCharacter(characterOne);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }

        [Test]
        public void AddTwoCharactersAndEnableWrapping_MoveNextTwice_CurrentIsCharacterTwo()
        {
            turnOrder.ShouldWrap = true;
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(characterTwo, turnOrder.Current);
        }

        [Test]
        public void AddTwoCharactersAndEnableWrapping_MoveNextThrice_CurrentIsCharacterOne()
        {
            turnOrder.ShouldWrap = true;
            CharacterTurnOrder.AddCharacter(characterOne);
            CharacterTurnOrder.AddCharacter(characterTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(characterOne, turnOrder.Current);
        }
    }
}