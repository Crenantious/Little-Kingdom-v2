using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using ResourceRequestsTests;
using System.Collections.Generic;
using Zenject;

namespace ResourceRequests
{
    public class ResolveMoveRequestsTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly ResolveMoveRequests resolveMoveRequests;

        private Mock<IHoldResources> holderOne;
        private Mock<IHoldResources> holderTwo;
        private IEnumerable<MoveResourcesRequest> moveRequests;

        [SetUp]
        public void CommonInstall()
        {
            holderOne = new();
            holderTwo = new();

            holderOne.Setup(h => h.Resources).Returns(new Resources(ResourceType.Stone | ResourceType.Wood, 10));
            holderTwo.Setup(h => h.Resources).Returns(new Resources(ResourceType.Stone | ResourceType.Wood, 10));

            Container.Bind<ResolveMoveRequests>().AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void MoveNoResources_NoChange()
        {
            SetMoveRequests(CreateMoveRequest(new()));

            ResolveMoveRequests();

            AssertHolders(new(ResourceType.Stone | ResourceType.Wood, 10),
                          new(ResourceType.Stone | ResourceType.Wood, 10));
        }

        [Test]
        public void MoveOneResourceType_CorrectlyMoved()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));

            ResolveMoveRequests();

            AssertHolders(new((ResourceType.Stone, 9), (ResourceType.Wood, 10)),
                          new((ResourceType.Stone, 11), (ResourceType.Wood, 10)));
        }

        [Test]
        public void MoveMultipleResourceTypes_CorrectlyMoved()
        {
            SetMoveRequests(CreateMoveRequest(new((ResourceType.Stone, 1), (ResourceType.Wood, 2))));

            ResolveMoveRequests();

            AssertHolders(new((ResourceType.Stone, 9), (ResourceType.Wood, 8)),
                          new((ResourceType.Stone, 11), (ResourceType.Wood, 12)));
        }

        private MoveResourcesRequest CreateMoveRequest(Resources resources) =>
            new(holderOne.Object, holderTwo.Object, resources);

        private void SetMoveRequests(params MoveResourcesRequest[] requests) =>
            moveRequests = requests;

        private void ResolveMoveRequests() =>
            resolveMoveRequests.Resolve(moveRequests);

        private void AssertHolders(Resources holderOneResources, Resources holderTwoResources)
        {
            Assert.AreEqual(ResourceRequestsTestsUtilities.ResourcesToString(holderOneResources),
                            ResourceRequestsTestsUtilities.ResourcesToString(holderOne.Object.Resources));
            Assert.AreEqual(ResourceRequestsTestsUtilities.ResourcesToString(holderTwoResources),
                            ResourceRequestsTestsUtilities.ResourcesToString(holderTwo.Object.Resources));
        }
    }
}