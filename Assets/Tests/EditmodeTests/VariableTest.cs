using NUnit.Framework;
using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class VariableTest
{
    /// <summary>
    /// Test int variable
    /// </summary>
    [Test]
    public void TestIntVariable()
    {
        IntVariable intVariable = ScriptableObject.CreateInstance<IntVariable>();
        intVariable.VariableType = VariableType.Variable;
        intVariable.InitializeTypeVariable = InitializeType.Normal;

        intVariable.Value = 1;

        Assert.AreEqual(1, intVariable.Value);

        intVariable.Value += 3;
        Assert.AreEqual(4, intVariable.Value);

        intVariable.Set(6);
        Assert.AreEqual(6, intVariable.Value);
    }

    /// <summary>
    /// Test bool variable
    /// </summary>
    [Test]
    public void TestBoolVariable()
    {
        BoolVariable boolVariable = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariable.VariableType = VariableType.Variable;
        boolVariable.InitializeTypeVariable = InitializeType.Normal;

        boolVariable.Value = true;

        Assert.AreEqual(true, boolVariable.Value);

        boolVariable.Invert();
        Assert.AreEqual(false, boolVariable.Value);
    }

    /// <summary>
    /// Test runtimeSet
    /// </summary>
    [Test]
    public void TestRuntimeSet()
    {
        FloatVariable floatSet = ScriptableObject.CreateInstance<FloatVariable>();
        floatSet.VariableType = VariableType.RuntimeSet;
        floatSet.InitializeTypeVariable = InitializeType.Normal;
        floatSet.InitializeRuntimeSet();

        floatSet.Add(1);

        Assert.AreEqual(new List<int> { 1 }, floatSet.RuntimeSet);

        floatSet.Add(2);
        Assert.AreEqual(new List<int> { 1, 2 }, floatSet.RuntimeSet);

        floatSet.ClearRuntimeSet();
        Assert.AreEqual(new List<int> { }, floatSet.RuntimeSet);
    }

    /// <summary>
    /// Test Game event wit listeners
    /// </summary>
    [Test]
    public void TestGameEvent()
    {
        Vector3Variable vector3Event = ScriptableObject.CreateInstance<Vector3Variable>();
        vector3Event.VariableType = VariableType.Event;
        vector3Event.InitializeTypeVariable = InitializeType.Normal;
        vector3Event.InitializeListeners();

        Assert.AreEqual(0, vector3Event.GetListeners().Count);

        GameObject testListener = new GameObject();
        var listener = testListener.AddComponent<Vector3GameEventListener>();
        vector3Event.RegisterListener(listener);

        Assert.AreEqual(1, vector3Event.GetListeners().Count);
        Assert.AreEqual(listener, vector3Event.GetListeners()[0]);

        vector3Event.UnregisterListener(listener);
        Assert.AreEqual(0, vector3Event.GetListeners().Count);
    }
}