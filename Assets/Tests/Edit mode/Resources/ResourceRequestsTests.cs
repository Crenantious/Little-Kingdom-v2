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
        private TestHandlerOne handler;
        private TestRequest requestOne;
        private TestRequest RequestTwo;
        private IEnumerable<TestRequest> requestsResult;

        [SetUp]
        public void CommonInstall()
        {
            collectionOrder = new();
            playerOne = new();
            playerTwo = new();
            handler = new();

            requestOne = GetUniqueRequest();
            RequestTwo = GetUniqueRequest();

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
            SetupHandler(handler, playerOne, requestOne);

            GetRequests(playerOne);

            AssertResult(requestOne);
        }

        [Test]
        public void RegisterHandlerWithPlayerOne_GetRequestsWithPlayerTwo_NoneGotten()
        {
            SetupHandler(handler, playerOne, requestOne);

            GetRequests(playerTwo);

            AssertResult();
        }

        [Test]
        public void RegisterHandlerWithNullPlayer_GetRequestsWithPlayerOne_AllGotten()
        {
            SetupHandler(handler, null, requestOne);

            GetRequests(playerOne);

            AssertResult(requestOne);
        }

        [Test]
        public void RegisterHandler_GetRequestsWithNullPlayer_Throws()
        {
            SetupHandler(handler, playerOne, requestOne);

            Assert.Throws<ArgumentNullException>(() => GetRequests(null));
        }

        [Test]
        public void RegisterTwoHandlersWithPlayerOne_GetRequestsWithPlayerOne_AllGotten()
        {
            SetupHandler(new TestHandlerOne(), playerOne, requestOne);
            SetupHandler(new TestHandlerOne(), playerOne, RequestTwo);

            GetRequests(playerOne);

            AssertResult(requestOne, RequestTwo);
        }

        [Test]
        public void RegisterHandlerWithPlayerOneAndHandlerWithPlayerTwo_GetRequestsWithPlayerOne_OnlyPlayerOneRequestsGotten()
        {
            SetupHandler(new TestHandlerOne(), playerOne, requestOne);
            SetupHandler(new TestHandlerOne(), playerTwo, RequestTwo);

            GetRequests(playerOne);

            AssertResult(requestOne);
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
                    SetupHandler(new TestHandlerOne(), playerTwo, requestOne);
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
    }
}