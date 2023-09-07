using LittleKingdom;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ResourceRequests
{
    public class ResolveHaltRequestsTests : ZenjectUnitTestFixture
    {
        private const string AssertRequestError = "{0} request at index {1} did not contain the expected number of resources.";

        [Inject] private readonly ResolveHaltRequests resolveHaltRequests;

        private Mock<IResourceHandlingUtilities> utilities;
        private Mock<IHoldResources> holderOne;
        private Mock<IHoldResources> holderTwo;
        private IEnumerable<HaltResourcesRequest> haltRequests;
        private IEnumerable<MoveResourcesRequest> moveRequests;

        [SetUp]
        public void CommonInstall()
        {
            utilities = new();
            holderOne = new();
            holderTwo = new();

            Container.Bind<ResolveHaltRequests>().AsSingle();
            Container.BindInstance(utilities.Object).AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void AddMoveRequest_NoHaltRequests_MoveRequestIsUnchanged()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            SetHaltRequests();

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            AssertHaltRequests();
        }

        [Test]
        public void AddHaltRequest_NoMoveRequests_HaltRequestIsUnchanged()
        {
            SetMoveRequests();
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 1)));

            ResolveRequests();

            AssertMoveRequests();
            AssertHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 1)));
        }

        [Test]
        public void AddMoveRequest_HaltNoResources_RequestsAreUnchanged()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            SetHaltRequests(CreateHaltRequest(new()));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            AssertHaltRequests(CreateHaltRequest(new()));
        }

        [Test]
        public void AddMoveRequest_HaltAllResources_RequestsAreEmpty()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 1)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new()));
            AssertHaltRequests(CreateHaltRequest(new()));
        }

        [Test]
        public void AddMoveRequest_HaltSomeResources_RequestsReducedByHaltAmount()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 3)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 1)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 2)));
            AssertHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 0)));
        }

        [Test]
        public void AddMoveRequestWithTwoResourceTypes_HaltSomeOfEach_RequestsReducedByHaltAmount()
        {
            SetMoveRequests(CreateMoveRequest(new((ResourceType.Stone, 3), (ResourceType.Wood, 4))));
            SetHaltRequests(CreateHaltRequest(new((ResourceType.Stone, 1), (ResourceType.Wood, 3))));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new((ResourceType.Stone, 2), (ResourceType.Wood, 1))));
            AssertHaltRequests(CreateHaltRequest(new()));
        }

        [Test]
        public void AddMoveRequest_HaltMoreResourcesThanRequested_RequestsAreReducedByTheRequestedAmount()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 2)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 3)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new()));
            AssertHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 1)));
        }

        [Test]
        public void AddMoveRequest_HaltUnrequestedResources_RequestsAreUnchanged()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Metal, 1)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            AssertHaltRequests(CreateHaltRequest(new(ResourceType.Metal, 1)));
        }

        [Test]
        public void AddMoveRequest_HaltRequestedAndUnrequestedResources_RequestsAreReducedByTheRequestedResourcesOnly()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 1)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone | ResourceType.Wood, 1)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new()));
            AssertHaltRequests(CreateHaltRequest(new(ResourceType.Wood, 1)));
        }

        [Test]
        public void AddTwoMoveRequestWithTwoResourcesEach_HaltThreeResources_FirstMoveIsEmptySecondIsReducedByOneAndTheHaltIsEmpty()
        {
            SetMoveRequests(CreateMoveRequest(new(ResourceType.Stone, 2)),
                            CreateMoveRequest(new(ResourceType.Stone, 2)));
            SetHaltRequests(CreateHaltRequest(new(ResourceType.Stone, 3)));

            ResolveRequests();

            AssertMoveRequests(CreateMoveRequest(new()),
                               CreateMoveRequest(new(ResourceType.Stone, 1)));
            AssertHaltRequests(CreateHaltRequest(new()));
        }

        private HaltResourcesRequest CreateHaltRequest(Resources resources) =>
            new(holderOne.Object, holderTwo.Object, resources);

        private MoveResourcesRequest CreateMoveRequest(Resources resources) =>
            new(holderOne.Object, holderTwo.Object, resources);

        private void SetHaltRequests(params HaltResourcesRequest[] haltRequests) => this.haltRequests = haltRequests;

        private void SetMoveRequests(params MoveResourcesRequest[] moveRequests) => this.moveRequests = moveRequests;

        private void ResolveRequests()
        {
            utilities.Setup(u => u.GetMatchingHaltRequests(
                    It.IsAny<MoveResourcesRequest>(),
                    It.IsAny<IEnumerable<HaltResourcesRequest>>()))
                .Returns(haltRequests);
            resolveHaltRequests.Resolve(haltRequests, moveRequests);
        }

        private void AssertHaltRequests(params HaltResourcesRequest[] haltRequests) =>
            AssertRequests(haltRequests.Select(r => r.Resources), this.haltRequests.Select(r => r.Resources), "Halt");

        private void AssertMoveRequests(params MoveResourcesRequest[] moveRequests) =>
            AssertRequests(moveRequests.Select(r => r.Resources), this.moveRequests.Select(r => r.Resources), "Move");

        // Assert that each element in expected contains the same resources as the corresponding element in actual.
        private void AssertRequests(IEnumerable<Resources> expected, IEnumerable<Resources> actual, string name)
        {
            Assert.AreEqual(expected.Count(), actual.Count(),
                $"Test invalid! Asserted incorrect number of {name.ToLower()} requests. Expected {expected.Count()} but was {actual.Count()}.");

            for (int i = 0; i < expected.Count(); i++)
            {
                string error = AssertRequestError.FormatConst(name, i.ToString());
                Resources expectedResources = expected.ElementAt(i);
                Resources actualResources = actual.ElementAt(i);
                Assert.AreEqual(ResourcesToString(expectedResources), ResourcesToString(actualResources), error);
            }
        }

        public string ResourcesToString(Resources resources)
        {
            IDictionary<ResourceType, int> dict = resources.GetAll();
            IEnumerable<string> kvps = dict.Select(p => string.Join(": ", p.Key, p.Value));
            return string.Join(", ", kvps);
        }
    }
}