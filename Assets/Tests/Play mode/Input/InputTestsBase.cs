using LittleKingdom;
using LittleKingdom.Input;
using LittleKingdom.UI;
using Moq;
using NUnit.Framework;
using PlayModeTests;
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
    [Inject] protected PlayModeTestHelper TestHelper { get; }

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
    /// An empty <see cref="GameObject"/> with a 1x1x1 box <see cref="Collider"/>.
    /// </summary>
    protected GameObject ObjectThree { get; set; }

    /// <summary>
    /// An empty <see cref="GameObject"/> with a 1x1x1 box <see cref="Collider"/>.
    /// </summary>
    protected GameObject ObjectFour { get; set; }

    /// <summary>
    /// An empty <see cref="GameObject"/> with no <see cref="Collider"/> that should be positioned away from all
    /// <see cref="GameObject"/>s with <see cref="Collider"/>s to mimic a point in empty space.
    /// </summary>
    protected GameObject EmptySpace { get; set; }

    /// <summary>
    /// A <see cref="GameObject"/> with the <see cref="UnityEngine.Camera"/> component.
    /// </summary>
    protected Camera Camera { get; set; }
    
    public InputTestFixture InputTestFixture { get; private set; }

    protected virtual void SetupInputSystem() { }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // TODO: JR - fix this regarding the DialogBox.
        Object.Instantiate(TestUtilities.LoadPrefab("Test EventSystem"));

        // Required to produce consistent results when using physics (i.e. raycasts and moving GameObjects).
        // The values don't matter as long as they're the same. Not sure why.
        Time.fixedDeltaTime = Time.captureDeltaTime = 1;
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
        // TODO: JR - ensure the PlayModeInstaller meets all needs then cleanup the binds (from here) it installs.
        References.Setup(r => r.ActiveCamera).Returns(Camera);
        //playModeInstaller.ExcludeFromInstall( PlayModeInstaller.BindType.);
        UIInstaller.ExcludeFromInstall(UIInstaller.BindType.PlayerHUD);
        PlayModeInstaller.Install(Container);
        //UIInstaller.InstallFromResource(Container);
        //Container.Bind<IReferences>().FromInstance(References.Object).AsSingle();
        //Container.Bind<Inputs>().AsSingle();
        //Container.Bind<StandardInput>().AsSingle();
        //Container.Bind<RaycastFromPointer>().AsSingle();
        //var dialogBox = Container.InstantiatePrefabForComponent<DialogBox>(TestUtilities.LoadPrefab("Dialog box"));
        //Container.BindInstance(dialogBox).AsSingle();
        //Container.Bind<PlayModeTestHelper>().AsSingle();
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
        CameraObject = CreateTestObject("Camera", false);
        Camera = CameraObject.AddComponent<Camera>();
        ObjectOne = CreateTestObject("ObjectOne");
        ObjectTwo = CreateTestObject("ObjectTwo");
        ObjectThree = CreateTestObject("ObjectThree");
        ObjectFour = CreateTestObject("ObjectFour");
        EmptySpace = CreateTestObject("EmptySpace", false);
    }

    protected GameObject CreateTestObject(string name, bool addCollider = true)
    {
        GameObject TestObject = new()
        {
            name = name
        };
        gameObjects.Add(TestObject);

        if (addCollider)
            TestObject.AddComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        return TestObject;
    }
}