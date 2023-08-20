using LittleKingdom;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputTestsBase : InputTestFixture
{
    [Inject] protected RaycastFromPointer RaycastFromPointer { get; set; }
    [Inject] protected Inputs Inputs { get; set; }

    protected DiContainer Container { get; private set; }

    protected Mock<IReferences> References { get; set; } = new();

    /// <summary>
    /// An empty <see cref="GameObject"/> with a default <see cref="UnityEngine.Camera"/>.
    /// </summary>
    protected GameObject CameraObject { get; set; }

    /// <summary>
    /// An empty <see cref="GameObject"/> with a 1x1x1 box <see cref="Collider"/>.
    /// </summary>
    protected GameObject TestObject { get; set; }

    /// <summary>
    /// The <see cref="UnityEngine.Camera"/> component on <see cref="CameraObject"/>.
    /// </summary>
    protected Camera Camera { get; set; }

    [SetUp]
    public virtual void SetUp()
    {
        CreateTestObjects();

        References.Setup(r => r.ActiveCamera)
            .Returns(CameraObject.AddComponent<Camera>());

        Container = new(StaticContext.Container);
    }

    protected virtual void CommonInstall()
    {
        Container.Bind<IReferences>().FromInstance(References.Object).AsSingle();
        Container.Bind<Inputs>().AsSingle();
        Container.Bind<IStandardInput>().To<StandardInput>().AsSingle();
        Container.Bind<RaycastFromPointer>().AsSingle();
        Container.Inject(this);
    }

    private void CreateTestObjects()
    {
        CameraObject = new();
        TestObject = new();
        TestObject.AddComponent<BoxCollider>().size = new Vector3(1, 1, 1);
    }
}