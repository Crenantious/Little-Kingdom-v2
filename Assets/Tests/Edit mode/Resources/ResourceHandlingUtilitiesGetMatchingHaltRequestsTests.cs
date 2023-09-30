using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using Zenject;
using static ResourceRequestsTests.ResourceRequestsTestsUtilities;

namespace ResourceRequests
{
    public class ResourceHandlingUtilitiesAccountForHoldingCapacityResourcesTests : ZenjectUnitTestFixture
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
            SetupHolder(new(ResourceType.Stone, 0), new(ResourceType.Stone, 1));
            CreateMoveRequest(new());

            AccountForHoldingCapacity();

            AssertMoveRequestResources(new());
        }

        [Test]
        public void MoveMoreOfAResourceThanCapacity_ReducedToMatchCapacity()
        {
            SetupHolder(new(ResourceType.Stone, 0), new(ResourceType.Stone, 2));
            CreateMoveRequest(new(ResourceType.Stone, 3));

            AccountForHoldingCapacity();

            AssertMoveRequestResources(new(ResourceType.Stone, 2));
        }

        [Test]
        public void MoveLessOfAResourceThanCapacityButEnoughToOverflow_ReducedToNotOverflow()
        {
            SetupHolder(new(ResourceType.Stone, 2), new(ResourceType.Stone, 5));
            CreateMoveRequest(new(ResourceType.Stone, 10));

            AccountForHoldingCapacity();

            AssertMoveRequestResources(new(ResourceType.Stone, 3));
        }

        [Test]
        public void MoveEnoughOfAResourceToNotReachCapacity_NoChange()
        {
            SetupHolder(new(ResourceType.Stone, 2), new(ResourceType.Stone, 5));
            CreateMoveRequest(new(ResourceType.Stone, 1));

            AccountForHoldingCapacity();

            AssertMoveRequestResources(new(ResourceType.Stone, 1));
        }

        [Test]
        public void MoveEnoughOfAResourceToMatchCapacity_NoChange()
        {
            SetupHolder(new(ResourceType.Stone, 2), new(ResourceType.Stone, 5));
            CreateMoveRequest(new(ResourceType.Stone, 3));

            AccountForHoldingCapacity();

            AssertMoveRequestResources(new(ResourceType.Stone, 3));
        }

        private void AccountForHoldingCapacity() =>
            utilities.AccountForHoldingCapacity(moveRequest);

        private void SetupHolder(Resources resources, Resources capacity)
        {
            holder.Setup(h => h.Resources).Returns(resources);
            holder.Setup(h => h.ResourcesCapacity).Returns(capacity);
        }

        private void CreateMoveRequest(Resources resources) =>
            moveRequest = new(new Mock<IHoldResources>().Object, holder.Object, resources);

        private void AssertMoveRequestResources(Resources expected) =>
            Assert.AreEqual(ResourcesToString(expected), ResourcesToString(moveRequest.Resources));
    }
}