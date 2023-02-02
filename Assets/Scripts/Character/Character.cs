using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private VariableAttribute health;
    public VariableAttribute Health { get => health; }

    [SerializeField] private Attribute defence;
    public Attribute Defence { get => defence; }

    [SerializeField] private Attribute speed;
    public Attribute Speed { get => defence; }

    [SerializeField] private Attribute damage;
    public Attribute Damage { get => damage; }

    public float TakeDamage(float damage) {
        health.Value -= damage;
        return damage;
    }

    private void Start() {
        health.Refill();
    }
}
