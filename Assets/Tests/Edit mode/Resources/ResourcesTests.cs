using LittleKingdom.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Zenject;

public class ResourcesTests : ZenjectUnitTestFixture
{
    private Resources resources1;
    private Resources resources2;

    [SetUp]
    public void CommonInstall()
    {
        resources1 = new((ResourceType.Glass | ResourceType.Stone, 1));
        resources2 = new((ResourceType.Glass | ResourceType.Stone, 1),
                         (ResourceType.Glass | ResourceType.Stone, 2),
                         (ResourceType.Glass, 1),
                         (ResourceType.Wood, 2),
                         (ResourceType.Metal, 1));
    }

    #region Creation
    [Test]
    public void CreateWithNoResourceTypes_CorrectlyStoredValues()
    {
        Resources resources = new();

        Assert.AreEqual(0, resources.Total);
        Assert.AreEqual(0, resources.Get(ResourceType.Glass));
        Assert.AreEqual(0, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithAnIndividualResourceType_CorrectlyStoredValues()
    {
        Resources resources = new((ResourceType.Glass, 1));

        Assert.AreEqual(1, resources.Total);
        Assert.AreEqual(1, resources.Get(ResourceType.Glass));
        Assert.AreEqual(0, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithIndividualResourceTypes_CorrectlyStoredValues()
    {
        Resources resources = new((ResourceType.Glass, 1), (ResourceType.Stone, 2));

        Assert.AreEqual(3, resources.Total);
        Assert.AreEqual(1, resources.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithACombination_CorrectlySeparatedIntoIndividualEnumValues()
    {
        Resources resources = new((ResourceType.Glass | ResourceType.Stone, 1));

        Assert.AreEqual(2, resources.Total);
        Assert.AreEqual(1, resources.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithAnIndividualAndACombination_CorrectlySeparatedIntoIndividualEnumValues()
    {
        Resources resources = new((ResourceType.Glass, 1), (ResourceType.Glass | ResourceType.Stone, 1));

        Assert.AreEqual(3, resources.Total);
        Assert.AreEqual(2, resources.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithMultipleCombinations_CorrectlySeparatedIntoIndividualEnumValues()
    {
        Resources resources = new((ResourceType.Glass | ResourceType.Stone, 1), (ResourceType.Glass | ResourceType.Wood, 1));

        Assert.AreEqual(4, resources.Total);
        Assert.AreEqual(2, resources.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithMultipleIndividualsAndCombinations_CorrectlySeparatedIntoIndividualEnumValues()
    {
        Resources resources = new((ResourceType.Glass | ResourceType.Stone, 1),
                                        (ResourceType.Glass, 1),
                                        (ResourceType.Glass | ResourceType.Stone, 1),
                                        (ResourceType.Glass | ResourceType.Wood, 1),
                                        (ResourceType.Metal | ResourceType.Brick, 1),
                                        (ResourceType.Brick, 1),
                                        (ResourceType.Brick, 1));

        Assert.AreEqual(11, resources.Total, 11);
        Assert.AreEqual(4, resources.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources.Get(ResourceType.Stone));
        Assert.AreEqual(1, resources.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources.Get(ResourceType.Metal));
        Assert.AreEqual(3, resources.Get(ResourceType.Brick));
        Assert.Throws<KeyNotFoundException>(() => resources.Get(ResourceType.None));
    }

    [Test]
    public void CreateWithANegativeResource_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Resources((ResourceType.Glass, -1)));
    }
    #endregion

    #region Addition
    [Test]
    public void AddTwoResources_CorrectlyAdded()
    {
        resources1.Add(resources2);

        Assert.AreEqual(12, resources1.Total);
        Assert.AreEqual(5, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(4, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));

        AssertResources2BaseValues();
    }

    [Test]
    public void AddTwoResourcesWithStaticMethod_CorrectlyAdded()
    {
        Resources result = Resources.Add(resources1, resources2);

        Assert.AreEqual(12, result.Total);
        Assert.AreEqual(5, result.Get(ResourceType.Glass));
        Assert.AreEqual(4, result.Get(ResourceType.Stone));
        Assert.AreEqual(2, result.Get(ResourceType.Wood));
        Assert.AreEqual(1, result.Get(ResourceType.Metal));
        Assert.AreEqual(0, result.Get(ResourceType.Brick));

        AssertResources1BaseValues();
        AssertResources2BaseValues();
    }

    [Test]
    public void AddResourceToResources_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass, 1);

        Assert.AreEqual(3, resources1.Total);
        Assert.AreEqual(2, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddResourceToResourcesTwice_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass, 1);
        resources1.Add(ResourceType.Glass, 1);

        Assert.AreEqual(4, resources1.Total);
        Assert.AreEqual(3, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddCombinationResourceToResources_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass | ResourceType.Stone, 1);

        Assert.AreEqual(4, resources1.Total);
        Assert.AreEqual(2, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddCombinationResourceToResourcesTwice_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass | ResourceType.Stone, 1);
        resources1.Add(ResourceType.Glass | ResourceType.Wood, 1);

        Assert.AreEqual(6, resources1.Total);
        Assert.AreEqual(3, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(1, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddIndividualAndCombinationResourceToResources_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass, 1);
        resources1.Add(ResourceType.Glass | ResourceType.Stone, 1);

        Assert.AreEqual(5, resources1.Total);
        Assert.AreEqual(3, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddNegativeAmountOfAResourceToResources_CorrectlyAdded()
    {
        resources1.Add(ResourceType.Glass, -1);

        Assert.AreEqual(1, resources1.Total);
        Assert.AreEqual(0, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void AddResourceToResourcesSuchThatItIsNegative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(()=> resources1.Add(ResourceType.Glass, -2));
    }
    #endregion

    #region Subtraction
    [Test]
    public void SubtractTwoResources_CorrectlySubtracted()
    {
        resources2.Subtract(resources1);

        Assert.AreEqual(8, resources2.Total);
        Assert.AreEqual(3, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));

        AssertResources1BaseValues();
    }

    [Test]
    public void SubtractTwoResourcesWithStaticMethod_CorrectlySubtracted()
    {
        Resources result = Resources.Subtract(resources2, resources1);

        Assert.AreEqual(8, result.Total);
        Assert.AreEqual(3, result.Get(ResourceType.Glass));
        Assert.AreEqual(2, result.Get(ResourceType.Stone));
        Assert.AreEqual(2, result.Get(ResourceType.Wood));
        Assert.AreEqual(1, result.Get(ResourceType.Metal));
        Assert.AreEqual(0, result.Get(ResourceType.Brick));

        AssertResources1BaseValues();
        AssertResources2BaseValues();
    }

    [Test]
    public void SubtractResourceFromResources_CorrectlySubtracted()
    {
        resources1.Subtract(ResourceType.Glass, 1);

        Assert.AreEqual(1, resources1.Total);
        Assert.AreEqual(0, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractResourceFromResourcesTwice_CorrectlySubtracted()
    {
        resources2.Subtract(ResourceType.Glass, 1);
        resources2.Subtract(ResourceType.Glass, 1);

        Assert.AreEqual(8, resources2.Total, 8);
        Assert.AreEqual(2, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(3, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractCombinationResourceFromResources_CorrectlySubtracted()
    {
        resources1.Subtract(ResourceType.Glass | ResourceType.Stone, 1);

        Assert.AreEqual(0, resources1.Total);
        Assert.AreEqual(0, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(0, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractCombinationResourceFromResourcesTwice_CorrectlySubtracted()
    {
        resources2.Subtract(ResourceType.Glass | ResourceType.Stone, 1);
        resources2.Subtract(ResourceType.Glass | ResourceType.Wood, 1);

        Assert.AreEqual(6, resources2.Total);
        Assert.AreEqual(2, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(1, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractNegativeAmountOfAResourceFromResourcesTwice_CorrectlySubtracted()
    {
        resources1.Subtract(ResourceType.Glass, -1);

        Assert.AreEqual(3, resources1.Total);
        Assert.AreEqual(2, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractIndividualAndCombinationResourceFromResources_CorrectlySubtracted()
    {
        resources2.Subtract(ResourceType.Glass, 1);
        resources2.Subtract(ResourceType.Glass | ResourceType.Stone, 1);

        Assert.AreEqual(7, resources2.Total);
        Assert.AreEqual(2, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(2, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));
    }

    [Test]
    public void SubtractTooManyResourceFromResources_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => resources1.Subtract(ResourceType.Glass, 5));
    }
    #endregion

    [Test]
    public void ClampMin_GetCorrectResults()
    {
        Resources resources = new((ResourceType.Glass | ResourceType.Wood, 2), (ResourceType.Stone, 5));
        resources2.ClampMin(resources);

        Assert.AreEqual(7, resources2.Total);
        Assert.AreEqual(2, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(3, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));
    }

    [Test]
    public void ClampMinStatic_GetCorrectResults()
    {
        Resources resources = new((ResourceType.Glass | ResourceType.Wood, 2), (ResourceType.Stone, 5));
        Resources result = Resources.ClampMin(resources, resources2);

        Assert.AreEqual(7, result.Total);
        Assert.AreEqual(2, result.Get(ResourceType.Glass));
        Assert.AreEqual(3, result.Get(ResourceType.Stone));
        Assert.AreEqual(2, result.Get(ResourceType.Wood));
        Assert.AreEqual(0, result.Get(ResourceType.Metal));
        Assert.AreEqual(0, result.Get(ResourceType.Brick));
    }

    private void AssertResources1BaseValues()
    {
        Assert.AreEqual(2, resources1.Total);
        Assert.AreEqual(1, resources1.Get(ResourceType.Glass));
        Assert.AreEqual(1, resources1.Get(ResourceType.Stone));
        Assert.AreEqual(0, resources1.Get(ResourceType.Wood));
        Assert.AreEqual(0, resources1.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources1.Get(ResourceType.Brick));
    }

    private void AssertResources2BaseValues()
    {
        Assert.AreEqual(10, resources2.Total);
        Assert.AreEqual(4, resources2.Get(ResourceType.Glass));
        Assert.AreEqual(3, resources2.Get(ResourceType.Stone));
        Assert.AreEqual(2, resources2.Get(ResourceType.Wood));
        Assert.AreEqual(1, resources2.Get(ResourceType.Metal));
        Assert.AreEqual(0, resources2.Get(ResourceType.Brick));
    }
}