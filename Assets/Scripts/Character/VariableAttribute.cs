using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.Serialization;
using UnityEditor;
 
[Serializable]
public class VariableAttribute : Attribute
{
    [SerializeField] float val;

    public float Max {get => GetMod(); set => SetMod(value); } // Multiplicative change

    // Changes the boost to ensure that the actual value is under the boost threshold.
    // This will never flip the sign of the boost, instead allowing it to go over the max.
    // getVal will still return the max if this occurs.

    // Ensures the value is within the bounds, and returns the value it should be if it is not.
    protected override float EnsureBounds(float b) {
        if (b > GetMax()) {
            return GetMax();
        }
        else if (b < 0) {
            return 0;
        }
        return b;
    }

    // Return actual value
    protected override float GetVal() {
        return val;
    }
    // Set the boost based on the value entered
    protected override void SetVal(float b) {
        val = EnsureBounds(b);
    }

    // Get the base value
    protected override float GetBase() {
        return baseVal;
    }
    // Set the base value, ensuring that it's properly in the bounds before editing the boost so that the full
    // value is also within the bounds.
    protected override void SetBase(float b) {
        float oldMax = GetMax();
        baseVal = b;
        val -= (GetMax() - oldMax);
    }

    // Gets the boost
    protected override float GetBoost() {
        return boost;
    }
    // Sets the boost value, then ensures it's properly within the bounds.
    protected override void SetBoost(float b) {
        float oldMax = GetMax();
        boost = b;
        val -= (GetMax() - oldMax);
    }

    // Gets the mod
    protected override float GetMod() {
        return mod;
    }
    // Sets the mod, sometimes going above max. If this occurs, when getting the value it will still be the maximum value.
    protected override void SetMod(float b) {
        float oldMax = GetMax();
        mod = b;
        val -= (GetMax() - oldMax);
    }

    private float GetMax() {
        return (baseVal + boost) * mod;
    }
    private void SetMax(float b) {
        float oldMax = GetMax();
        boost = b / mod - baseVal;
        val -= (b - oldMax);
    }

    public void Refill() {
        val = GetMax();
    }

    public override void Reset() {
        base.Reset();
        val = GetMax();
    }
}
