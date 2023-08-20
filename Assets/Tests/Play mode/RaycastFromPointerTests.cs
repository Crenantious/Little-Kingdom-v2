using LittleKingdom;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Zenject;

public class RaycastFromPointerTests : InputTestFixture
{
    [Inject] private readonly RaycastFromPointer raycastFromPointer;
    [Inject] private readonly Inputs inputs;

    private readonly Mock<IReferences> references = new();
    private GameObject cameraObject = new();
    private GameObject testObject = new();

    [SetUp]
    public void CommonInstall()
    {
        CreateTestObjects();

        references.Setup(r => r.ActiveCamera)
            .Returns(cameraObject.AddComponent<Camera>());

        DiContainer Container = new(StaticContext.Container);
        Container.Bind<IReferences>().FromInstance(references.Object).AsSingle();
        Container.Bind<Inputs>().AsSingle();
        Container.Bind<IStandardInput>().To<StandardInput>().AsSingle();
        Container.Bind<RaycastFromPointer>().AsSingle();
        Container.Inject(this);

        MoveMouseToCentreOfScreen();
    }

    [UnityTest]
    public IEnumerator PositionObjectUnderPointer_RayCast_HitsObject()
    {
        testObject.transform.position = new(0, 0, 2);
        
        yield return null;

        Assert.IsTrue(raycastFromPointer.Cast(out RaycastHit _));
    }

    [UnityTest]
    public IEnumerator PositionObjectNotUnderPointer_RayCast_DoesNotHitObject()
    {
        testObject.transform.position = new(2, 0, 0);

        yield return null;

        Assert.IsFalse(raycastFromPointer.Cast(out RaycastHit _));
    }

    private void CreateTestObjects()
    {
        cameraObject = new();
        testObject = new();
        testObject.AddComponent<BoxCollider>().size = new Vector3(1, 1);
    }

    private void MoveMouseToCentreOfScreen()
    {
        Mouse mouse = InputSystem.AddDevice<Mouse>();
        inputs.Standard.Enable();
        Move(mouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
    }
}