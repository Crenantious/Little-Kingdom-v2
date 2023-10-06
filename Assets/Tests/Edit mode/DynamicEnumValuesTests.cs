using LittleKingdom.DataStructures;
using NUnit.Framework;
using System;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GridTests
{
    public class DynamicEnumValuesTests : ZenjectUnitTestFixture
    {
        private SerializedObject serialisedValues;
        private DynamicEnumValues values;
        private string[] setValues;

        [SetUp]
        public void CommonInstall()
        {
            values = ScriptableObject.CreateInstance<DynamicEnumValues>();
            serialisedValues = new(values);
        }

        [Test]
        public void DoNotBeginEdit_ApplyEdit_GetException()
        {
            Assert.Throws<InvalidOperationException>(values.ApplyEdit);
        }


        [Test]
        public void DoNotBeginEdit_EndEdit_GetException()
        {
            Assert.Throws<InvalidOperationException>(values.EndEdit);
        }

        [Test]
        public void SetValues_ValuesSaved()
        {
            values.BeginEdit();

            SetValues("1", "2");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 0), ("2", 1));
        }

        [Test]
        public void SetValues_RemoveAValue_ValueRemoved()
        {
            values.BeginEdit();
            SetValues("1", "2");
            values.ApplyEdit();

            SetValues("1");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 0), ("2", -1));
        }

        [Test]
        public void SetValues_AddAValue_ValueAdded()
        {
            values.BeginEdit();
            SetValues("1", "2");
            values.ApplyEdit();

            SetValues("1", "2", "3");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 0), ("2", 1), ("3", 2));
        }

        [Test]
        public void SetValues_SwapTwo_IdsUnchanged()
        {
            values.BeginEdit();
            SetValues("1", "2");
            values.ApplyEdit();

            SetValues("2", "1");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 0), ("2", 1));
        }

        [Test]
        public void SetValues_RemoveAValue_AddThatValueBack_ThatValueHasANewId()
        {
            values.BeginEdit();
            SetValues("1");
            values.ApplyEdit();
            SetValues();
            values.ApplyEdit();

            SetValues("1");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 1));
        }

        [Test]
        public void SetTwoOfTheSameValue_TheSecondIsRemoved()
        {
            values.BeginEdit();

            SetValues("1", "1");
            values.ApplyEdit();

            CollectionAssert.AreEqual(new string[] { "1" }, values.Values);
            AssertIds(("1", 0));
        }


        [Test]
        public void SetAValue_SetThatValueAgain_TheSecondIsRemoved()
        {
            values.BeginEdit();
            SetValues("1");
            values.ApplyEdit();

            SetValues("1");
            values.ApplyEdit();

            CollectionAssert.AreEqual(setValues, values.Values);
            AssertIds(("1", 0));
        }

        [Test]
        public void BeginEdit_SetValues_EndEditWithoutApplying_NoValuesAreSet()
        {
            values.BeginEdit();
            SetValues("1");

            values.EndEdit();

            CollectionAssert.AreEqual(new string[0], values.Values);
        }

        private void SetValues(params string[] values)
        {
            serialisedValues.Update();

            SerializedProperty propertyValues = serialisedValues.FindProperty("editingValues");
            propertyValues.arraySize = values.Length;

            for (int i = 0; i < values.Length; i++)
            {
                propertyValues.GetArrayElementAtIndex(i).stringValue = values[i];
            }

            serialisedValues.ApplyModifiedProperties();
            setValues = values;
        }

        private void AssertIds(params (string value, int id)[] tests)
        {
            foreach (var (value, id) in tests)
            {
                Assert.AreEqual(id, values.GetId(value));
            }
        }
    }
}