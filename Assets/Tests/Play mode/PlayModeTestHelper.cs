using LittleKingdom.UI;
using NUnit.Framework;
using System;
using System.Collections;

namespace PlayModeTests
{
    public class PlayModeTestHelper : IEnumerator
    {
        private readonly DialogBox dialogBox;

        private Action verify;
        private bool isTestFinished = false;

        public object Current => null;

        public PlayModeTestHelper(DialogBox dialogBox) =>
            this.dialogBox = dialogBox;

        /// <param name="verify">Called when the "verify" button is pressed. Should be used for things such as Asserts.</param>
        public void Initialise(Action verify)
        {
            this.verify = verify;
            dialogBox.Open("Test helper",
                ("Pass", (s) => ConcludeTest(true)),
                ("Verify and conclude", (s) => VerifyAndConcludeTest()),
                ("Fail", (s) => ConcludeTest(false)));
        }

        public bool MoveNext() => !isTestFinished;

        public void Reset() { }

        private void ConcludeTest() => isTestFinished = true;
        private void VerifyAndConcludeTest()
        {
            isTestFinished = true;
            verify();
        }

        private void ConcludeTest(bool wasSuccessful)
        {
            ConcludeTest();
            Assert.IsTrue(wasSuccessful);
        }
    }
}