using LittleKingdom;
using LittleKingdom.Interactions;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Zenject;

public class InteractionTests : InputTestsBase
{
    [Inject] private readonly InteractionUtilities interactionUtilities;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        Container.Bind<InteractionUtilities>().AsSingle();
        CommonInstall();
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedAndNotInProhibited_StatesAreValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsTrue(interactionUtilities.AreValidStates(GameState.UILocked, GameState.LoadingScreen));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsNotInAllowedAndNotInProhibited_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsFalse(interactionUtilities.AreValidStates(GameState.LoadingScreen, GameState.StandardInGame));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedAndInProhibited_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsFalse(interactionUtilities.AreValidStates(GameState.UILocked, GameState.UILocked));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsNotInAllowedAndInProhibited_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsFalse(interactionUtilities.AreValidStates(GameState.StandardInGame, GameState.UILocked));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateNone_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.None);

        Assert.IsFalse(interactionUtilities.AreValidStates(GameState.StandardInGame, GameState.UILocked));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedWithMultipleStatesAndNotInProhibited_StatesAreValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsTrue(interactionUtilities.AreValidStates(GameState.UILocked | GameState.StandardInGame, GameState.LoadingScreen));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedWithMultipleStatesAndInProhibited_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsFalse(interactionUtilities.AreValidStates(GameState.UILocked | GameState.StandardInGame, GameState.UILocked));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedWithMultipleStatesAndNotInProhibitedWithMultipleStates_StatesAreValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsTrue(interactionUtilities.AreValidStates(
            GameState.UILocked | GameState.StandardInGame,
            GameState.StandardInGame | GameState.PlacingTowns));
        yield return null;
    }

    [UnityTest]
    public IEnumerator CurrentStateIsInAllowedWithMultipleStatesAndInProhibitedWithMultipleStates_StatesAreNotValid()
    {
        References.Setup(r => r.GameState).Returns(GameState.UILocked);

        Assert.IsFalse(interactionUtilities.AreValidStates(
            GameState.UILocked | GameState.StandardInGame,
            GameState.UILocked | GameState.PlacingTowns));
        yield return null;
    }
}