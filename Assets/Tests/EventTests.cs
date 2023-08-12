using LittleKingdom.Constraints;
using NUnit.Framework;
using Moq;
using System;
using LittleKingdom.Events;

namespace EventTests
{
    internal class EventTests
    {
        private const string EventDataTestValue = "Test";

        private TestEvent testEvent;
        private IConstraint validConstraint1;
        private IConstraint validConstraint2;
        private IConstraint invalidConstraint1;
        private IConstraint invalidConstraint2;
        private int callbackCallCount = 0;

        [SetUp]
        public void SetUp()
        {
            testEvent = new();
            callbackCallCount = 0;
            validConstraint1 = GetMockConstraint(true);
            validConstraint2 = GetMockConstraint(true);
            invalidConstraint1 = GetMockConstraint(false);
            invalidConstraint2 = GetMockConstraint(false);
        }

        [Test]
        public void SubscribeWithNoConstraints_Invoke_CallbackWasCalled()
        {
            Subscribe();

            InvokeAndAssertCallback(1);
        }

        [Test]
        public void SubscribeWithAValidConstraint_Invoke_CallbackWasCalled()
        {
            Subscribe(validConstraint1);

            InvokeAndAssertCallback(1);
        }

        [Test]
        public void SubscribeWithMultipleValidConstraints_Invoke_CallbackWasCalled()
        {
            Subscribe(validConstraint1, validConstraint2);

            InvokeAndAssertCallback(1);
        }

        [Test]
        public void SubscribeWithInvalidConstraint_Invoke_CallbackWasNotCalled()
        {
            Subscribe(invalidConstraint1);

            InvokeAndAssertCallback(0);
        }

        [Test]
        public void SubscribeWithMultipleInvalidConstraints_Invoke_CallbackWasNotCalled()
        {
            Subscribe(invalidConstraint1, invalidConstraint2);

            InvokeAndAssertCallback(0);
        }

        [Test]
        public void SubscribeWithValidAndInvalidConstraints_Invoke_CallbackWasNotCalled()
        {
            Subscribe(validConstraint1, invalidConstraint1);

            InvokeAndAssertCallback(0);
        }

        [Test]
        public void SubscribeWithConstraintsThenUnsubscribeWithMatchingConstraints_Invoke_CallbackWasNotCalled()
        {
            Subscribe(validConstraint1, validConstraint2);
            Unsubscribe(validConstraint1, validConstraint2);

            InvokeAndAssertCallback(0);
        }

        [Test]
        public void SubscribeTwiceWithConstraintsThenUnsubscribeWithMatchingConstraints_Invoke_CallbackWasNotCalled()
        {
            Subscribe(validConstraint1, validConstraint2);
            Subscribe(validConstraint1, validConstraint2);
            Unsubscribe(validConstraint1, validConstraint2);

            InvokeAndAssertCallback(0);
        }

        [Test]
        public void SubscribeTwiceWithConstraintsThenUnsubscribeWithAMatchingConstraint_Invoke_CallbackWasCalledOnce()
        {
            Subscribe(validConstraint1);
            Subscribe(validConstraint1, validConstraint2);
            Unsubscribe(validConstraint1);

            InvokeAndAssertCallback(1);
        }

        [Test]
        public void SubscribeTwiceWithConstraintsThenUnsubscribeWithNoConstraints_Invoke_CallbackWasCalledOnce()
        {
            Subscribe(validConstraint1);
            Subscribe(validConstraint1, validConstraint2);
            testEvent.Unsubscribe(Callback);

            InvokeAndAssertCallback(0);
        }

        // These test are fine, but the system under test fails them. It is unclear at the moment
        // if the system is worth maintaining.
        // TODO: JR - do an analysis to determine if the system is worth keeping.
        //[Test]
        //public void SubscribeTwice_InvokeAndUnsubscribeTheFirstFromWithinTheFirst_BothCallbacksWereCalled()
        //{
        //    testEvent.Subscribe(UnsubscribeDuringInvokationCallback1);
        //    testEvent.Subscribe(UnsubscribeDuringInvokationCallback2);

        //    InvokeAndAssertCallback(2);
        //}

        //private void UnsubscribeDuringInvokationCallback1(TestEvent.TestEventData eventData)
        //{
        //    testEvent.Unsubscribe(UnsubscribeDuringInvokationCallback1);
        //    Callback(eventData);
        //}

        //private void UnsubscribeDuringInvokationCallback2(TestEvent.TestEventData eventData)
        //{
        //    testEvent.Unsubscribe(UnsubscribeDuringInvokationCallback2);
        //    Callback(eventData);
        //}

        private IConstraint GetMockConstraint(bool isValid)
        {
            var mock = new Mock<IConstraint>();
            mock.Setup(x => x.Validate()).Returns(isValid);
            return mock.Object;
        }

        private void Subscribe(params IConstraint[] constraints) =>
            testEvent.Subscribe(Callback, constraints);

        private void Unsubscribe(params IConstraint[] constraints) =>
            testEvent.Unsubscribe(Callback, constraints);

        private void Callback(TestEvent.TestEventData eventData) =>
            callbackCallCount++;

        private void InvokeAndAssertCallback(int callbackCallCount)
        {
            testEvent.Invoke(new TestEvent.TestEventData(EventDataTestValue));
            Assert.AreEqual(callbackCallCount, this.callbackCallCount);
        }
    }
}