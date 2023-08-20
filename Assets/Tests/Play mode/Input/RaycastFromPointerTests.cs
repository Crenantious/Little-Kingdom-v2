using LittleKingdom.Input;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class RaycastFromPointerTests : InputTestsBase
{
    private Mouse mouse;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        mouse = InputSystem.AddDevice<Mouse>();
        Inputs.Standard.Enable();

        CommonInstall();
    }

    [UnityTest]
    public IEnumerator PositionObjectUnderPointer_RayCast_HitsObject()
    {
        TestObject.transform.position = new(0, 0, 2);
        MoveMouseToCentreOfScreen();
        yield return null;

        Assert.IsTrue(RaycastFromPointer.Cast(out RaycastHit _));
    }

    [UnityTest]
    public IEnumerator PositionObjectNotUnderPointer_RayCast_DoesNotHitObject()
    {
        TestObject.transform.position = new(2, 0, 0);
        MoveMouseToCentreOfScreen();
        yield return null;

        Assert.IsFalse(RaycastFromPointer.Cast(out RaycastHit _));
    }

    private void MoveMouseToCentreOfScreen() =>
        Move(mouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
}