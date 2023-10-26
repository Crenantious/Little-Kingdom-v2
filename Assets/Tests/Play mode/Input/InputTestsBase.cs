using LittleKingdom;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public abstract class InputTestsBase : ZenjectUnitTestFixture
{
    private readonly List<GameObject> gameObjects = new(2);

    [Inject] protected RaycastFromPointer RaycastFromPointer { get; }
    [Inject] protected Inputs Inputs { get; }

    protected Mock<IReferences> References { get; set; } = new();

    /// <summary>
    /// An empty <see cref="GameObject"/> with a default <see cref="UnityEngine.Camera"/>.
    /// </summary>
    protected GameObject CameraObject { get; set; }

    /// <summary>
    /// An empty <see cref="GameObject"/> with a 1x1x1 box <see cref="Collider"/>.
    /// </summary>
    protected GameObject ObjectOne { get; set; }

    /// <summary>
    /// An empty <see cref="GameObject"/> with a 1x1x1 box <see cref="Collider"/>.
    /// </summary>
    protected GameObject ObjectTwo { get; set; }

    /// <summary>
    /// The <see cref="UnityEngine.Camera"/> component on <see cref="CameraObject"/>.
    /// </summary>
    protected Camera Camera { get; set; }

    public InputTestFixture InputTestFixture { get; private set; }

    protected virtual void SetupInputSystem() { }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        GameObject eventSystem = new();
        eventSystem.AddComponent<EventSystem>();
    }

    [SetUp]
    protected virtual void SetUp()
    {
        PreInstall();
        Install();
        PostInstall();
    }

    protected virtual void PreInstall()
    {
        CreateTestObjects();
        InputTestFixture = new();
        InputTestFixture.Setup();
    }

    protected virtual void Install()
    {
        References.Setup(r => r.ActiveCamera).Returns(Camera);

        Container.Bind<IReferences>().FromInstance(References.Object).AsSingle();
        Container.Bind<Inputs>().AsSingle();
        Container.Bind<StandardInput>().AsSingle();
        Container.Bind<RaycastFromPointer>().AsSingle();
        Container.Inject(this);
    }

    protected virtual void PostInstall()
    {
        SetupInputSystem();
    }

    [TearDown]
    public virtual void TearDown()
    {
        InputTestFixture.TearDown();
        gameObjects.ForEach(o => Object.Destroy(o));
        base.Teardown();
    }

    private void CreateTestObjects()
    {
        CameraObject = CreateTestObject(false);
        Camera = CameraObject.AddComponent<Camera>();
        ObjectOne = CreateTestObject();
        ObjectTwo = CreateTestObject();
    }

    protected GameObject CreateTestObject(bool addCollider = true)
    {
        GameObject TestObject = new();
        gameObjects.Add(TestObject);

        if (addCollider)
            TestObject.AddComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        return TestObject;
    }
}