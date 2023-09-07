using LittleKingdom;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using ResourceRequestsTests;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ResourceRequests
{
    public class ResourceRequestsTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly ResourceRequests<IResourceRequestsTestsHandler, TestRequest> resourceRequests;

        private Mock<IResourceCollectionOrder> collectionOrder;
        private Mock<IPlayer> playerOne;
        private Mock<IPlayer> playerTwo;
        private Mock<IHoldResources> holderOne;
        private Mock<IHoldResources> holderTwo;
        private TestHandlerOne handlerOne;
        private TestHandlerTwo handlerTwo;
        private TestRequest nullToNullRequest;
        private TestRequest holderOneToHolderOnceRequest;
        private TestRequest holderOneToHolderTwoRequest;
        private TestRequest holderTwoToHolderOneRequest;
        private TestRequest holderTwoToHolderTwoRequest;
        private IEnumerable<TestRequest> requestsResult;

        [SetUp]
        public void CommonInstall()
        {
            collectionOrder = new();
            playerOne = new();
            playerTwo = new();
            handlerOne = new();
            handlerTwo = new();
            holderOne = new();
            holderTwo = new();

            nullToNullRequest = new(null, null);
            holderOneToHolderOnceRequest = new(holderOne.Object, holderOne.Object);
            holderOneToHolderTwoRequest = new(holderOne.Object, holderTwo.Object);
            holderTwoToHolderOneRequest = new(holderTwo.Object, holderOne.Object);
            holderTwoToHolderTwoRequest = new(holderTwo.Object, holderTwo.Object);

            collectionOrder.Setup(o => o.Halters).Returns(new Type[] { typeof(IResourceRequestsTestsHandler) });
            collectionOrder.Setup(o => o.GetOrderFor<IResourceRequestsTestsHandler>()).Returns(
                new Type[] { typeof(TestHandlerOne), typeof(TestHandlerTwo) });

            Container.Bind<ResourceRequests<IResourceRequestsTestsHandler, TestRequest>>().AsSingle();
            Container.BindInstance(collectionOrder.Object).AsSingle();
            Container.Inject(this);
        }

        [Test]
        public void RegisterHandlerWithPlayerOne_GetRequestsWithPlayerOne_AllGotten()
        {
            SetupHandler(handlerOne, playerOne, nullToNullRequest);

            GetRequests(playerOne);

            AssertResult(nullToNullRequest);
        }

        [Test]
        public void RegisterHandlerWithPlayerOne_GetRequestsWithPlayerTwo_NoneGotten()
        {
            SetupHandler(handlerOne, playerOne, nullToNullRequest);

            GetRequests(playerTwo);

            AssertResult();
        }

        [Test]
        public void RegisterHandlerWithNullPlayer_GetRequestsWithPlayerOne_AllGotten()
        {
            SetupHandler(handlerOne, null, nullToNullRequest);

            GetRequests(playerOne);

            AssertResult(nullToNullRequest);
        }

        [Test]
        public void RegisterHandler_GetRequestsWithNullPlayer_Throws()
        {
            SetupHandler(handlerOne, playerOne, nullToNullRequest);

            Assert.Throws<ArgumentNullException>(() => GetRequests(null));
        }

        [Test]
        public void RegisterTwoHandlersWithPlayerOne_GetRequestsWithPlayerOne_AllGotten()
        {
            SetupHandler(new TestHandlerOne(), playerOne, nullToNullRequest);
            SetupHandler(new TestHandlerOne(), playerOne, holderOneToHolderOnceRequest);

            GetRequests(playerOne);

            AssertResult(nullToNullRequest, holderOneToHolderOnceRequest);
        }

        [Test]
        public void RegisterHandlerWithPlayerOneAndHandlerWithPlayerTwo_GetRequestsWithPlayerOne_OnlyPlayerOneRequestsGotten()
        {
            SetupHandler(new TestHandlerOne(), playerOne, nullToNullRequest);
            SetupHandler(new TestHandlerOne(), playerTwo, holderOneToHolderOnceRequest);

            GetRequests(playerOne);

            AssertResult(nullToNullRequest);
        }

        [Test]
        public void RegisterManyHandlers_GetRequests_GotAllRequestsInCorrectOrder()
        {
            // Testing so many times to eliminate randomness with data structures such as HashSets.
            TestRequest[] requests = new TestRequest[10000];
            for (int i = 0; i < 10000; i++)
            {
                var request = GetUniqueRequest();
                requests[i] = request;
                SetupHandler(new TestHandlerOne(), playerOne, request);
            }

            GetRequests(playerOne);

            AssertResult(requests);
        }

        [Test]
        public void RegisterManyDifferentHandlers_GetRequests_GotAllRequestsInCorrectOrder()
        {
            // Testing so many times to eliminate randomness with data structures such as HashSets.
            TestRequest[] handlerOneRequests = new TestRequest[5000];
            TestRequest[] handlerTwoRequests = new TestRequest[5000];
            for (int i = 0; i < 10000; i++)
            {
                var request = GetUniqueRequest();
                if (i % 2 == 0)
                {
                    handlerOneRequests[i / 2] = request;
                    SetupHandler(new TestHandlerOne(), null, request);
                }
                else
                {
                    handlerTwoRequests[i / 2] = request;
                    SetupHandler(new TestHandlerTwo(), null, request);
                }
            }

            GetRequests(playerOne);

            AssertResult(handlerOneRequests.Concat(handlerTwoRequests).ToArray());
        }

        [Test]
        public void RegisterManyHandlersWithDifferentPlayers_GetRequestsWithPlayerOne_OnlyPlayerOneRequestsGotten()
        {
            // Testing so many times to eliminate randomness with data structures such as HashSets.
            TestRequest[] requests = new TestRequest[5000];
            for (int i = 0; i < 10000; i++)
            {
                if (i % 2 == 0)
                {
                    var request = GetUniqueRequest();
                    requests[i / 2] = request;
                    SetupHandler(new TestHandlerOne(), playerOne, request);
                }
                else
                    SetupHandler(new TestHandlerOne(), playerTwo, nullToNullRequest);
            }

            GetRequests(playerOne);

            AssertResult(requests);
        }

        [Test]
        public void RegisterHandlerWithManyRequests_GetRequests_GotInCorrectOrder()
        {
            // Testing so many times to eliminate randomness with data structures such as HashSets.
            TestRequest[] requests = new TestRequest[10000];
            for (int i = 0; i < 10000; i++)
                requests[i] = GetUniqueRequest();
            SetupHandler(new TestHandlerOne(), null, requests);

            GetRequests(playerOne);

            AssertResult(requests);
        }

        private static TestRequest GetUniqueRequest() =>
            new(new Mock<IHoldResources>().Object, null);

#nullable enable
        private void SetupHandler(ResourceRequestsTestsHandler handler, Mock<IPlayer>? player, params TestRequest[] requests)
        {
            handler.Requests = requests;
            handler.Player = player?.Object;
            resourceRequests.RegisterHandler(handler);
        }

        private void GetRequests(Mock<IPlayer>? player) =>
            requestsResult = resourceRequests.GetRequests(player?.Object);
#nullable disable

        private void AssertResult(params TestRequest[] requests) =>
            CollectionAssert.AreEqual(requests, requestsResult);

        //private void AssertResult(params TestRequest[] requests) =>
        //    CollectionAssert.AreEqual(requests, requestsResult);
    }
}