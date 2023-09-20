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

        public object Current => null;

        public PlayModeTestHelper(DialogBox dialogBox) =>
            this.dialogBox = dialogBox;

        /// <summary>
        /// Initialise and display the panel.
        /// </summary>
        /// <param name="extraButtons"></param>
        public void Initialise(params (string text, Action callback)[] extraButtons)
        {
            CreateCamera();

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
        /// Initialise and display the panel. Creates a button called "Verify",
        /// or "Verify and conclude" if <paramref name="concludeAfterVerify"/> is true.
        ///</summary>
        /// <param name="verifyCallback">Called when the "Verify"/"Verify and conclude" button is pressed.
        /// Should be used for things such as Asserts.</param>
        public void Initialise(Action verifyCallback, bool concludeAfterVerify)
        {
            CreateCamera();

            (string, Action) verifyButton = concludeAfterVerify ? 
                ("Verify and conclude", () => { verifyCallback(); ConcludeTest(); }) :
                ("Verify", verifyCallback);

            dialogBox.Open("Test helper", false,
                ("Pass", () => ConcludeTest(true)),
                verifyButton,
                ("Fail", () => ConcludeTest(false)));
        }

        public bool MoveNext() => !isTestFinished;

        public void Reset() { }

        private void CreateCamera() =>
            new GameObject().AddComponent<Camera>();

        private void ConcludeTest() => isTestFinished = true;

        private void ConcludeTest(bool wasSuccessful)
        {
            ConcludeTest();
            Assert.IsTrue(wasSuccessful);
        }
    }
}