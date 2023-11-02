using LittleKingdom.UI;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayModeTests
{
    public class PlayModeTestHelper : IEnumerator
    {
        private readonly DialogBox dialogBox;

        private bool isTestFinished = false;
        private bool isPaused = false;

        public object Current => null;

        public PlayModeTestHelper(DialogBox dialogBox) =>
            this.dialogBox = dialogBox;

        /// <summary>
        /// Display a <see cref="DialogBox"/> with the "Pass", <paramref name="extraButtons"/> and "Fail" buttons.
        /// </summary>
        public void OpenDialogBox(params (string text, Action callback)[] extraButtons)
        {
            List<(string, Action)> buttons = new(extraButtons.Length + 2)
            {
                ("Pass", () => ConcludeTest(true))
            };
            foreach (var (text, callback) in extraButtons)
            {
                buttons.Add((text, callback));
            }
            buttons.Add(("Fail", () => ConcludeTest(false)));

            dialogBox.Open("Test helper", false, buttons.ToArray());
        }

        ///<summary>
        /// Display a <see cref="DialogBox"/> with the "Pass", "verify" and "Fail" buttons,
        /// or "Verify and conclude" if <paramref name="concludeAfterVerify"/> is true.
        ///</summary>
        /// <param name="verifyCallback">Called when the "Verify"/"Verify and conclude" button is pressed.
        /// Should be used for things such as Asserts.</param>
        public void OpenDialogBox(Action verifyCallback, bool concludeAfterVerify)
        {
            (string, Action) verifyButton = concludeAfterVerify ?
                ("Verify and conclude", () => { ConcludeTest(); verifyCallback(); } ) :
                ("Verify", verifyCallback);

            dialogBox.Open("Test helper", false,
                ("Pass", () => ConcludeTest(true)),
                verifyButton,
                ("Fail", () => ConcludeTest(false)));
        }

        public bool MoveNext() => !isTestFinished;

        public void Reset() { }

        /// <summary>
        /// Pauses the test to allow for scene inspection.
        /// Make sure to end the test with the "End" button rather than manually canceling it,
        /// as doing so can lead to a memory leak and persistent errors once the test has concluded.
        /// </summary>
        /// <param name="update">Called once per frame.</param>
        public IEnumerator Pause(Action update = null)
        {
            (string, Action)[] buttons = new (string, Action)[]
            {
                ("Play", () => isPaused = false),
                ("End", () => ConcludeTest(true))
            };

            dialogBox.Open("Test helper", true, buttons);

            isPaused = true;
            while (isPaused)
            {
                // TODO: JR - this causes recursive errors once the test is canceled that
                // requires Unity to be restarted. Fix.
                // Temporary fix is to conclude via the dialog box.
                update?.Invoke();
                yield return null;
            }
        }

        public void CreateCamera() =>
            new GameObject()
                .AddComponent<Camera>()
                .name = "Camera";

        private void ConcludeTest() => isTestFinished = true;

        private void ConcludeTest(bool wasSuccessful)
        {
            ConcludeTest();
            Assert.IsTrue(wasSuccessful);
        }
    }
}