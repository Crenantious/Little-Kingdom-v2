using NUnit.Framework;
using LittleKingdom.DataStructures;
using LittleKingdom.Extensions;
using Moq;
using Zenject;

namespace LittleKingdom.Tests
{
    [TestFixture]
    internal class TownPlacementTests : ZenjectUnitTestFixture
    {
        private Mock<ITown> town;
        [Inject] private TownPlacement townPlacement;

        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<TownPlacement>().AsSingle();
            Container.Inject(this);
            town = new();
        }

        [Test]
        public void SetAllWorks_Correctly()
        {

        }
    }
}