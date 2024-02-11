using NUnit.Framework;
using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Tests the int and bool reference
/// </summary>
public class ReferenceTest
{
    /// <summary>
    /// Test int variable
    /// </summary>
    [Test]
    public void TestIntReference()
    {
        IntVariable intVariable = ScriptableObject.CreateInstance<IntVariable>();
        intVariable.VariableType = VariableType.Variable;
        intVariable.InitializeTypeVariable = InitializeType.Normal;

        intVariable.Value = 1;

        IntReference intReference = new IntReference();
        intReference.Value = 2;

        intReference.OverrideVariable(intVariable);

        Assert.AreEqual(2, intReference.Value);

        intReference.Value += 3;
        Assert.AreEqual(5, intReference.Value);

        intReference.OverrideIsVariable(true);
        Assert.AreEqual(1, intReference.Value);
    }

    /// <summary>
    /// Test bool variable
    /// </summary>
    [Test]
    public void TestBoolReference()
    {
        BoolVariable boolVariable = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariable.VariableType = VariableType.Variable;
        boolVariable.InitializeTypeVariable = InitializeType.Normal;

        boolVariable.Value = true;

        BoolReference boolReference = new BoolReference();
        boolReference.Value = false;

        boolReference.OverrideVariable(boolVariable);

        Assert.AreEqual(false, boolReference.Value);

        boolReference.Value = true;
        Assert.AreEqual(true, boolReference.Value);

        boolReference.OverrideIsVariable(true);
        Assert.AreEqual(true, boolReference.Value);
    }
}