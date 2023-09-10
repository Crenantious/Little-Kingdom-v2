using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using Zenject;
using static ResourceRequestsTests.ResourceRequestsTestsUtilities;

namespace ResourceRequests
{
    public class ResourceHandlingUtilitiesAccountForAvailableResourcesTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly ResourceHandlingUtilities utilities;

        private Mock<IHoldResources> holder;
        private MoveResourcesRequest moveRequest;

        [SetUp]
        public void CommonInstall()
        {
            holder = new();

            Container.Bind<ResourceHandlingUtilities>().AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void MoveNoResources_NoChange()
        {
            SetupHolder(new(ResourceType.Stone, 1));
            CreateMoveRequest(new());

            AccountForAvailableResources();

            AssertMoveRequestResources(new());
        }

        [Test]
        public void MoveMoreOfAResourceThanAvailable_ReducedToMatchAvailability()
        {
            SetupHolder(new(ResourceType.Stone, 2));
            CreateMoveRequest(new(ResourceType.Stone, 3));

            AccountForAvailableResources();

            AssertMoveRequestResources(new(ResourceType.Stone, 2));
        }

        [Test]
        public void MoveLessOfAResourceThanAvailable_NoChange()
        {
            SetupHolder(new(ResourceType.Stone, 3));
            CreateMoveRequest(new(ResourceType.Stone, 2));

            AccountForAvailableResources();

            AssertMoveRequestResources(new(ResourceType.Stone, 2));
        }

        private void AccountForAvailableResources() =>
            utilities.AccountForAvailableResources(moveRequest);

        private void SetupHolder(Resources resources) =>
            holder.Setup(h => h.Resources).Returns(resources);

        private void CreateMoveRequest(Resources resources) =>
            moveRequest = new(holder.Object, new Mock<IHoldResources>().Object, resources);

        private void AssertMoveRequestResources(Resources expected) =>
            Assert.AreEqual(ResourcesToString(expected), ResourcesToString(moveRequest.Resources));
    }
}