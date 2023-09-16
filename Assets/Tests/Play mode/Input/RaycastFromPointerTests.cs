using LittleKingdom.Input;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class RaycastFromPointerTests : InputTestsBase
{
    private Mouse mouse;

    protected override void SetupInputSystem()
    {
        base.SetupInputSystem();
        mouse = InputSystem.AddDevice<Mouse>();
        Inputs.Standard.Enable();
    }

    [UnityTest]
    public IEnumerator PositionObjectUnderPointer_RayCast_HitsObject()
    {
        Object1.transform.position = new(0, 0, 2);
        MoveMouseToCentreOfScreen();
        yield return null;

        Assert.IsTrue(RaycastFromPointer.CastTo3D(out RaycastHit _));
    }

    [UnityTest]
    public IEnumerator PositionObjectNotUnderPointer_RayCast_DoesNotHitObject()
    {
        Object1.transform.position = new(2, 0, 0);
        MoveMouseToCentreOfScreen();
        yield return null;

        Assert.IsFalse(RaycastFromPointer.CastTo3D(out RaycastHit _));
    }

    private void MoveMouseToCentreOfScreen() =>
        InputTestFixture.Move(mouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
}