using LittleKingdom;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class RaycastFromPointerTests : ZenjectUnitTestFixture
{
    [Inject] private readonly RaycastFromPointer raycastFromPointer;
    private readonly InputTestFixture input = new();
    private readonly Mock<IReferences> references = new();
    private readonly GameObject cameraObject = new();
    private readonly GameObject testObject = new();

    private Mouse mouse;

    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<IReferences>().FromInstance(references.Object).AsSingle();
        Container.Bind<Inputs>().AsSingle();
        Container.Bind<StandardInput>().AsSingle();
        Container.Bind<RaycastFromPointer>().AsSingle();
        Container.Inject(this);

        // TODO: JR - figure out how to do this.
        EventSystem.current = Object.Instantiate(AssetUtilities.LoadPrefab("Test EventSystem")).GetComponent<EventSystem>();

        mouse = InputSystem.AddDevice<Mouse>();
        var action2 = new InputAction("action2", binding: "<Mouse>/leftButton");
        action2.Enable();

        input.Set(mouse.position, new Vector2(Screen.width, Screen.height));

        testObject.AddComponent<BoxCollider>().size = new Vector3(1, 1);
        cameraObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        references.Setup(r => r.ActiveCamera).Returns(
            cameraObject.AddComponent<Camera>());
    }

    [Test]
    public void PositionObjectUnderPointer_RayCast_HitsObject()
    {
        testObject.transform.position = new(0, 0, 2);
        input.PressAndRelease(mouse.leftButton);

        Assert.IsTrue(raycastFromPointer.CastTo3D(out RaycastHit _));
    }

    [Test]
    public void PositionObjectNotUnderPointer_RayCast_DoesNotHitObject()
    {
        testObject.transform.position = new(2, 0, 0);
        input.PressAndRelease(mouse.leftButton);

        Assert.IsFalse(raycastFromPointer.CastTo3D(out RaycastHit _));
    }
}