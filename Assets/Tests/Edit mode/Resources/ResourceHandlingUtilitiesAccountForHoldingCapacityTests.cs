using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Zenject;

namespace ResourceRequests
{
    public class ResourceHandlingUtilitiesGetMatchingHaltRequestsTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly ResourceHandlingUtilities utilities;

        private IEnumerable<HaltResourcesRequest> haltRequests;
        private MoveResourcesRequest moveRequest;

        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<ResourceHandlingUtilities>().AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithMatchingToAndFromHolders_GetMatching_GotTheHaltRequest()
        {
            var from = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(from, to);
            CreateMoveRequest(from, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests(haltRequest);
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithMatchingToAndDifferentFromHolders_GetMatching_GotNone()
        {
            var from1 = CreateHolder();
            var from2 = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(from1, to);
            CreateMoveRequest(from2, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests();
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithDifferentToAndMatchingFromHolders_GetMatching_GotNone()
        {
            var from = CreateHolder();
            var to1 = CreateHolder();
            var to2 = CreateHolder();
            var haltRequest = CreateHaltRequest(from, to1);
            CreateMoveRequest(from, to2);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests();
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithDifferentToAndFromHolders_GetMatching_GotNone()
        {
            var from1 = CreateHolder();
            var from2 = CreateHolder();
            var to1 = CreateHolder();
            var to2 = CreateHolder();
            var haltRequest = CreateHaltRequest(from1, to1);
            CreateMoveRequest(from2, to2);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests();
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithMatchingToHoldersAndHaltFromHolderIsNull_GetMatching_GotTheHaltRequest()
        {
            var from = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(null, to);
            CreateMoveRequest(from, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests(haltRequest);
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithMatchingFromHoldersAndHaltToHolderIsNull_GetMatching_GotTheHaltRequest()
        {
            var from = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(from, null);
            CreateMoveRequest(from, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests(haltRequest);
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithHaltToAndFromHoldersNull_GetMatching_GotTheHaltRequest()
        {
            var from = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(null, null);
            CreateMoveRequest(from, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests(haltRequest);
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithDifferentToHoldersAndHaltFromHolderIsNull_GetMatching_GotNone()
        {
            var from = CreateHolder();
            var to1 = CreateHolder();
            var to2 = CreateHolder();
            var haltRequest = CreateHaltRequest(null, to1);
            CreateMoveRequest(from, to2);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests();
        }

        [Test]
        public void CreateMoveAndHaltRequestsWithDifferentFromHoldersAndHaltToHolderIsNull_GetMatching_GotNone()
        {
            var from1 = CreateHolder();
            var from2 = CreateHolder();
            var to = CreateHolder();
            var haltRequest = CreateHaltRequest(from1, null);
            CreateMoveRequest(from2, to);
            SetHaltRequests(haltRequest);

            GetMatchingHaltRequests();

            AssertHaltRequests();
        }

        private IHoldResources CreateHolder() => new Mock<IHoldResources>().Object;

        private void CreateMoveRequest(IHoldResources from, IHoldResources to) =>
            moveRequest = new(from, to, new());

        private HaltResourcesRequest CreateHaltRequest(IHoldResources from, IHoldResources to) =>
            new(from, to, new());

        private void SetHaltRequests(params HaltResourcesRequest[] requests) =>
            haltRequests = requests;

        private void GetMatchingHaltRequests() =>
            haltRequests = utilities.GetMatchingHaltRequests(moveRequest, haltRequests);

        private void AssertHaltRequests(params HaltResourcesRequest[] requests) =>
            CollectionAssert.AreEqual(requests, haltRequests);
    }
}