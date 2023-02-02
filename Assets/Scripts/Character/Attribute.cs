using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.Serialization;
using UnityEditor;
 
[Serializable]
public class Attribute
{
    [SerializeField] protected float baseVal;
    [SerializeField] protected float boost;
    [SerializeField] protected float mod = 1;
    [SerializeField] float min = float.MinValue;
    [SerializeField] float max = float.MaxValue;

    public float Value {get => GetVal(); set => SetVal(value); } // Actual Value
    public float Base {get => GetBase(); set => SetBase(value); } // Base Value
    public float Boost {get => GetBoost(); set => SetBoost(value); } // Additive change
    public float Mod {get => GetMod(); set => SetMod(value); } // Multiplicative change

    // Changes the boost to ensure that the actual value is under the boost threshold.
    // This will never flip the sign of the boost, instead allowing it to go over the max.
    // getVal will still return the max if this occurs.
    protected virtual void EnsureBoostBounds() {
        float oBoost = boost;

        if (GetVal() > max) {
            boost = max / mod - baseVal;
        }
        if (GetVal() < min) {
            boost = min / mod - baseVal;
        }

        if (Mathf.Sign(oBoost) != Mathf.Sign(boost))  {
            boost = 0;
        }
    }

    // Ensures the value is within the bounds, and returns the value it should be if it is not.
    protected virtual float EnsureBounds(float b) {
        if (b > max) {
            return max;
        }
        else if (b < min) {
            return min;
        }
        return b;
    }

    // Return actual value
    protected virtual float GetVal() {
        return EnsureBounds((baseVal + boost) * mod);
    }
    // Set the boost based on the value entered
    protected virtual void SetVal(float b) {
        boost = b / mod - baseVal;
        EnsureBoostBounds();
    }

    // Get the base value
    protected virtual float GetBase() {
        return baseVal;
    }
    // Set the base value, ensuring that it's properly in the bounds before editing the boost so that the full
    // value is also within the bounds.
    protected virtual void SetBase(float b) {
        if (b * mod > max) {
            b = max / mod;
            boost = 0;
        }
        else if (b * mod < min) {
            b = min / mod;
            boost = 0;
        }
        baseVal = b;
        EnsureBoostBounds();
    }

    // Gets the boost
    protected virtual float GetBoost() {
        return boost;
    }
    // Sets the boost value, then ensures it's properly within the bounds.
    protected virtual void SetBoost(float b) {
        boost = b;
        EnsureBoostBounds();
    }

    // Gets the mod
    protected virtual float GetMod() {
        return mod;
    }
    // Sets the mod, sometimes going above max. If this occurs, when getting the value it will still be the maximum value.
    protected virtual void SetMod(float b) {
        mod = b;
        EnsureBoostBounds();
    }

    public virtual void Reset() {
        boost = 0;
        mod = 1;
    }
}
