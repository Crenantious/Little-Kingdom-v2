using LittleKingdom;
using Moq;
using NUnit.Framework;
using Zenject;

namespace TurnOrderTests
{
    public class TurnOrderTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly TurnOrder turnOrder;

        private IPlayer playerOne;
        private IPlayer playerTwo;

        [SetUp]
        public void CommonInstall()
        {
            playerOne = new Mock<IPlayer>().Object;
            playerTwo = new Mock<IPlayer>().Object;

            Container.Bind<TurnOrder>().AsTransient();
            Container.Inject(this);
        }

        [Test]
        public void AddNoPlayers_CurrentIsNull()
        {
            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddNoPlayers_MoveNextIsFalse()
        {
            bool result = turnOrder.MoveNext();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddNoPlayers_MoveNext_CurrentIsNull()
        {
            turnOrder.MoveNext();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddNoPlayers_CountIsZero()
        {
            Assert.AreEqual(0, turnOrder.Count);
        }

        [Test]
        public void AddAPlayer_CountIsOne()
        {
            turnOrder.AddPlayer(playerOne);

            Assert.AreEqual(1, turnOrder.Count);
        }

        [Test]
        public void AddTwoPlayers_CountIsTwo()
        {
            turnOrder.AddPlayer(playerOne);
            turnOrder.AddPlayer(playerTwo);

            Assert.AreEqual(2, turnOrder.Count);
        }

        [Test]
        public void AddAPlayer_CurrentIsNull()
        {
            turnOrder.AddPlayer(playerOne);

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddAPlayer_MoveNext_CurrentIsThePlayer()
        {
            turnOrder.AddPlayer(playerOne);

            turnOrder.MoveNext();

            Assert.AreEqual(playerOne, turnOrder.Current);
        }

        [Test]
        public void AddAPlayer_MoveNextTwice_CurrentIsNull()
        {
            turnOrder.AddPlayer(playerOne);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddAPlayer_MoveNext_Reset_CurrentIsNull()
        {
            turnOrder.AddPlayer(playerOne);

            turnOrder.MoveNext();
            turnOrder.Reset();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddAPlayer_MoveNextTwice_Reset_CurrentIsNull()
        {
            turnOrder.AddPlayer(playerOne);

            turnOrder.MoveNext();
            turnOrder.MoveNext();
            turnOrder.Reset();

            Assert.AreEqual(null, turnOrder.Current);
        }

        [Test]
        public void AddAPlayer_MoveNext_Reset_MoveNext_CurrentIsThePlayer()
        {
            turnOrder.AddPlayer(playerOne);

            turnOrder.MoveNext();
            turnOrder.Reset();
            turnOrder.MoveNext();

            Assert.AreEqual(playerOne, turnOrder.Current);
        }

        [Test]
        public void AddTwoPlayers_MoveNext_CurrentIsPlayerOne()
        {
            turnOrder.AddPlayer(playerOne);
            turnOrder.AddPlayer(playerTwo);

            turnOrder.MoveNext();

            Assert.AreEqual(playerOne, turnOrder.Current);
        }

        [Test]
        public void AddTwoPlayers_MoveNextTwice_CurrentIsPlayerTwo()
        {
            turnOrder.AddPlayer(playerOne);
            turnOrder.AddPlayer(playerTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();

            Assert.AreEqual(playerTwo, turnOrder.Current);
        }

        [Test]
        public void AddTwoPlayers_MoveNextTwice_Reset_MoveNext_CurrentIsPlayerOne()
        {
            turnOrder.AddPlayer(playerOne);
            turnOrder.AddPlayer(playerTwo);

            turnOrder.MoveNext();
            turnOrder.MoveNext();
            turnOrder.Reset();
            turnOrder.MoveNext();

            Assert.AreEqual(playerOne, turnOrder.Current);
        }
    }
}